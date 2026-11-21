using ChudoObuv.Core;

namespace ChudoObuv.WinForms;

/// <summary>Главное окно: каталог, корзина и заказы.</summary>
// Разметка формы находится в MainForm.Designer.cs — это стандартное разделение WinForms.
public partial class MainForm : Form
{
    private readonly Database _database = null!;
    private readonly UserSession _session = null!;
    private readonly CatalogService _catalogService = null!;
    private readonly OrderService _orderService = null!;
    private readonly Cart _cart = new();

    private List<ProductView> _products = new();
    private ProductView? _selectedProduct;
    private bool _loadingCatalog;
    private bool _permissionsApplied;

    public MainForm()
    {
        InitializeComponent();
    }

    public MainForm(Database database, UserSession session) : this()
    {
        _database = database;
        _session = session;
        _catalogService = new CatalogService(database);
        _orderService = new OrderService(database);
        ApplyTheme();

        _userLabel.Text = $"{_session.FullName} ({_session.RoleDisplayName})";
        _databaseStatusLabel.Text = $"БД: {Path.GetFileName(_database.DatabasePath)}";

        ApplyPermissions();
        LoadFilterData();
        LoadCatalog();
        RefreshCart();
        LoadOrders();
    }

    private void ApplyTheme()
    {
        UiTheme.ApplyForm(this);
        _logoPictureBox.Image = ImageResources.Logo;
        UiTheme.StyleGrid(_productsGrid);
        UiTheme.StyleGrid(_cartGrid);
        UiTheme.StyleGrid(_ordersGrid);
        UiTheme.StyleGrid(_orderItemsGrid);
    }

    private void SearchTextBox_TextChanged(object? sender, EventArgs e)
    {
        LoadCatalog();
    }

    private void FilterComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LoadCatalog();
    }

    private void ResetFiltersButton_Click(object? sender, EventArgs e)
    {
        ResetFilters();
    }

    private void LogoutButton_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void RefreshOrdersButton_Click(object? sender, EventArgs e)
    {
        LoadOrders();
    }

    private void CartTab_Enter(object? sender, EventArgs e)
    {
        RefreshCart();
    }

    private void OrdersTab_Enter(object? sender, EventArgs e)
    {
        LoadOrders();
    }

    private void LoadFilterData()
    {
        _loadingCatalog = true;
        try
        {
            var categories = new List<IdName> { new(0, "Все категории") };
            categories.AddRange(_catalogService.GetCategories());
            _categoryComboBox.DataSource = categories;
            _categoryComboBox.DisplayMember = nameof(IdName.Name);
            _categoryComboBox.ValueMember = nameof(IdName.Id);

            _sortComboBox.DataSource = new List<CatalogSortOption>
            {
                new("По названию (А–Я)", CatalogSort.NameAsc),
                new("По названию (Я–А)", CatalogSort.NameDesc),
                new("По цене (возрастание)", CatalogSort.PriceAsc),
                new("По цене (убывание)", CatalogSort.PriceDesc),
                new("По количеству (возрастание)", CatalogSort.QuantityAsc),
                new("По количеству (убывание)", CatalogSort.QuantityDesc)
            };
            _sortComboBox.DisplayMember = nameof(CatalogSortOption.Name);
            _categoryComboBox.SelectedIndex = 0;
            _sortComboBox.SelectedIndex = 0;
        }
        finally
        {
            _loadingCatalog = false;
        }
    }

    private void ApplyPermissions()
    {
        if (_permissionsApplied)
        {
            return;
        }

        _permissionsApplied = true;

        if (_session.IsGuest)
        {
            _tabs.TabPages.Remove(_cartTab);
            _tabs.TabPages.Remove(_ordersTab);
            _filterPanel.Visible = false;
            _guestHintLabel.Visible = true;
            _addToCartButton.Enabled = false;
        }
        else
        {
            _guestHintLabel.Visible = false;
            _addToCartButton.Enabled = true;
        }

        _newOrderButton.Visible = Permissions.CanCreateOrder(_session.Role);
        _editOrderButton.Visible = Permissions.CanEditOrder(_session.Role);
        _deleteOrderButton.Visible = Permissions.CanDeleteOrder(_session.Role);
        _checkoutButton.Visible = Permissions.CanOrder(_session.Role);
        _statusLabel.Text = $"Вы вошли как: {_session.FullName} ({_session.RoleDisplayName})";
    }

    private void MainForm_Shown(object? sender, EventArgs e)
    {
        if (_catalogSplit.Width > 800)
        {
            _catalogSplit.SplitterDistance = Math.Max(420, _catalogSplit.Width - 340);
            _catalogSplit.Panel1MinSize = 300;
            _catalogSplit.Panel2MinSize = 330;
        }

        if (_ordersSplit.Width > 800)
        {
            _ordersSplit.SplitterDistance = Math.Max(420, _ordersSplit.Width - 460);
            _ordersSplit.Panel1MinSize = 300;
            _ordersSplit.Panel2MinSize = 440;
        }
    }

    private void LoadCatalog()
    {
        if (_loadingCatalog)
        {
            return;
        }

        _loadingCatalog = true;
        try
        {
            var previousProductId = _selectedProduct?.ProductId;
            var categoryId = _categoryComboBox.SelectedItem is IdName category && category.Id > 0
                ? category.Id
                : (int?)null;
            var sort = _sortComboBox.SelectedItem is CatalogSortOption sortOption
                ? sortOption.Sort
                : CatalogSort.NameAsc;

            _products = _catalogService.GetProducts(_searchTextBox.Text, categoryId, sort).ToList();

            _productsGrid.Rows.Clear();
            foreach (var product in _products)
            {
                var image = ImageResources.LoadProductImage(product.ImageName)
                    ?? SystemIcons.Application.ToBitmap();
                var rowIndex = _productsGrid.Rows.Add(
                    image,
                    product.Category,
                    product.Subcategory,
                    product.Name,
                    product.Manufacturer,
                    product.Composition,
                    product.PriceLabel,
                    product.DiscountLabel,
                    product.TotalQuantity,
                    product.StockLabel);

                _productsGrid.Rows[rowIndex].Tag = product;
            }

            _statusLabel.Text = $"Товаров в каталоге: {_products.Count}";

            if (_products.Count == 0)
            {
                _selectedProduct = null;
                ClearProductDetails();
                return;
            }

            var rowToSelect = 0;
            if (previousProductId.HasValue)
            {
                var index = _products.FindIndex(p => p.ProductId == previousProductId.Value);
                if (index >= 0)
                {
                    rowToSelect = index;
                }
            }

            _productsGrid.ClearSelection();
            _productsGrid.Rows[rowToSelect].Selected = true;
            _productsGrid.CurrentCell = _productsGrid.Rows[rowToSelect].Cells[0];
            _selectedProduct = _products[rowToSelect];
            ShowProductDetails(_selectedProduct);
            _productsGrid.Invalidate();
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "Ошибка загрузки каталога";
            MessageBox.Show(
                "Не удалось загрузить каталог.\n\n" + ex.Message,
                "Каталог",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            _loadingCatalog = false;
        }
    }

    private void ProductsGrid_SelectionChanged(object? sender, EventArgs e)
    {
        if (_productsGrid.CurrentRow?.Tag is not ProductView product)
        {
            return;
        }

        _selectedProduct = product;
        ShowProductDetails(product);
    }

    private void ProductsGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= _productsGrid.Rows.Count)
        {
            return;
        }

        if (e.CellStyle is null)
        {
            return;
        }

        if (_productsGrid.Rows[e.RowIndex].Tag is ProductView product && product.IsLowStock)
        {
            e.CellStyle.BackColor = UiTheme.LowStock;
            e.CellStyle.SelectionBackColor = ControlPaint.Dark(UiTheme.LowStock);
        }
    }

    private void ShowProductDetails(ProductView product)
    {
        _productPicture.Image = ImageResources.LoadProductImage(product.ImageName)
            ?? SystemIcons.Application.ToBitmap();
        _productNameLabel.Text = product.Name;
        _productInfoLabel.Text =
            $"{product.Category} / {product.Subcategory}{Environment.NewLine}" +
            $"Производитель: {product.Manufacturer}";
        _priceLabel.Text = product.DiscountPercent > 0
            ? $"{product.PriceLabel}  (скидка {product.DiscountPercent}%)"
            : $"{product.PriceLabel}  (без скидки)";
        _stockLabel.Text = $"Количество: {product.TotalQuantity} ({product.StockLabel})";
        _descriptionLabel.Text = "Описание: " +
            (string.IsNullOrWhiteSpace(product.Description) ? "—" : product.Description);
        _compositionLabel.Text = "Состав: " +
            (string.IsNullOrWhiteSpace(product.Composition) ? "—" : product.Composition);

        _sizeComboBox.Items.Clear();
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
            _addToCartButton.Enabled = false;
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

        _addToCartButton.Enabled = Permissions.CanOrder(_session.Role) && size.Quantity > 0;
    }

    private void ClearProductDetails()
    {
        _productPicture.Image = null;
        _productNameLabel.Text = string.Empty;
        _productInfoLabel.Text = string.Empty;
        _priceLabel.Text = string.Empty;
        _stockLabel.Text = string.Empty;
        _descriptionLabel.Text = string.Empty;
        _compositionLabel.Text = string.Empty;
        _sizeComboBox.Items.Clear();
        _quantityNumeric.Maximum = 1;
        _quantityNumeric.Value = 1;
        _addToCartButton.Enabled = false;
    }

    private void AddToCartButton_Click(object? sender, EventArgs e)
    {
        if (_selectedProduct is null || _sizeComboBox.SelectedItem is not ProductSizeView size)
        {
            MessageBox.Show("Выберите товар и размер.", "Корзина", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (size.Quantity <= 0)
        {
            MessageBox.Show("Этого размера нет в наличии.", "Корзина", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var quantity = (int)_quantityNumeric.Value;
        var item = new CartItem
        {
            ProductSizeId = size.ProductSizeId,
            ProductId = _selectedProduct.ProductId,
            ProductName = _selectedProduct.Name,
            Size = size.Size,
            BasePrice = _selectedProduct.Price,
            DiscountPercent = _selectedProduct.DiscountPercent,
            UnitPrice = _selectedProduct.PriceWithDiscount,
            Quantity = quantity,
            AvailableQuantity = size.Quantity
        };

        try
        {
            _cart.Add(item);
            RefreshCart();
            _statusLabel.Text = $"Добавлено: {_selectedProduct.Name}, размер {size.Size}, {quantity} шт.";
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Корзина", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefreshCart()
    {
        _cartGrid.Rows.Clear();
        foreach (var item in _cart.Items)
        {
            var rowIndex = _cartGrid.Rows.Add(
                item.ProductName,
                item.Size,
                ProductView.Money(item.UnitPrice),
                item.Quantity,
                ProductView.Money(item.LineTotal));
            _cartGrid.Rows[rowIndex].Tag = item;
        }

        _cartTotalLabel.Text = $"Итого: {ProductView.Money(_cart.Total)}";
        _cartTab.Text = _cart.Count > 0 ? $"Корзина ({_cart.Count})" : "Корзина";
        _checkoutButton.Enabled = _cart.Count > 0 && Permissions.CanOrder(_session.Role);
        _removeCartButton.Enabled = _cart.Count > 0;
        _clearCartButton.Enabled = _cart.Count > 0;
    }

    private void RemoveCartButton_Click(object? sender, EventArgs e)
    {
        if (_cartGrid.CurrentRow?.Tag is not CartItem item)
        {
            return;
        }

        _cart.Remove(item.ProductSizeId);
        RefreshCart();
    }

    private void ClearCartButton_Click(object? sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            return;
        }

        var result = MessageBox.Show(
            "Очистить корзину?",
            "Корзина",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (result != DialogResult.Yes)
        {
            return;
        }

        _cart.Clear();
        RefreshCart();
    }

    private void CheckoutButton_Click(object? sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("Корзина пуста.", "Оформление заказа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var orderForm = CreateOrderForm(_cart.Items.ToList(), orderId: null);
        if (orderForm is null || orderForm.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _cart.Clear();
        RefreshCart();
        LoadCatalog();
        LoadOrders();

        if (orderForm.SavedOrderId is int orderId)
        {
            _tabs.SelectedTab = _ordersTab;
            SelectOrder(orderId);
            _statusLabel.Text = $"Заказ №{orderId} успешно оформлен.";
        }
    }

    private void LoadOrders()
    {
        if (_session.IsGuest)
        {
            return;
        }

        try
        {
            var previousOrderId = (_ordersGrid.CurrentRow?.Tag as OrderView)?.OrderId;
            int? userIdFilter = Permissions.CanViewAllOrders(_session.Role)
                ? null
                : (_session.UserId ?? -1);

            var orders = _orderService.GetOrders(userIdFilter);
            _ordersGrid.Rows.Clear();
            foreach (var order in orders)
            {
                var rowIndex = _ordersGrid.Rows.Add(
                    order.OrderId,
                    order.DateLabel,
                    order.CustomerName,
                    order.CreatorName,
                    order.Positions,
                    order.TotalLabel);
                _ordersGrid.Rows[rowIndex].Tag = order;
            }

            if (orders.Count == 0)
            {
                ClearOrderDetails();
                return;
            }

            var rowToSelect = 0;
            if (previousOrderId.HasValue)
            {
                var index = orders.ToList().FindIndex(order => order.OrderId == previousOrderId.Value);
                if (index >= 0)
                {
                    rowToSelect = index;
                }
            }

            _ordersGrid.ClearSelection();
            _ordersGrid.Rows[rowToSelect].Selected = true;
            _ordersGrid.CurrentCell = _ordersGrid.Rows[rowToSelect].Cells[0];
            ShowSelectedOrderDetails();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось загрузить заказы.\n\n" + ex.Message,
                "Заказы",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void OrdersGrid_SelectionChanged(object? sender, EventArgs e)
    {
        ShowSelectedOrderDetails();
    }

    private void ShowSelectedOrderDetails()
    {
        if (_ordersGrid.CurrentRow?.Tag is not OrderView order)
        {
            return;
        }

        try
        {
            var details = _orderService.GetOrderDetails(order.OrderId);
            _orderHeaderLabel.Text =
                $"Заказ №{details.Order.OrderId} от {details.Order.DateLabel}{Environment.NewLine}" +
                $"Клиент: {details.Order.CustomerName}{Environment.NewLine}" +
                $"Оформил: {details.Order.CreatorName}";
            _orderTotalLabel.Text = $"Итого по заказу: {ProductView.Money(details.Order.Total)}";

            _orderItemsGrid.Rows.Clear();
            foreach (var item in details.Items)
            {
                _orderItemsGrid.Rows.Add(
                    item.ProductName,
                    item.Size,
                    item.Quantity,
                    item.UnitPriceLabel,
                    item.LineTotalLabel);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось загрузить состав заказа.\n\n" + ex.Message,
                "Заказы",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ClearOrderDetails()
    {
        _orderHeaderLabel.Text = "Выберите заказ";
        _orderTotalLabel.Text = string.Empty;
        _orderItemsGrid.Rows.Clear();
    }

    private void NewOrderButton_Click(object? sender, EventArgs e)
    {
        using var orderForm = CreateOrderForm(initialItems: null, orderId: null);
        if (orderForm is null || orderForm.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        LoadOrders();
        LoadCatalog();
        if (orderForm.SavedOrderId is int orderId)
        {
            SelectOrder(orderId);
            _statusLabel.Text = $"Создан заказ №{orderId}.";
        }
    }

    private void EditOrderButton_Click(object? sender, EventArgs e)
    {
        if (_ordersGrid.CurrentRow?.Tag is not OrderView order)
        {
            MessageBox.Show("Выберите заказ.", "Заказы", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var orderForm = CreateOrderForm(initialItems: null, orderId: order.OrderId);
        if (orderForm is null || orderForm.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        LoadOrders();
        LoadCatalog();
        _statusLabel.Text = $"Заказ №{order.OrderId} изменён.";
    }

    private void DeleteOrderButton_Click(object? sender, EventArgs e)
    {
        if (_ordersGrid.CurrentRow?.Tag is not OrderView order)
        {
            MessageBox.Show("Выберите заказ.", "Заказы", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Удалить заказ №{order.OrderId} от {order.DateLabel}?\n" +
            "Товары будут возвращены в наличие.",
            "Удаление заказа",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        if (result != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _orderService.DeleteOrder(order.OrderId);
            LoadOrders();
            LoadCatalog();
            _statusLabel.Text = $"Заказ №{order.OrderId} удалён.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось удалить заказ.\n\n" + ex.Message,
                "Заказы",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private OrderEditForm? CreateOrderForm(IReadOnlyList<CartItem>? initialItems, int? orderId)
    {
        try
        {
            return new OrderEditForm(_database, _session, initialItems, orderId);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось открыть форму заказа.\n\n" + ex.Message,
                "Заказ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }
    }

    private void SelectOrder(int orderId)
    {
        foreach (DataGridViewRow row in _ordersGrid.Rows)
        {
            if (row.Tag is OrderView order && order.OrderId == orderId)
            {
                row.Selected = true;
                _ordersGrid.CurrentCell = row.Cells[0];
                ShowSelectedOrderDetails();
                break;
            }
        }
    }

    private void ResetFilters()
    {
        _loadingCatalog = true;
        try
        {
            _searchTextBox.Text = string.Empty;
            if (_categoryComboBox.Items.Count > 0)
            {
                _categoryComboBox.SelectedIndex = 0;
            }

            if (_sortComboBox.Items.Count > 0)
            {
                _sortComboBox.SelectedIndex = 0;
            }
        }
        finally
        {
            _loadingCatalog = false;
        }

        LoadCatalog();
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.UserClosing || _cart.Count == 0)
        {
            return;
        }

        var result = MessageBox.Show(
            "В корзине есть товары. Закрыть приложение?",
            "Чудо Обувь",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (result != DialogResult.Yes)
        {
            e.Cancel = true;
        }
    }
}
