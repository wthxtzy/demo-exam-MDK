using ChudoObuv.Core;

var failures = 0;
var checks = 0;

void Check(bool condition, string message)
{
    checks++;
    if (condition)
    {
        Console.WriteLine($"  OK   {message}");
        return;
    }

    failures++;
    Console.WriteLine($"  FAIL {message}");
}

void CheckThrows<TException>(Action action, string message) where TException : Exception
{
    checks++;
    try
    {
        action();
        failures++;
        Console.WriteLine($"  FAIL {message} (исключение не возникло)");
    }
    catch (TException)
    {
        Console.WriteLine($"  OK   {message}");
    }
    catch (Exception ex)
    {
        failures++;
        Console.WriteLine($"  FAIL {message} (получено {ex.GetType().Name}: {ex.Message})");
    }
}

var databasePath = Path.Combine(Path.GetTempPath(), $"chudo_obuv_test_{Guid.NewGuid():N}.db");
Console.WriteLine($"Тестовая БД: {databasePath}");
Console.WriteLine();

try
{
    var database = new Database(databasePath);
    database.EnsureCreated();

    var auth = new AuthService(database);
    var catalog = new CatalogService(database);
    var orders = new OrderService(database);

    Console.WriteLine("1. Пользователи и роли");
    var loginOptions = auth.GetLoginOptions();
    Check(loginOptions.Count == 20, "загружено 20 пользователей");
    var admin = auth.Authenticate("isivanov");
    Check(admin is not null, "администратор isivanov найден");
    Check(admin!.Role == UserRole.Administrator, "роль isivanov — администратор");
    Check(admin.FullName == "Иванов Иван Сергеевич", "ФИО администратора корректно");
    Check(auth.Authenticate("ISIVANOV")?.Role == UserRole.Administrator, "логин сравнивается без учёта регистра");
    Check(auth.Authenticate("no_such_user") is null, "неизвестный логин не найден");

    var manager = auth.Authenticate("papetrov")!;
    var authorized = auth.Authenticate("asidorova")!;
    Check(manager.Role == UserRole.Manager, "роль papetrov — менеджер");
    Check(authorized.Role == UserRole.AuthorizedUser, "роль asidorova — авторизованный пользователь");

    Console.WriteLine();
    Console.WriteLine("2. Права доступа");
    Check(!Permissions.CanOrder(UserRole.Guest) && !Permissions.CanFilterCatalog(UserRole.Guest), "гость не может заказывать и фильтровать");
    Check(Permissions.CanOrder(UserRole.AuthorizedUser) && !Permissions.CanViewAllOrders(UserRole.AuthorizedUser), "пользователь заказывает, но не видит все заказы");
    Check(Permissions.CanCreateOrder(UserRole.Manager) && Permissions.CanDeleteOrder(UserRole.Manager) && !Permissions.CanEditOrder(UserRole.Manager), "менеджер создаёт/удаляет, но не редактирует");
    Check(Permissions.CanEditOrder(UserRole.Administrator) && Permissions.CanViewAllOrders(UserRole.Administrator), "администратор редактирует и видит все заказы");

    Console.WriteLine();
    Console.WriteLine("3. Каталог и скидки");
    var products = catalog.GetProducts();
    Check(products.Count == 31, "в каталоге 31 модель");
    Check(catalog.GetCategories().Count == 3, "3 категории");

    var product1 = products.Single(p => p.ProductId == 1);
    Check(product1.TotalQuantity == 20, "остаток товара 1 суммарно 20 шт.");
    Check(product1.DiscountPercent == 0, "товар 1 заказывали в апреле — скидки нет");
    Check(product1.PriceWithDiscount == 2190m, "цена товара 1 без скидки 2190 ₽");

    var product11 = products.Single(p => p.ProductId == 11);
    Check(product11.DiscountPercent == 25, "товар 11 не заказывали в апреле — скидка 25%");
    Check(product11.PriceWithDiscount == 11250m, "цена товара 11 со скидкой 11 250 ₽");
    Check(product11.IsLowStock, "у товара 11 остаток 2 — подсвечивается (порог 3)");

    var product7 = products.Single(p => p.ProductId == 7);
    Check(product7.TotalQuantity == 3 && product7.IsLowStock, "товар 7 с остатком 3 подсвечивается");
    Check(product7.StockLabel == "мало", "для остатка 3 выводится «мало»");

    var product4 = products.Single(p => p.ProductId == 4);
    Check(product4.TotalQuantity == 15 && product4.StockLabel == "много", "для остатка 15 выводится «много»");

    var searchResult = catalog.GetProducts("сапоги");
    Check(searchResult.Count >= 5, "поиск по слову «сапоги» находит товары (регистр не важен)");
    Check(catalog.GetProducts("", catalog.GetCategories().Single(c => c.Name == "Женская обувь").Id).Count == 13,
        "фильтр по категории «Женская обувь» даёт 13 моделей");

    var sortedByPrice = catalog.GetProducts("", null, CatalogSort.PriceAsc).ToList();
    var priceOrderOk = true;
    for (var i = 1; i < sortedByPrice.Count; i++)
    {
        if (sortedByPrice[i - 1].PriceWithDiscount > sortedByPrice[i].PriceWithDiscount)
        {
            priceOrderOk = false;
            break;
        }
    }
    Check(priceOrderOk, "сортировка по цене возрастает");

    var sizes1 = catalog.GetSizes(1).ToList();
    Check(sizes1.Count == 12, "у товара 1 12 размеров");
    Check(sizes1.Sum(s => s.Quantity) == 20, "сумма остатков по размерам товара 1 равна 20");
    Check(catalog.HasAprilOrders(1) && !catalog.HasAprilOrders(11), "признак апрельских заказов определяется верно");

    Console.WriteLine();
    Console.WriteLine("4. Заказы из импорта");
    var allOrders = orders.GetOrders();
    Check(allOrders.Count == 10, "загружено 10 импортированных заказов");
    var order1 = orders.GetOrderDetails(1);
    Check(order1.Items.Count == 2, "в заказе 1 две позиции");
    Check(order1.Order.Total == 7770m, "сумма заказа 1 — 7 770 ₽");
    Check(orders.GetOrderDetails(7).Order.Total == 61500m, "сумма заказа 7 — 61 500 ₽");
    Check(orders.GetOrders(authorized.UserId).Count == 1, "Сидорова видит только свой импортированный заказ");

    Console.WriteLine();
    Console.WriteLine("5. Создание заказа и списание остатка");
    var sizes11 = catalog.GetSizes(11).ToList();
    var size39 = sizes11.Single(s => s.Size == "39");
    Check(size39.Quantity == 1, "размер 39 товара 11 есть в наличии (1 шт.)");

    var newOrderId = orders.CreateOrder(
        "Тестовый Клиент",
        DateTime.Today,
        new[]
        {
            new CartItem
            {
                ProductSizeId = size39.ProductSizeId,
                ProductId = 11,
                ProductName = product11.Name,
                Size = "39",
                BasePrice = product11.Price,
                DiscountPercent = product11.DiscountPercent,
                UnitPrice = product11.PriceWithDiscount,
                Quantity = 1,
                AvailableQuantity = 1
            }
        },
        admin.UserId);

    Check(newOrderId > 10, "новый заказ получил номер");
    var created = orders.GetOrderDetails(newOrderId);
    Check(created.Items.Single().UnitPrice == 11250m, "в заказ записана цена со скидкой 11 250 ₽");
    Check(created.Order.Total == 11250m, "итог нового заказа 11 250 ₽");
    Check(catalog.GetSizes(11).Single(s => s.Size == "39").Quantity == 0, "остаток размера 39 уменьшился до 0");
    Check(orders.GetOrders(admin.UserId).Any(o => o.OrderId == newOrderId), "администратор видит созданный заказ");

    CheckThrows<InvalidOperationException>(() =>
        orders.CreateOrder(
            "Тестовый Клиент",
            DateTime.Today,
            new[]
            {
                new CartItem
                {
                    ProductSizeId = size39.ProductSizeId,
                    ProductId = 11,
                    ProductName = product11.Name,
                    Size = "39",
                    Quantity = 1,
                    AvailableQuantity = 1
                }
            },
            admin.UserId), "повторная продажа того же размера запрещена");

    orders.DeleteOrder(newOrderId);
    Check(catalog.GetSizes(11).Single(s => s.Size == "39").Quantity == 1, "после удаления заказа остаток восстановлен");
    Check(orders.GetOrders().Count == 10, "после удаления снова 10 заказов");

    Console.WriteLine();
    Console.WriteLine("6. Редактирование заказа");
    var orderToEdit = orders.CreateOrder(
        "Клиент Для Редактирования",
        DateTime.Today,
        new[]
        {
            new CartItem
            {
                ProductSizeId = size39.ProductSizeId,
                ProductId = 11,
                ProductName = product11.Name,
                Size = "39",
                BasePrice = product11.Price,
                DiscountPercent = product11.DiscountPercent,
                UnitPrice = product11.PriceWithDiscount,
                Quantity = 1,
                AvailableQuantity = 1
            }
        },
        admin.UserId);

    var editData = orders.GetOrderForEdit(orderToEdit);
    Check(editData.Items.Single().AvailableQuantity == 1, "при редактировании учитывается старое количество");
    Check(editData.Order.CustomerName == "Клиент Для Редактирования", "ФИО клиента загружено");

    var product12 = products.Single(p => p.ProductId == 12);
    var size37 = catalog.GetSizes(12).Single(s => s.Size == "37");
    Check(size37.Quantity == 2, "размер 37 товара 12 есть в наличии (2 шт.)");

    orders.UpdateOrder(
        orderToEdit,
        "Клиент После Редактирования",
        DateTime.Today,
        new[]
        {
            new CartItem
            {
                ProductSizeId = size37.ProductSizeId,
                ProductId = 12,
                ProductName = product12.Name,
                Size = "37",
                BasePrice = product12.Price,
                DiscountPercent = product12.DiscountPercent,
                UnitPrice = product12.PriceWithDiscount,
                Quantity = 1,
                AvailableQuantity = 2
            }
        });

    var updated = orders.GetOrderDetails(orderToEdit);
    Check(updated.Order.CustomerName == "Клиент После Редактирования", "ФИО клиента изменено");
    Check(updated.Items.Single().ProductName == product12.Name, "позиция заказа заменена");
    Check(updated.Order.Total == 12750m, "итог после редактирования 12 750 ₽");
    Check(catalog.GetSizes(11).Single(s => s.Size == "39").Quantity == 1, "старый размер возвращён в наличие");
    Check(catalog.GetSizes(12).Single(s => s.Size == "37").Quantity == 1, "новый размер списан");

    orders.DeleteOrder(orderToEdit);
    Check(catalog.GetSizes(12).Single(s => s.Size == "37").Quantity == 2, "после удаления отредактированного заказа остаток восстановлен");

    Console.WriteLine();
    Console.WriteLine("7. Целостность базы данных");
    using (var connection = database.Open())
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_key_check;";
            using var reader = command.ExecuteReader();
            var foreignKeyErrors = 0;
            while (reader.Read())
            {
                foreignKeyErrors++;
            }
            Check(foreignKeyErrors == 0, "нарушений внешних ключей нет");
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT SUM(Quantity) FROM ProductSizes;";
            Check(Convert.ToInt64(command.ExecuteScalar()) == 372, "суммарный остаток 372 шт.");
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT COUNT(*) FROM ProductSizes ps JOIN Products p ON p.ProductId = ps.ProductId;";
            Check(Convert.ToInt64(command.ExecuteScalar()) == 128, "128 товарных позиций (модель + размер)");
        }
    }

    Console.WriteLine();
    Console.WriteLine(failures == 0
        ? $"ВСЕ ТЕСТЫ ПРОЙДЕНЫ ({checks} проверок)."
        : $"ПРОВАЛЕНО ПРОВЕРОК: {failures} ИЗ {checks}.");
}
catch (Exception ex)
{
    failures++;
    Console.WriteLine("НЕОБРАБОТАННАЯ ОШИБКА ТЕСТОВ:");
    Console.WriteLine(ex);
}
finally
{
    try
    {
        if (File.Exists(databasePath))
        {
            File.Delete(databasePath);
        }
    }
    catch
    {
        // Не мешает результату тестов.
    }
}

return failures == 0 ? 0 : 1;
