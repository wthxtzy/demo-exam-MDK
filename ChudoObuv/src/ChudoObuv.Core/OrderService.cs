using System.Globalization;
using Microsoft.Data.Sqlite;

namespace ChudoObuv.Core;

/// <summary>
/// Заказы: чтение, создание, редактирование и удаление.
/// Все изменения остатков выполняются в одной транзакции.
/// </summary>
public sealed class OrderService
{
    private readonly Database _database;

    public OrderService(Database database)
    {
        _database = database;
    }

    public IReadOnlyList<OrderView> GetOrders(int? userId = null)
    {
        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            SELECT o.OrderId,
                   o.OrderDate,
                   o.CustomerName,
                   COALESCE(u.LastName || ' ' || u.FirstName || ' ' || u.MiddleName, '—') AS CreatorName,
                   (SELECT COUNT(*) FROM OrderItems oi WHERE oi.OrderId = o.OrderId) AS Positions,
                   (SELECT COALESCE(SUM(oi.Quantity * oi.UnitPrice), 0)
                    FROM OrderItems oi WHERE oi.OrderId = o.OrderId) AS Total
            FROM Orders o
            LEFT JOIN Users u ON u.UserId = o.UserId
            {(userId.HasValue ? "WHERE o.UserId = @userId" : string.Empty)}
            ORDER BY o.OrderDate DESC, o.OrderId DESC;
            """;

        if (userId.HasValue)
        {
            command.Parameters.AddWithValue("@userId", userId.Value);
        }

        using var reader = command.ExecuteReader();
        var result = new List<OrderView>();
        while (reader.Read())
        {
            result.Add(ReadOrder(reader));
        }

        return result;
    }

    public OrderDetails GetOrderDetails(int orderId)
    {
        var order = GetOrder(orderId)
            ?? throw new InvalidOperationException("Заказ не найден.");

        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT p.ProductName,
                   ps.Size,
                   oi.Quantity,
                   oi.UnitPrice,
                   oi.DiscountPercent,
                   ROUND(oi.Quantity * oi.UnitPrice, 2) AS LineTotal
            FROM OrderItems oi
            JOIN ProductSizes ps ON ps.ProductSizeId = oi.ProductSizeId
            JOIN Products p ON p.ProductId = ps.ProductId
            WHERE oi.OrderId = @orderId
            ORDER BY oi.OrderItemId;
            """;
        command.Parameters.AddWithValue("@orderId", orderId);

        using var reader = command.ExecuteReader();
        var items = new List<OrderItemView>();
        while (reader.Read())
        {
            items.Add(new OrderItemView
            {
                ProductName = reader.GetString(0),
                Size = reader.GetString(1),
                Quantity = reader.GetInt32(2),
                UnitPrice = (decimal)reader.GetDouble(3),
                DiscountPercent = Convert.ToInt32(reader.GetDouble(4)),
                LineTotal = (decimal)reader.GetDouble(5)
            });
        }

        return new OrderDetails { Order = order, Items = items };
    }

    /// <summary>Данные для формы редактирования с учётом текущего остатка и старого количества.</summary>
    public OrderEditData GetOrderForEdit(int orderId)
    {
        var order = GetOrder(orderId)
            ?? throw new InvalidOperationException("Заказ не найден.");

        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT oi.ProductSizeId,
                   ps.ProductId,
                   p.ProductName,
                   ps.Size,
                   p.Price,
                   oi.Quantity,
                   oi.UnitPrice,
                   oi.DiscountPercent,
                   ps.Quantity
            FROM OrderItems oi
            JOIN ProductSizes ps ON ps.ProductSizeId = oi.ProductSizeId
            JOIN Products p ON p.ProductId = ps.ProductId
            WHERE oi.OrderId = @orderId
            ORDER BY oi.OrderItemId;
            """;
        command.Parameters.AddWithValue("@orderId", orderId);

        using var reader = command.ExecuteReader();
        var items = new List<CartItem>();
        while (reader.Read())
        {
            var quantity = reader.GetInt32(5);
            var currentStock = reader.GetInt32(8);
            items.Add(new CartItem
            {
                ProductSizeId = reader.GetInt32(0),
                ProductId = reader.GetInt32(1),
                ProductName = reader.GetString(2),
                Size = reader.GetString(3),
                BasePrice = (decimal)reader.GetDouble(4),
                Quantity = quantity,
                UnitPrice = (decimal)reader.GetDouble(6),
                DiscountPercent = Convert.ToInt32(reader.GetDouble(7)),
                AvailableQuantity = currentStock + quantity
            });
        }

        return new OrderEditData { Order = order, Items = items };
    }

    public int CreateOrder(
        string customerName,
        DateTime orderDate,
        IReadOnlyList<CartItem> items,
        int? userId)
    {
        var customer = ValidateCustomer(customerName);
        var requests = GroupItems(items);

        using var connection = _database.Open();
        using var transaction = connection.BeginTransaction();

        var resolved = ResolveRequests(connection, transaction, requests, null);

        using var insertOrder = connection.CreateCommand();
        insertOrder.Transaction = transaction;
        insertOrder.CommandText = """
            INSERT INTO Orders (OrderDate, CustomerName, UserId)
            VALUES (@orderDate, @customerName, @userId)
            RETURNING OrderId;
            """;
        insertOrder.Parameters.AddWithValue("@orderDate", ToDbDate(orderDate));
        insertOrder.Parameters.AddWithValue("@customerName", customer);
        insertOrder.Parameters.AddWithValue("@userId", userId.HasValue ? userId.Value : DBNull.Value);

        var orderId = Convert.ToInt32(insertOrder.ExecuteScalar());

        ApplyResolvedItems(connection, transaction, orderId, resolved);
        transaction.Commit();

        return orderId;
    }

    public void UpdateOrder(
        int orderId,
        string customerName,
        DateTime orderDate,
        IReadOnlyList<CartItem> items)
    {
        var customer = ValidateCustomer(customerName);
        var requests = GroupItems(items);

        using var connection = _database.Open();
        using var transaction = connection.BeginTransaction();

        if (!OrderExists(connection, transaction, orderId))
        {
            throw new InvalidOperationException("Заказ не найден.");
        }

        var oldItems = LoadOrderItems(connection, transaction, orderId);

        foreach (var oldItem in oldItems)
        {
            RestoreStock(connection, transaction, oldItem.ProductSizeId, oldItem.Quantity);
        }

        using (var deleteItems = connection.CreateCommand())
        {
            deleteItems.Transaction = transaction;
            deleteItems.CommandText = "DELETE FROM OrderItems WHERE OrderId = @orderId;";
            deleteItems.Parameters.AddWithValue("@orderId", orderId);
            deleteItems.ExecuteNonQuery();
        }

        var priceOverrides = oldItems
            .GroupBy(item => item.ProductSizeId)
            .ToDictionary(
                group => group.Key,
                group => new PriceSnapshot(group.First().UnitPrice, group.First().DiscountPercent));

        var resolved = ResolveRequests(connection, transaction, requests, priceOverrides);

        using (var updateOrder = connection.CreateCommand())
        {
            updateOrder.Transaction = transaction;
            updateOrder.CommandText = """
                UPDATE Orders
                SET OrderDate = @orderDate,
                    CustomerName = @customerName
                WHERE OrderId = @orderId;
                """;
            updateOrder.Parameters.AddWithValue("@orderDate", ToDbDate(orderDate));
            updateOrder.Parameters.AddWithValue("@customerName", customer);
            updateOrder.Parameters.AddWithValue("@orderId", orderId);
            updateOrder.ExecuteNonQuery();
        }

        ApplyResolvedItems(connection, transaction, orderId, resolved);
        transaction.Commit();
    }

    public void DeleteOrder(int orderId)
    {
        using var connection = _database.Open();
        using var transaction = connection.BeginTransaction();

        if (!OrderExists(connection, transaction, orderId))
        {
            throw new InvalidOperationException("Заказ не найден.");
        }

        var oldItems = LoadOrderItems(connection, transaction, orderId);
        foreach (var oldItem in oldItems)
        {
            RestoreStock(connection, transaction, oldItem.ProductSizeId, oldItem.Quantity);
        }

        using var deleteOrder = connection.CreateCommand();
        deleteOrder.Transaction = transaction;
        deleteOrder.CommandText = "DELETE FROM Orders WHERE OrderId = @orderId;";
        deleteOrder.Parameters.AddWithValue("@orderId", orderId);
        deleteOrder.ExecuteNonQuery();

        transaction.Commit();
    }

    private OrderView? GetOrder(int orderId)
    {
        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT o.OrderId,
                   o.OrderDate,
                   o.CustomerName,
                   COALESCE(u.LastName || ' ' || u.FirstName || ' ' || u.MiddleName, '—') AS CreatorName,
                   (SELECT COUNT(*) FROM OrderItems oi WHERE oi.OrderId = o.OrderId) AS Positions,
                   (SELECT COALESCE(SUM(oi.Quantity * oi.UnitPrice), 0)
                    FROM OrderItems oi WHERE oi.OrderId = o.OrderId) AS Total
            FROM Orders o
            LEFT JOIN Users u ON u.UserId = o.UserId
            WHERE o.OrderId = @orderId;
            """;
        command.Parameters.AddWithValue("@orderId", orderId);

        using var reader = command.ExecuteReader();
        return reader.Read() ? ReadOrder(reader) : null;
    }

    private static OrderView ReadOrder(SqliteDataReader reader) => new()
    {
        OrderId = reader.GetInt32(0),
        OrderDate = DateTime.ParseExact(
            reader.GetString(1),
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture),
        CustomerName = reader.GetString(2),
        CreatorName = reader.GetString(3),
        Positions = Convert.ToInt32(reader.GetInt64(4)),
        Total = Math.Round((decimal)reader.GetDouble(5), 2, MidpointRounding.AwayFromZero)
    };

    private static List<ItemRequest> GroupItems(IReadOnlyList<CartItem> items)
    {
        if (items is null || items.Count == 0)
        {
            throw new InvalidOperationException("Заказ должен содержать хотя бы один товар.");
        }

        var grouped = items
            .Where(item => item.Quantity > 0)
            .GroupBy(item => item.ProductSizeId)
            .Select(group => new ItemRequest(group.Key, group.Sum(item => item.Quantity)))
            .ToList();

        if (grouped.Count == 0)
        {
            throw new InvalidOperationException("Заказ должен содержать хотя бы один товар.");
        }

        return grouped;
    }

    private static string ValidateCustomer(string customerName)
    {
        var customer = (customerName ?? string.Empty).Trim();
        if (customer.Length == 0)
        {
            throw new InvalidOperationException("Укажите ФИО клиента.");
        }

        if (customer.Length > 200)
        {
            throw new InvalidOperationException("ФИО клиента слишком длинное.");
        }

        return customer;
    }

    private static string ToDbDate(DateTime date) =>
        date.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static bool OrderExists(
        SqliteConnection connection,
        SqliteTransaction transaction,
        int orderId)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "SELECT COUNT(*) FROM Orders WHERE OrderId = @orderId;";
        command.Parameters.AddWithValue("@orderId", orderId);
        return Convert.ToInt64(command.ExecuteScalar()) > 0;
    }

    private static List<OldOrderItem> LoadOrderItems(
        SqliteConnection connection,
        SqliteTransaction transaction,
        int orderId)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            SELECT ProductSizeId, Quantity, UnitPrice, DiscountPercent
            FROM OrderItems
            WHERE OrderId = @orderId;
            """;
        command.Parameters.AddWithValue("@orderId", orderId);

        using var reader = command.ExecuteReader();
        var items = new List<OldOrderItem>();
        while (reader.Read())
        {
            items.Add(new OldOrderItem(
                reader.GetInt32(0),
                reader.GetInt32(1),
                (decimal)reader.GetDouble(2),
                Convert.ToInt32(reader.GetDouble(3))));
        }

        return items;
    }

    private static void RestoreStock(
        SqliteConnection connection,
        SqliteTransaction transaction,
        int productSizeId,
        int quantity)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            UPDATE ProductSizes
            SET Quantity = Quantity + @quantity
            WHERE ProductSizeId = @productSizeId;
            """;
        command.Parameters.AddWithValue("@quantity", quantity);
        command.Parameters.AddWithValue("@productSizeId", productSizeId);
        command.ExecuteNonQuery();
    }

    private static List<ResolvedItem> ResolveRequests(
        SqliteConnection connection,
        SqliteTransaction transaction,
        IReadOnlyList<ItemRequest> requests,
        IReadOnlyDictionary<int, PriceSnapshot>? priceOverrides)
    {
        var aprilStart = new DateTime(DateTime.Today.Year, 4, 1).ToString("yyyy-MM-dd");
        var mayStart = new DateTime(DateTime.Today.Year, 5, 1).ToString("yyyy-MM-dd");
        var result = new List<ResolvedItem>();

        foreach (var request in requests)
        {
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = """
                SELECT p.ProductId,
                       p.ProductName,
                       ps.Size,
                       ps.Quantity,
                       p.Price,
                       CASE WHEN EXISTS (
                           SELECT 1
                           FROM OrderItems oi
                           JOIN Orders o ON o.OrderId = oi.OrderId
                           JOIN ProductSizes ps2 ON ps2.ProductSizeId = oi.ProductSizeId
                           WHERE ps2.ProductId = p.ProductId
                             AND o.OrderDate >= @aprilStart
                             AND o.OrderDate < @mayStart
                       ) THEN 1 ELSE 0 END AS HasAprilOrder
                FROM ProductSizes ps
                JOIN Products p ON p.ProductId = ps.ProductId
                WHERE ps.ProductSizeId = @productSizeId;
                """;
            command.Parameters.AddWithValue("@productSizeId", request.ProductSizeId);
            command.Parameters.AddWithValue("@aprilStart", aprilStart);
            command.Parameters.AddWithValue("@mayStart", mayStart);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                throw new InvalidOperationException("Товарная позиция не найдена.");
            }

            var productName = reader.GetString(1);
            var size = reader.GetString(2);
            var available = reader.GetInt32(3);
            var basePrice = (decimal)reader.GetDouble(4);
            var hasAprilOrder = reader.GetInt64(5) == 1;

            if (request.Quantity > available)
            {
                throw new InvalidOperationException(
                    $"Недостаточно товара «{productName}» размера {size}. " +
                    $"Доступно: {available} шт., запрошено: {request.Quantity} шт.");
            }

            decimal unitPrice;
            int discountPercent;

            if (priceOverrides is not null
                && priceOverrides.TryGetValue(request.ProductSizeId, out var snapshot))
            {
                unitPrice = snapshot.UnitPrice;
                discountPercent = snapshot.DiscountPercent;
            }
            else
            {
                discountPercent = DiscountPolicy.GetDiscountPercent(hasAprilOrder);
                unitPrice = DiscountPolicy.Apply(basePrice, discountPercent);
            }

            result.Add(new ResolvedItem
            {
                ProductSizeId = request.ProductSizeId,
                ProductName = productName,
                Size = size,
                Quantity = request.Quantity,
                UnitPrice = unitPrice,
                DiscountPercent = discountPercent
            });
        }

        return result;
    }

    private static void ApplyResolvedItems(
        SqliteConnection connection,
        SqliteTransaction transaction,
        int orderId,
        IReadOnlyList<ResolvedItem> items)
    {
        foreach (var item in items)
        {
            using (var updateStock = connection.CreateCommand())
            {
                updateStock.Transaction = transaction;
                updateStock.CommandText = """
                    UPDATE ProductSizes
                    SET Quantity = Quantity - @quantity
                    WHERE ProductSizeId = @productSizeId
                      AND Quantity >= @quantity;
                    """;
                updateStock.Parameters.AddWithValue("@quantity", item.Quantity);
                updateStock.Parameters.AddWithValue("@productSizeId", item.ProductSizeId);

                if (updateStock.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        $"Не удалось зарезервировать товар «{item.ProductName}» размера {item.Size}.");
                }
            }

            using var insertItem = connection.CreateCommand();
            insertItem.Transaction = transaction;
            insertItem.CommandText = """
                INSERT INTO OrderItems
                    (OrderId, ProductSizeId, Quantity, UnitPrice, DiscountPercent)
                VALUES
                    (@orderId, @productSizeId, @quantity, @unitPrice, @discountPercent);
                """;
            insertItem.Parameters.AddWithValue("@orderId", orderId);
            insertItem.Parameters.AddWithValue("@productSizeId", item.ProductSizeId);
            insertItem.Parameters.AddWithValue("@quantity", item.Quantity);
            insertItem.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
            insertItem.Parameters.AddWithValue("@discountPercent", item.DiscountPercent);
            insertItem.ExecuteNonQuery();
        }
    }

    private readonly record struct ItemRequest(int ProductSizeId, int Quantity);
    private readonly record struct PriceSnapshot(decimal UnitPrice, int DiscountPercent);
    private sealed record OldOrderItem(
        int ProductSizeId,
        int Quantity,
        decimal UnitPrice,
        int DiscountPercent);

    private sealed class ResolvedItem
    {
        public int ProductSizeId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string Size { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public int DiscountPercent { get; init; }
    }
}
