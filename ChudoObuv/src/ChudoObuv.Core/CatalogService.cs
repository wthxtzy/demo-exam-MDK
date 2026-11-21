using Microsoft.Data.Sqlite;

namespace ChudoObuv.Core;

/// <summary>Каталог товаров, остатки и скидки.</summary>
public sealed class CatalogService
{
    private readonly Database _database;

    public CatalogService(Database database)
    {
        _database = database;
    }

    public IReadOnlyList<IdName> GetCategories()
    {
        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT CategoryId, CategoryName FROM Categories ORDER BY CategoryName;";

        using var reader = command.ExecuteReader();
        var result = new List<IdName>();
        while (reader.Read())
        {
            result.Add(new IdName(reader.GetInt32(0), reader.GetString(1)));
        }

        return result;
    }

    /// <summary>
    /// Возвращает каталог с ценой со скидкой и суммарным остатком.
    /// Поиск выполняется в C#, чтобы корректно работать с кириллицей
    /// независимо от настроек SQLite LIKE.
    /// </summary>
    public IReadOnlyList<ProductView> GetProducts(
        string? search = null,
        int? categoryId = null,
        CatalogSort sort = CatalogSort.NameAsc)
    {
        var searchText = (search ?? string.Empty).Trim();
        var aprilStart = new DateTime(DateTime.Today.Year, 4, 1).ToString("yyyy-MM-dd");
        var mayStart = new DateTime(DateTime.Today.Year, 5, 1).ToString("yyyy-MM-dd");

        var orderBy = sort switch
        {
            CatalogSort.NameDesc => "ProductName COLLATE NOCASE DESC, Manufacturer COLLATE NOCASE",
            CatalogSort.PriceAsc => "EffectivePrice ASC, ProductName COLLATE NOCASE",
            CatalogSort.PriceDesc => "EffectivePrice DESC, ProductName COLLATE NOCASE",
            CatalogSort.QuantityAsc => "TotalQuantity ASC, ProductName COLLATE NOCASE",
            CatalogSort.QuantityDesc => "TotalQuantity DESC, ProductName COLLATE NOCASE",
            _ => "ProductName COLLATE NOCASE ASC, Manufacturer COLLATE NOCASE"
        };

        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            WITH ProductInfo AS (
                SELECT p.ProductId,
                       c.CategoryId,
                       c.CategoryName,
                       sc.SubcategoryName,
                       p.ProductName,
                       p.Manufacturer,
                       p.Description,
                       p.Composition,
                       p.Price,
                       p.ImageName,
                       COALESCE((
                           SELECT SUM(ps.Quantity)
                           FROM ProductSizes ps
                           WHERE ps.ProductId = p.ProductId
                       ), 0) AS TotalQuantity,
                       CASE WHEN EXISTS (
                           SELECT 1
                           FROM OrderItems oi
                           JOIN Orders o ON o.OrderId = oi.OrderId
                           JOIN ProductSizes ps2 ON ps2.ProductSizeId = oi.ProductSizeId
                           WHERE ps2.ProductId = p.ProductId
                             AND o.OrderDate >= @aprilStart
                             AND o.OrderDate < @mayStart
                       ) THEN 1 ELSE 0 END AS HasAprilOrder
                FROM Products p
                JOIN Subcategories sc ON sc.SubcategoryId = p.SubcategoryId
                JOIN Categories c ON c.CategoryId = sc.CategoryId
            )
            SELECT ProductId,
                   CategoryName,
                   SubcategoryName,
                   ProductName,
                   Manufacturer,
                   Description,
                   Composition,
                   Price,
                   ImageName,
                   TotalQuantity,
                   HasAprilOrder,
                   CASE WHEN HasAprilOrder = 1 THEN Price
                        ELSE ROUND(Price * 0.75, 2) END AS EffectivePrice
            FROM ProductInfo
            WHERE (@categoryId = 0 OR CategoryId = @categoryId)
            ORDER BY {orderBy};
            """;

        command.Parameters.AddWithValue("@aprilStart", aprilStart);
        command.Parameters.AddWithValue("@mayStart", mayStart);
        command.Parameters.AddWithValue("@categoryId", categoryId ?? 0);

        using var reader = command.ExecuteReader();
        var result = new List<ProductView>();
        while (reader.Read())
        {
            var hasAprilOrder = reader.GetInt64(10) == 1;
            var discountPercent = DiscountPolicy.GetDiscountPercent(hasAprilOrder);
            var product = new ProductView
            {
                ProductId = reader.GetInt32(0),
                Category = reader.GetString(1),
                Subcategory = reader.GetString(2),
                Name = reader.GetString(3),
                Manufacturer = reader.GetString(4),
                Description = reader.GetString(5),
                Composition = reader.GetString(6),
                Price = (decimal)reader.GetDouble(7),
                ImageName = reader.GetString(8),
                TotalQuantity = Convert.ToInt32(reader.GetInt64(9)),
                HasAprilOrders = hasAprilOrder,
                DiscountPercent = discountPercent
            };

            if (searchText.Length == 0
                || product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.Manufacturer.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.Category.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.Subcategory.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(product);
            }
        }

        return result;
    }

    public IReadOnlyList<ProductSizeView> GetSizes(int productId)
    {
        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT ProductSizeId, Size, Quantity
            FROM ProductSizes
            WHERE ProductId = @productId
            ORDER BY CAST(Size AS REAL), Size;
            """;
        command.Parameters.AddWithValue("@productId", productId);

        using var reader = command.ExecuteReader();
        var result = new List<ProductSizeView>();
        while (reader.Read())
        {
            result.Add(new ProductSizeView
            {
                ProductSizeId = reader.GetInt32(0),
                Size = reader.GetString(1),
                Quantity = reader.GetInt32(2)
            });
        }

        return result;
    }

    public ProductSizeView? GetSize(int productSizeId)
    {
        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT ProductSizeId, Size, Quantity
            FROM ProductSizes
            WHERE ProductSizeId = @id;
            """;
        command.Parameters.AddWithValue("@id", productSizeId);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new ProductSizeView
        {
            ProductSizeId = reader.GetInt32(0),
            Size = reader.GetString(1),
            Quantity = reader.GetInt32(2)
        };
    }

    /// <summary>Проверка, что в апреле текущего года у товара были заказы.</summary>
    public bool HasAprilOrders(int productId)
    {
        var aprilStart = new DateTime(DateTime.Today.Year, 4, 1).ToString("yyyy-MM-dd");
        var mayStart = new DateTime(DateTime.Today.Year, 5, 1).ToString("yyyy-MM-dd");

        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT EXISTS (
                SELECT 1
                FROM OrderItems oi
                JOIN Orders o ON o.OrderId = oi.OrderId
                JOIN ProductSizes ps ON ps.ProductSizeId = oi.ProductSizeId
                WHERE ps.ProductId = @productId
                  AND o.OrderDate >= @aprilStart
                  AND o.OrderDate < @mayStart
            );
            """;
        command.Parameters.AddWithValue("@productId", productId);
        command.Parameters.AddWithValue("@aprilStart", aprilStart);
        command.Parameters.AddWithValue("@mayStart", mayStart);

        return Convert.ToInt64(command.ExecuteScalar()) == 1;
    }
}
