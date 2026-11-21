using ChudoObuv.Core;

namespace ChudoObuv.WinForms;

/// <summary>Главное окно: каталог, корзина и заказы.</summary>
public sealed class MainForm : Form
{
    private readonly Database _database;
    private readonly UserSession _session;
    private readonly CatalogService _catalogService;
    private readonly OrderService _orderService;
    private readonly Cart _cart = new();

    private TabControl _tabs = null!;
    private TabPage _catalogTab = null!;
    private TabPage _cartTab = null!;
    private TabPage _ordersTab = null!;

    private Panel _filterPanel = null!;
    private TextBox _searchTextBox = null!;
    private ComboBox _categoryComboBox = null!;
    private ComboBox _sortComboBox = null!;
    private SplitContainer _catalogSplit = null!;
    private SplitContainer _ordersSplit = null!;

    private DataGridView _productsGrid = null!;
    private DataGridView _cartGrid = null!;
    private DataGridView _ordersGrid = null!;
    private DataGridView _orderItemsGrid = null!;

    private PictureBox _productPicture = null!;
    private Label _productNameLabel = null!;
    private Label _productInfoLabel = null!;
    private Label _priceLabel = null!;
    private Label _stockLabel = null!;
    private Label _descriptionLabel = null!;
    private Label _compositionLabel = null!;
    private Label _guestHintLabel = null!;
    private ComboBox _sizeComboBox = null!;
    private NumericUpDown _quantityNumeric = null!;
    private Button _addToCartButton = null!;

    private Label _cartTotalLabel = null!;
    private Button _checkoutButton = null!;
    private Button _removeCartButton = null!;
    private Button _clearCartButton = null!;

    private Label _orderHeaderLabel = null!;
    private Label _orderTotalLabel = null!;
    private Button _newOrderButton = null!;
    private Button _editOrderButton = null!;
    private Button _deleteOrderButton = null!;

    private StatusStrip _statusStrip = null!;
    private ToolStripStatusLabel _statusLabel = null!;

    private List<ProductView> _products = new();
    private ProductView? _selectedProduct;
    private bool _loadingCatalog;
    private bool _permissionsApplied;

    public MainForm(Database database, UserSession session)
    {
        _database = database;
        _session = session;
        _catalogService = new CatalogService(database);
        _orderService = new OrderService(database);

        Text = "Чудо Обувь — оформление заказов";
        ClientSize = new Size(1280, 800);
        MinimumSize = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        UiTheme.ApplyForm(this);

        BuildInterface();
        ApplyPermissions();
        LoadFilterData();
        LoadCatalog();
        RefreshCart();
        LoadOrders();

        Shown += MainForm_Shown;
        FormClosing += MainForm_FormClosing;
    }

    private void BuildInterface()
    {
        var header = BuildHeader();
        _tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = UiTheme.BaseFont,
            Padding = new Point(18, 6)
        };

        _catalogTab = BuildCatalogTab();
        _cartTab = BuildCartTab();
        _ordersTab = BuildOrdersTab();

        _tabs.TabPages.Add(_catalogTab);
        _tabs.TabPages.Add(_cartTab);
        _tabs.TabPages.Add(_ordersTab);

        _statusStrip = new StatusStrip { SizingGrip = false };
        _statusLabel = new ToolStripStatusLabel("Готово") { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
        var databaseLabel = new ToolStripStatusLabel($"БД: {Path.GetFileName(_database.DatabasePath)}")
        {
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.DimGray
        };
        _statusStrip.Items.Add(_statusLabel);
        _statusStrip.Items.Add(databaseLabel);

        // Порядок добавления задаёт порядок докинга: Fill, Bottom, Top.
        Controls.Add(_tabs);
        Controls.Add(_statusStrip);
        Controls.Add(header);
    }

    private Panel BuildHeader()
    {
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 64,
            BackColor = Color.White,
            Padding = new Padding(8)
        };

        var logo = new PictureBox
        {
            Location = new Point(10, 8),
            Size = new Size(48, 48),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = ImageResources.Logo,
            BackColor = Color.White
        };

        var title = new Label
        {
            Text = "Чудо Обувь — оформление заказов",
            Font = UiTheme.SubtitleFont,
            AutoSize = true,
            Location = new Point(70, 19)
        };

        var userLabel = new Label
        {
            Text = $"{_session.FullName} ({_session.RoleDisplayName})",
            TextAlign = ContentAlignment.MiddleRight,
            AutoSize = false,
            Size = new Size(430, 40),
            ForeColor = Color.DimGray,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        var logoutButton = UiTheme.CreateButton("Выход", false, 90);
        logoutButton.Height = 32;
        logoutButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        logoutButton.Click += (_, _) => Close();

        header.Controls.AddRange(new Control[] { logo, title, userLabel, logoutButton });

        userLabel.Location = new Point(Math.Max(360, ClientSize.Width - 540), 12);
        logoutButton.Location = new Point(Math.Max(820, ClientSize.Width - 102), 16);
        header.Resize += (_, _) =>
        {
            userLabel.Location = new Point(Math.Max(360, header.ClientSize.Width - 540), 12);
            logoutButton.Location = new Point(Math.Max(820, header.ClientSize.Width - 102), 16);
        };

        return header;
    }

    private TabPage BuildCatalogTab()
    {
        var page = new TabPage("Каталог")
        {
            BackColor = Color.White,
            Padding = new Padding(6),
            UseVisualStyleBackColor = false
        };

        _filterPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 50,
            BackColor = UiTheme.Secondary,
            Padding = new Padding(4)
        };

        var filterFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoScroll = true
        };

        filterFlow.Controls.Add(CreateFilterLabel("Поиск:"));
        _searchTextBox = new TextBox
        {
            Width = 230,
            Margin = new Padding(0, 10, 14, 0),
            Font = UiTheme.BaseFont
        };
        _searchTextBox.TextChanged += (_, _) => LoadCatalog();
        filterFlow.Controls.Add(_searchTextBox);

        filterFlow.Controls.Add(CreateFilterLabel("Категория:"));
        _categoryComboBox = new ComboBox
        {
            Width = 190,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Margin = new Padding(0, 10, 14, 0),
            Font = UiTheme.BaseFont
        };
        _categoryComboBox.SelectedIndexChanged += (_, _) => LoadCatalog();
        filterFlow.Controls.Add(_categoryComboBox);

        filterFlow.Controls.Add(CreateFilterLabel("Сортировка:"));
        _sortComboBox = new ComboBox
        {
            Width = 230,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Margin = new Padding(0, 10, 14, 0),
            Font = UiTheme.BaseFont
        };
        _sortComboBox.SelectedIndexChanged += (_, _) => LoadCatalog();
        filterFlow.Controls.Add(_sortComboBox);

        var resetButton = UiTheme.CreateButton("Сбросить", false, 100);
        resetButton.Height = 30;
        resetButton.Margin = new Padding(0, 8, 0, 0);
        resetButton.Click += (_, _) => ResetFilters();
        filterFlow.Controls.Add(resetButton);

        _filterPanel.Controls.Add(filterFlow);

        _catalogSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterWidth = 6,
            FixedPanel = FixedPanel.Panel2,
            BackColor = UiTheme.Secondary
        };
        _catalogSplit.Panel1.BackColor = Color.White;
        _catalogSplit.Panel2.BackColor = UiTheme.Secondary;

        _productsGrid = CreateProductsGrid();
        _catalogSplit.Panel1.Controls.Add(_productsGrid);

        var detailsFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(10),
            BackColor = UiTheme.Secondary
        };

        _productPicture = new PictureBox
        {
            Width = 300,
            Height = 210,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 0, 8)
        };
        _productNameLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(300, 0),
            Font = UiTheme.SubtitleFont,
            Margin = new Padding(0, 0, 0, 4)
        };
        _productInfoLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(300, 0),
            ForeColor = Color.DimGray,
            Margin = new Padding(0, 0, 0, 8)
        };
        _priceLabel = new Label
        {
            AutoSize = true,
            Font = new Font("Calibri", 12F, FontStyle.Bold),
            ForeColor = UiTheme.AccentDark,
            Margin = new Padding(0, 0, 0, 4)
        };
        _stockLabel = new Label
        {
            AutoSize = true,
            Font = UiTheme.BoldFont,
            Margin = new Padding(0, 0, 0, 8)
        };
        _descriptionLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(300, 0),
            Margin = new Padding(0, 0, 0, 6)
        };
        _compositionLabel = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(300, 0),
            ForeColor = Color.DimGray,
            Margin = new Padding(0, 0, 0, 8)
        };

        var delimiter = new Panel
        {
            Width = 300,
            Height = 1,
            BackColor = Color.Silver,
            Margin = new Padding(0, 2, 0, 8)
        };

        _sizeComboBox = new ComboBox
        {
            Width = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = UiTheme.BaseFont,
            Margin = new Padding(0, 2, 0, 6)
        };
        _sizeComboBox.SelectedIndexChanged += SizeComboBox_SelectedIndexChanged;

        _quantityNumeric = new NumericUpDown
        {
            Width = 100,
            Minimum = 1,
            Maximum = 1,
            Value = 1,
            Font = UiTheme.BaseFont,
            Margin = new Padding(0, 2, 0, 8)
        };

        _addToCartButton = UiTheme.CreateButton("Добавить в корзину", true, 300);
        _addToCartButton.Click += AddToCartButton_Click;

        _guestHintLabel = new Label
        {
            Text = "Для оформления заказа войдите как пользователь.",
            AutoSize = true,
            MaximumSize = new Size(300, 0),
            ForeColor = Color.Firebrick,
            Font = UiTheme.BoldFont,
            Visible = false,
            Margin = new Padding(0, 6, 0, 0)
        };

        var detailsBottom = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 150,
            BackColor = UiTheme.Secondary,
            Padding = new Padding(10)
        };

        var sizeCaption = UiTheme.CreateLabel("Размер:");
        sizeCaption.Location = new Point(10, 4);
        sizeCaption.Margin = Padding.Empty;
        _sizeComboBox.Location = new Point(10, 24);

        var quantityCaption = UiTheme.CreateLabel("Количество:");
        quantityCaption.Location = new Point(10, 58);
        quantityCaption.Margin = Padding.Empty;
        _quantityNumeric.Location = new Point(10, 78);

        _addToCartButton.Location = new Point(10, 108);

        detailsBottom.Controls.AddRange(new Control[]
        {
            sizeCaption, _sizeComboBox, quantityCaption, _quantityNumeric, _addToCartButton
        });

        detailsFlow.Controls.AddRange(new Control[]
        {
            _productPicture, _productNameLabel, _productInfoLabel, _priceLabel, _stockLabel,
            _descriptionLabel, _compositionLabel, delimiter, _guestHintLabel
        });

        _catalogSplit.Panel2.Controls.Add(detailsFlow);
        _catalogSplit.Panel2.Controls.Add(detailsBottom);

        // Сначала Fill, затем Top: панель фильтров останется сверху.
        page.Controls.Add(_catalogSplit);
        page.Controls.Add(_filterPanel);

        return page;
    }

    private DataGridView CreateProductsGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            BackgroundColor = Color.White
        };

        grid.Columns.Add(new DataGridViewImageColumn
        {
            Name = "Image",
            HeaderText = "",
            FillWeight = 55,
            MinimumWidth = 50,
            ImageLayout = DataGridViewImageCellLayout.Zoom,
            ValuesAreIcons = false
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Категория", FillWeight = 80, MinimumWidth = 80 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Subcategory", HeaderText = "Подкатегория", FillWeight = 80, MinimumWidth = 80 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "ProductName", HeaderText = "Наименование", FillWeight = 190, MinimumWidth = 160 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Manufacturer", HeaderText = "Производство", FillWeight = 110, MinimumWidth = 100 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Composition", HeaderText = "Состав", FillWeight = 250, MinimumWidth = 180 });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Price",
            HeaderText = "Цена со скидкой",
            FillWeight = 95,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Discount",
            HeaderText = "Скидка",
            FillWeight = 60,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Quantity",
            HeaderText = "Кол-во",
            FillWeight = 60,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Stock",
            HeaderText = "Наличие",
            FillWeight = 65,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        grid.RowTemplate.Height = 64;
        grid.SelectionChanged += ProductsGrid_SelectionChanged;
        grid.CellFormatting += ProductsGrid_CellFormatting;
        UiTheme.StyleGrid(grid);
        return grid;
    }

    private TabPage BuildCartTab()
    {
        var page = new TabPage("Корзина")
        {
            BackColor = Color.White,
            Padding = new Padding(6),
            UseVisualStyleBackColor = false
        };

        var hint = new Label
        {
            Text = "Проверьте состав заказа и оформите его.",
            Dock = DockStyle.Top,
            Height = 32,
            Font = UiTheme.BoldFont,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.DimGray
        };

        _cartGrid = new DataGridView
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
        _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Product", HeaderText = "Товар", FillWeight = 320 });
        _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Size", HeaderText = "Размер", FillWeight = 70 });
        _cartGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Price",
            HeaderText = "Цена",
            FillWeight = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        _cartGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Quantity",
            HeaderText = "Кол-во",
            FillWeight = 70,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        _cartGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Sum",
            HeaderText = "Сумма",
            FillWeight = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        UiTheme.StyleGrid(_cartGrid);

        var bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 64,
            BackColor = UiTheme.Secondary,
            Padding = new Padding(8)
        };

        _cartTotalLabel = new Label
        {
            Text = "Итого: 0,00 ₽",
            Font = new Font("Calibri", 13F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(10, 18)
        };

        _removeCartButton = UiTheme.CreateButton("Удалить позицию", false, 150);
        _removeCartButton.Location = new Point(360, 14);
        _removeCartButton.Click += RemoveCartButton_Click;

        _clearCartButton = UiTheme.CreateButton("Очистить", false, 110);
        _clearCartButton.Location = new Point(520, 14);
        _clearCartButton.Click += ClearCartButton_Click;

        _checkoutButton = UiTheme.CreateButton("Оформить заказ", true, 180);
        _checkoutButton.Location = new Point(640, 14);
        _checkoutButton.Click += CheckoutButton_Click;

        bottomPanel.Controls.AddRange(new Control[]
        {
            _cartTotalLabel, _removeCartButton, _clearCartButton, _checkoutButton
        });

        page.Controls.Add(_cartGrid);
        page.Controls.Add(bottomPanel);
        page.Controls.Add(hint);

        page.Enter += (_, _) => RefreshCart();
        return page;
    }

    private TabPage BuildOrdersTab()
    {
        var page = new TabPage("Заказы")
        {
            BackColor = Color.White,
            Padding = new Padding(6),
            UseVisualStyleBackColor = false
        };

        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 50,
            BackColor = UiTheme.Secondary,
            Padding = new Padding(6)
        };

        var topFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        var title = new Label
        {
            Text = "Список заказов",
            Font = UiTheme.SubtitleFont,
            AutoSize = true,
            Margin = new Padding(4, 8, 20, 0)
        };
        topFlow.Controls.Add(title);

        _newOrderButton = UiTheme.CreateButton("Новый заказ", true, 140);
        _newOrderButton.Height = 30;
        _newOrderButton.Margin = new Padding(0, 6, 6, 0);
        _newOrderButton.Click += NewOrderButton_Click;
        topFlow.Controls.Add(_newOrderButton);

        _editOrderButton = UiTheme.CreateButton("Редактировать", false, 150);
        _editOrderButton.Height = 30;
        _editOrderButton.Margin = new Padding(0, 6, 6, 0);
        _editOrderButton.Click += EditOrderButton_Click;
        topFlow.Controls.Add(_editOrderButton);

        _deleteOrderButton = UiTheme.CreateButton("Удалить", false, 110);
        _deleteOrderButton.Height = 30;
        _deleteOrderButton.Margin = new Padding(0, 6, 6, 0);
        _deleteOrderButton.Click += DeleteOrderButton_Click;
        topFlow.Controls.Add(_deleteOrderButton);

        var refreshButton = UiTheme.CreateButton("Обновить", false, 110);
        refreshButton.Height = 30;
        refreshButton.Margin = new Padding(0, 6, 0, 0);
        refreshButton.Click += (_, _) => LoadOrders();
        topFlow.Controls.Add(refreshButton);

        topPanel.Controls.Add(topFlow);

        _ordersSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterWidth = 6,
            FixedPanel = FixedPanel.Panel2,
            BackColor = UiTheme.Secondary
        };
        _ordersSplit.Panel1.BackColor = Color.White;
        _ordersSplit.Panel2.BackColor = Color.White;

        _ordersGrid = new DataGridView
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
        _ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "№", FillWeight = 45 });
        _ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Дата", FillWeight = 85 });
        _ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Customer", HeaderText = "Клиент", FillWeight = 190 });
        _ordersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Creator", HeaderText = "Оформил", FillWeight = 150 });
        _ordersGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Positions",
            HeaderText = "Позиций",
            FillWeight = 65,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        _ordersGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Total",
            HeaderText = "Сумма",
            FillWeight = 120,
            MinimumWidth = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        _ordersGrid.SelectionChanged += OrdersGrid_SelectionChanged;
        UiTheme.StyleGrid(_ordersGrid);

        var detailsPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(8)
        };

        var orderHeaderPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = UiTheme.Secondary,
            Padding = new Padding(8)
        };

        _orderHeaderLabel = new Label
        {
            Dock = DockStyle.Fill,
            Font = UiTheme.BoldFont,
            TextAlign = ContentAlignment.TopLeft
        };

        _orderTotalLabel = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 26,
            Font = new Font("Calibri", 12F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = UiTheme.AccentDark
        };

        orderHeaderPanel.Controls.Add(_orderHeaderLabel);
        orderHeaderPanel.Controls.Add(_orderTotalLabel);

        _orderItemsGrid = new DataGridView
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
        _orderItemsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Product", HeaderText = "Товар", FillWeight = 260 });
        _orderItemsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Size", HeaderText = "Размер", FillWeight = 60 });
        _orderItemsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Quantity",
            HeaderText = "Кол-во",
            FillWeight = 60,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        _orderItemsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Price",
            HeaderText = "Цена",
            FillWeight = 90,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        _orderItemsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Sum",
            HeaderText = "Сумма",
            FillWeight = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        UiTheme.StyleGrid(_orderItemsGrid);

        detailsPanel.Controls.Add(_orderItemsGrid);
        detailsPanel.Controls.Add(orderHeaderPanel);

        _ordersSplit.Panel1.Controls.Add(_ordersGrid);
        _ordersSplit.Panel2.Controls.Add(detailsPanel);

        page.Controls.Add(_ordersSplit);
        page.Controls.Add(topPanel);
        page.Enter += (_, _) => LoadOrders();

        return page;
    }

    private static Label CreateFilterLabel(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Margin = new Padding(4, 14, 4, 0),
        Font = UiTheme.BaseFont
    };

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
