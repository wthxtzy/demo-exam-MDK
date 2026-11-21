namespace ChudoObuv.Core;

/// <summary>Корзина текущего сеанса (до сохранения заказа).</summary>
public sealed class Cart
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items;
    public int Count => _items.Count;
    public decimal Total => _items.Sum(item => item.LineTotal);

    public void Add(CartItem item)
    {
        if (item.Quantity <= 0)
        {
            throw new InvalidOperationException("Количество должно быть больше нуля.");
        }

        var existing = _items.FirstOrDefault(x => x.ProductSizeId == item.ProductSizeId);
        if (existing is not null)
        {
            var newQuantity = existing.Quantity + item.Quantity;
            if (newQuantity > existing.AvailableQuantity)
            {
                throw new InvalidOperationException(
                    $"Недостаточно товара «{existing.ProductName}» размера {existing.Size}. " +
                    $"Доступно: {existing.AvailableQuantity} шт.");
            }

            existing.Quantity = newQuantity;
            return;
        }

        if (item.Quantity > item.AvailableQuantity)
        {
            throw new InvalidOperationException(
                $"Недостаточно товара «{item.ProductName}» размера {item.Size}. " +
                $"Доступно: {item.AvailableQuantity} шт.");
        }

        _items.Add(item);
    }

    public bool Remove(int productSizeId)
    {
        var item = _items.FirstOrDefault(x => x.ProductSizeId == productSizeId);
        return item is not null && _items.Remove(item);
    }

    public void Clear() => _items.Clear();
}
