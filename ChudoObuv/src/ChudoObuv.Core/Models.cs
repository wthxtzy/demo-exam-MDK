using System.Globalization;

namespace ChudoObuv.Core;

/// <summary>Роль пользователя. Числовые значения совпадают с RoleId в БД.</summary>
public enum UserRole
{
    Guest = 0,
    Administrator = 1,
    Manager = 2,
    AuthorizedUser = 3
}

public static class RoleNames
{
    public const string Administrator = "Администратор";
    public const string Manager = "Менеджер";
    public const string AuthorizedUser = "Авторизованный пользователь";

    public static string ToDisplayName(UserRole role) => role switch
    {
        UserRole.Administrator => Administrator,
        UserRole.Manager => Manager,
        UserRole.AuthorizedUser => AuthorizedUser,
        _ => "Гость"
    };
}

/// <summary>Сеанс вошедшего пользователя. Для гостя UserId = null.</summary>
public sealed record UserSession(int? UserId, string Login, string FullName, UserRole Role)
{
    public bool IsGuest => Role == UserRole.Guest;
    public string RoleDisplayName => RoleNames.ToDisplayName(Role);
}

public sealed record LoginOption(string Login, string FullName, string RoleName)
{
    public override string ToString() => $"{Login} — {FullName} ({RoleName})";
}

public sealed record IdName(int Id, string Name)
{
    public override string ToString() => Name;
}

public enum CatalogSort
{
    NameAsc,
    NameDesc,
    PriceAsc,
    PriceDesc,
    QuantityAsc,
    QuantityDesc
}

public sealed record CatalogSortOption(string Name, CatalogSort Sort)
{
    public override string ToString() => Name;
}

public static class DiscountPolicy
{
    /// <summary>Скидка на товары, которые не заказывали в апреле текущего года.</summary>
    public const int NoAprilOrdersDiscount = 25;

    public static int GetDiscountPercent(bool hasAprilOrders) =>
        hasAprilOrders ? 0 : NoAprilOrdersDiscount;

    public static decimal Apply(decimal price, int discountPercent) =>
        Math.Round(price * (100 - discountPercent) / 100m, 2, MidpointRounding.AwayFromZero);
}

public sealed class ProductView
{
    public int ProductId { get; init; }
    public string Category { get; init; } = string.Empty;
    public string Subcategory { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Manufacturer { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Composition { get; init; } = string.Empty;
    public string ImageName { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int DiscountPercent { get; init; }
    public int TotalQuantity { get; init; }
    public bool HasAprilOrders { get; init; }

    public decimal PriceWithDiscount => DiscountPolicy.Apply(Price, DiscountPercent);
    public bool IsLowStock => TotalQuantity <= 3;
    public string StockLabel => TotalQuantity > 5 ? "много" : "мало";
    public string DiscountLabel => DiscountPercent == 0 ? "—" : $"{DiscountPercent}%";
    public string PriceLabel => Money(PriceWithDiscount);

    public static string Money(decimal value) =>
        value.ToString("N2", CultureInfo.GetCultureInfo("ru-RU")) + " руб.";

    public override string ToString() => $"{Name} — {Manufacturer} — {PriceLabel}";
}

public sealed class ProductSizeView
{
    public int ProductSizeId { get; init; }
    public string Size { get; init; } = string.Empty;
    public int Quantity { get; init; }

    public override string ToString() => Quantity > 0
        ? $"{Size} — {Quantity} шт."
        : $"{Size} — нет в наличии";
}

public sealed class CartItem
{
    public int ProductSizeId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string Size { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
    public int DiscountPercent { get; init; }
    public decimal UnitPrice { get; init; }
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }

    public decimal LineTotal => Math.Round(UnitPrice * Quantity, 2, MidpointRounding.AwayFromZero);
}

public sealed class OrderView
{
    public int OrderId { get; init; }
    public DateTime OrderDate { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string CreatorName { get; init; } = "—";
    public int Positions { get; init; }
    public decimal Total { get; init; }

    public string DateLabel => OrderDate.ToString("dd.MM.yyyy");
    public string TotalLabel => ProductView.Money(Total);
}

public sealed class OrderItemView
{
    public string ProductName { get; init; } = string.Empty;
    public string Size { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public int DiscountPercent { get; init; }
    public decimal LineTotal { get; init; }

    public string UnitPriceLabel => ProductView.Money(UnitPrice);
    public string LineTotalLabel => ProductView.Money(LineTotal);
    public string DiscountLabel => DiscountPercent == 0 ? "—" : $"{DiscountPercent}%";
}

public sealed class OrderDetails
{
    public OrderView Order { get; init; } = new();
    public IReadOnlyList<OrderItemView> Items { get; init; } = Array.Empty<OrderItemView>();
}

public sealed class OrderEditData
{
    public OrderView Order { get; init; } = new();
    public IReadOnlyList<CartItem> Items { get; init; } = Array.Empty<CartItem>();
}
