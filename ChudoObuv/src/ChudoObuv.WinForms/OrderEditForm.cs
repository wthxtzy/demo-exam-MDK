using ChudoObuv.Core;

namespace ChudoObuv.WinForms;

/// <summary>Создание и редактирование заказа.</summary>
public sealed class OrderEditForm : Form
{
    private readonly Database _database;
    private readonly UserSession _session;
    private readonly int? _orderId;
    private readonly CatalogService _catalogService;
    private readonly OrderService _orderService;
    private readonly Cart _cart = new();

    private Label _titleLabel = null!;
    private DateTimePicker _datePicker = null!;
    private TextBox _customerTextBox = null!;
    private ComboBox _productComboBox = null!;
    private ComboBox _sizeComboBox = null!;
    private NumericUpDown _quantityNumeric = null!;
    private Button _addButton = null!;
    private DataGridView _itemsGrid = null!;
    private Label _totalLabel = null!;
    private Button _saveButton = null!;
    private Button _removeButton = null!;
    private Button _clearButton = null!;

    private List<ProductView> _products = new();

    public int? SavedOrderId { get; private set; }

    public OrderEditForm(
        Database database,
        UserSession session,
        IReadOnlyList<CartItem>? initialItems,
        int? orderId)
    {
        _database = database;
        _session = session;
        _orderId = orderId;
        _catalogService = new CatalogService(database);
        _orderService = new OrderService(database);

        Text = orderId.HasValue
            ? $"Чудо Обувь — заказ №{orderId.Value}"
            : "Чудо Обувь — оформление заказа";
        ClientSize = new Size(1000, 640);
        MinimumSize = new Size(900, 560);
        StartPosition = FormStartPosition.CenterParent;
        UiTheme.ApplyForm(this);

        BuildInterface();
        LoadProducts();
        LoadOrderData(initialItems);
        RefreshItems();
    }

    private void BuildInterface()
    {
        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 104,
            BackColor = UiTheme.Secondary,
            Padding = new Padding(10)
        };

        _titleLabel = new Label
        {
            Text = _orderId.HasValue
                ? $"Редактирование заказа №{_orderId.Value}"
                : "Оформление заказа",
            Font = UiTheme.SubtitleFont,
            AutoSize = true,
            Location = new Point(10, 8)
        };

        var dateLabel = new Label
        {
            Text = "Дата заказа:",
            AutoSize = true,
            Location = new Point(10, 56),
            Font = UiTheme.BoldFont
        };

        _datePicker = new DateTimePicker
        {
            Location = new Point(115, 52),
            Width = 140,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd.MM.yyyy",
            Font = UiTheme.BaseFont
        };

        var customerLabel = new Label
        {
            Text = "ФИО клиента:",
            AutoSize = true,
            Location = new Point(280, 56),
            Font = UiTheme.BoldFont
        };

        _customerTextBox = new TextBox
        {
            Location = new Point(385, 50),
            Width = 560,
            Height = 26,
            Font = UiTheme.BaseFont,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };

        topPanel.Controls.AddRange(new Control[]
        {
            _titleLabel, dateLabel, _datePicker, customerLabel, _customerTextBox
        });

        var addPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 84,
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        var productLabel = new Label
        {
            Text = "Товар:",
            AutoSize = true,
            Location = new Point(10, 4),
            Font = UiTheme.BoldFont
        };

        _productComboBox = new ComboBox
        {
            Location = new Point(10, 26),
            Width = 500,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = UiTheme.BaseFont
        };
        _productComboBox.SelectedIndexChanged += (_, _) => LoadSizes();

        var sizeLabel = new Label
        {
            Text = "Размер:",
            AutoSize = true,
            Location = new Point(525, 4),
            Font = UiTheme.BoldFont
        };

        _sizeComboBox = new ComboBox
        {
            Location = new Point(525, 26),
            Width = 140,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = UiTheme.BaseFont
        };
        _sizeComboBox.SelectedIndexChanged += SizeComboBox_SelectedIndexChanged;

        var quantityLabel = new Label
        {
            Text = "Кол-во:",
            AutoSize = true,
            Location = new Point(680, 4),
            Font = UiTheme.BoldFont
        };

        _quantityNumeric = new NumericUpDown
        {
            Location = new Point(680, 26),
            Width = 90,
            Minimum = 1,
            Maximum = 1,
            Value = 1,
            Font = UiTheme.BaseFont
        };

        _addButton = UiTheme.CreateButton("Добавить", true, 130);
        _addButton.Location = new Point(790, 25);
        _addButton.Click += AddButton_Click;

        addPanel.Controls.AddRange(new Control[]
        {
            productLabel, _productComboBox, sizeLabel, _sizeComboBox,
            quantityLabel, _quantityNumeric, _addButton
        });

        _itemsGrid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        _itemsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Product", HeaderText = "Товар", FillWeight = 300 });
        _itemsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Size", HeaderText = "Размер", FillWeight = 70 });
        _itemsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Price",
            HeaderText = "Цена",
            FillWeight = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        _itemsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Quantity",
            HeaderText = "Кол-во",
            FillWeight = 70,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        _itemsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Sum",
            HeaderText = "Сумма",
            FillWeight = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        UiTheme.StyleGrid(_itemsGrid);

        var bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 66,
            BackColor = UiTheme.Secondary,
            Padding = new Padding(10)
        };

        _totalLabel = new Label
        {
            Text = "Итого: 0,00 ₽",
            Font = new Font("Calibri", 13F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(10, 18)
        };

        _removeButton = UiTheme.CreateButton("Удалить позицию", false, 160);
        _removeButton.Location = new Point(360, 15);
        _removeButton.Click += RemoveButton_Click;

        _clearButton = UiTheme.CreateButton("Очистить", false, 110);
        _clearButton.Location = new Point(530, 15);
        _clearButton.Click += ClearButton_Click;

        _saveButton = UiTheme.CreateButton("Сохранить", true, 160);
        _saveButton.Location = new Point(690, 15);
        _saveButton.Click += SaveButton_Click;

        var cancelButton = UiTheme.CreateButton("Отмена", false, 110);
        cancelButton.Location = new Point(860, 15);
        cancelButton.DialogResult = DialogResult.Cancel;

        bottomPanel.Controls.AddRange(new Control[]
        {
            _totalLabel, _removeButton, _clearButton, _saveButton, cancelButton
        });

        bottomPanel.SizeChanged += (_, _) =>
        {
            if (bottomPanel.ClientSize.Width < 700)
            {
                return;
            }

            _saveButton.Location = new Point(bottomPanel.ClientSize.Width - 280, 15);
            cancelButton.Location = new Point(bottomPanel.ClientSize.Width - 110, 15);
        };

        // Порядок добавления: Fill, Bottom, Top (нижняя панель), Top (верхняя панель).
        Controls.Add(_itemsGrid);
        Controls.Add(bottomPanel);
        Controls.Add(addPanel);
        Controls.Add(topPanel);

        AcceptButton = _saveButton;
        CancelButton = cancelButton;
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
