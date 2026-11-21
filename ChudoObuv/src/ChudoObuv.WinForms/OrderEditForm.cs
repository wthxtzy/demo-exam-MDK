using ChudoObuv.Core;

namespace ChudoObuv.WinForms;

/// <summary>Создание и редактирование заказа.</summary>
public partial class OrderEditForm : Form
{
    private readonly UserSession _session = null!;
    private readonly int? _orderId;
    private readonly CatalogService _catalogService = null!;
    private readonly OrderService _orderService = null!;
    private readonly Cart _cart = new();

    private List<ProductView> _products = new();

    public int? SavedOrderId { get; private set; }

    public OrderEditForm()
    {
        InitializeComponent();
        ApplyTheme();
    }

    public OrderEditForm(
        Database database,
        UserSession session,
        IReadOnlyList<CartItem>? initialItems,
        int? orderId) : this()
    {
        _session = session;
        _orderId = orderId;
        _catalogService = new CatalogService(database);
        _orderService = new OrderService(database);

        Text = orderId.HasValue
            ? $"Чудо Обувь — заказ №{orderId.Value}"
            : "Чудо Обувь — оформление заказа";

        LoadProducts();
        LoadOrderData(initialItems);
        RefreshItems();
    }

    private void ApplyTheme()
    {
        UiTheme.ApplyForm(this);
        UiTheme.StyleGrid(_itemsGrid);
    }

    private void ProductComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LoadSizes();
    }

    private void BottomPanel_SizeChanged(object? sender, EventArgs e)
    {
        if (_bottomPanel.ClientSize.Width < 700)
        {
            return;
        }

        _saveButton.Location = new Point(_bottomPanel.ClientSize.Width - 280, 15);
        _cancelButton.Location = new Point(_bottomPanel.ClientSize.Width - 110, 15);
    }

    private void LoadProducts()
    {
        _products = _catalogService.GetProducts(string.Empty, null, CatalogSort.NameAsc).ToList();
        _productComboBox.DataSource = _products;
        if (_products.Count > 0)
        {
            _productComboBox.SelectedIndex = 0;
        }
    }

    private void LoadOrderData(IReadOnlyList<CartItem>? initialItems)
    {
        if (_orderId is int orderId)
        {
            var data = _orderService.GetOrderForEdit(orderId);
            _titleLabel.Text = $"Редактирование заказа №{orderId}";
            _datePicker.Value = data.Order.OrderDate;
            _customerTextBox.Text = data.Order.CustomerName;

            foreach (var item in data.Items)
            {
                AddLoadedItem(item);
            }

            return;
        }

        _datePicker.Value = DateTime.Today;
        _customerTextBox.Text = initialItems is not null ? _session.FullName : string.Empty;

        if (initialItems is not null)
        {
            foreach (var item in initialItems)
            {
                AddLoadedItem(item);
            }
        }
    }

    private void AddLoadedItem(CartItem item)
    {
        try
        {
            _cart.Add(item);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(
                "Позиция не добавлена в заказ.\n\n" + ex.Message,
                "Заказ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void LoadSizes()
    {
        _sizeComboBox.Items.Clear();
        if (_productComboBox.SelectedItem is not ProductView product)
        {
            return;
        }

        var sizes = _catalogService.GetSizes(product.ProductId);
        foreach (var size in sizes)
        {
            _sizeComboBox.Items.Add(size);
        }

        var firstAvailable = sizes.FirstOrDefault(size => size.Quantity > 0);
        if (sizes.Count > 0)
        {
            _sizeComboBox.SelectedItem = firstAvailable ?? sizes[0];
        }
        else
        {
            _quantityNumeric.Maximum = 1;
            _quantityNumeric.Value = 1;
            _addButton.Enabled = false;
        }
    }

    private void SizeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_sizeComboBox.SelectedItem is not ProductSizeView size)
        {
            return;
        }

        _quantityNumeric.Maximum = Math.Max(1, size.Quantity);
        if (_quantityNumeric.Value > _quantityNumeric.Maximum)
        {
            _quantityNumeric.Value = _quantityNumeric.Maximum;
        }

        _addButton.Enabled = size.Quantity > 0;
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        if (_productComboBox.SelectedItem is not ProductView product
            || _sizeComboBox.SelectedItem is not ProductSizeView size)
        {
            MessageBox.Show("Выберите товар и размер.", "Заказ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (size.Quantity <= 0)
        {
            MessageBox.Show("Этого размера нет в наличии.", "Заказ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var item = new CartItem
        {
            ProductSizeId = size.ProductSizeId,
            ProductId = product.ProductId,
            ProductName = product.Name,
            Size = size.Size,
            BasePrice = product.Price,
            DiscountPercent = product.DiscountPercent,
            UnitPrice = product.PriceWithDiscount,
            Quantity = (int)_quantityNumeric.Value,
            AvailableQuantity = size.Quantity
        };

        try
        {
            _cart.Add(item);
            RefreshItems();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Заказ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefreshItems()
    {
        _itemsGrid.Rows.Clear();
        foreach (var item in _cart.Items)
        {
            var rowIndex = _itemsGrid.Rows.Add(
                item.ProductName,
                item.Size,
                ProductView.Money(item.UnitPrice),
                item.Quantity,
                ProductView.Money(item.LineTotal));
            _itemsGrid.Rows[rowIndex].Tag = item;
        }

        _totalLabel.Text = $"Итого: {ProductView.Money(_cart.Total)}";
        _saveButton.Enabled = _cart.Count > 0;
        _removeButton.Enabled = _cart.Count > 0;
        _clearButton.Enabled = _cart.Count > 0;
    }

    private void RemoveButton_Click(object? sender, EventArgs e)
    {
        if (_itemsGrid.CurrentRow?.Tag is not CartItem item)
        {
            return;
        }

        _cart.Remove(item.ProductSizeId);
        RefreshItems();
    }

    private void ClearButton_Click(object? sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            return;
        }

        var result = MessageBox.Show(
            "Удалить все позиции заказа?",
            "Заказ",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (result != DialogResult.Yes)
        {
            return;
        }

        _cart.Clear();
        RefreshItems();
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        var customer = _customerTextBox.Text.Trim();
        if (customer.Length == 0)
        {
            MessageBox.Show("Укажите ФИО клиента.", "Заказ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _customerTextBox.Focus();
            return;
        }

        if (_cart.Count == 0)
        {
            MessageBox.Show("Добавьте хотя бы один товар.", "Заказ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (_orderId is int orderId)
            {
                _orderService.UpdateOrder(orderId, customer, _datePicker.Value, _cart.Items);
                SavedOrderId = orderId;
            }
            else
            {
                SavedOrderId = _orderService.CreateOrder(
                    customer,
                    _datePicker.Value,
                    _cart.Items,
                    _session.UserId);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось сохранить заказ.\n\n" + ex.Message,
                "Заказ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
