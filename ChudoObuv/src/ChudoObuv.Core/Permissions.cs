namespace ChudoObuv.Core;

/// <summary>Правила доступа по ролям из предметной области.</summary>
public static class Permissions
{
    public static bool CanOrder(UserRole role) =>
        role is UserRole.AuthorizedUser or UserRole.Manager or UserRole.Administrator;

    public static bool CanFilterCatalog(UserRole role) => role != UserRole.Guest;

    public static bool CanViewOrders(UserRole role) => role != UserRole.Guest;

    public static bool CanViewAllOrders(UserRole role) =>
        role is UserRole.Manager or UserRole.Administrator;

    public static bool CanCreateOrder(UserRole role) =>
        role is UserRole.Manager or UserRole.Administrator;

    public static bool CanDeleteOrder(UserRole role) =>
        role is UserRole.Manager or UserRole.Administrator;

    public static bool CanEditOrder(UserRole role) => role == UserRole.Administrator;
}
