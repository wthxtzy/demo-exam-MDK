#nullable enable

using System.Drawing;
using System.Windows.Forms;

namespace ChudoObuv.WinForms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components;
    private Panel _headerPanel = null!;
    private PictureBox _logoPictureBox = null!;
    private Label _headerTitleLabel = null!;
    private Label _userLabel = null!;
    private Button _logoutButton = null!;
    private TabControl _tabs = null!;
    private TabPage _catalogTab = null!;
    private TabPage _cartTab = null!;
    private TabPage _ordersTab = null!;
    private Panel _filterPanel = null!;
    private FlowLayoutPanel _filterFlow = null!;
    private Label _searchLabel = null!;
    private Label _categoryLabel = null!;
    private Label _sortLabel = null!;
    private TextBox _searchTextBox = null!;
    private ComboBox _categoryComboBox = null!;
    private ComboBox _sortComboBox = null!;
    private Button _resetFiltersButton = null!;
    private SplitContainer _catalogSplit = null!;
    private DataGridView _productsGrid = null!;
    private DataGridViewImageColumn _productImageColumn = null!;
    private DataGridViewTextBoxColumn _productCategoryColumn = null!;
    private DataGridViewTextBoxColumn _productSubcategoryColumn = null!;
    private DataGridViewTextBoxColumn _productNameColumn = null!;
    private DataGridViewTextBoxColumn _productManufacturerColumn = null!;
    private DataGridViewTextBoxColumn _productCompositionColumn = null!;
    private DataGridViewTextBoxColumn _productPriceColumn = null!;
    private DataGridViewTextBoxColumn _productDiscountColumn = null!;
    private DataGridViewTextBoxColumn _productQuantityColumn = null!;
    private DataGridViewTextBoxColumn _productStockColumn = null!;
    private FlowLayoutPanel _detailsFlow = null!;
    private PictureBox _productPicture = null!;
    private Label _productNameLabel = null!;
    private Label _productInfoLabel = null!;
    private Label _priceLabel = null!;
    private Label _stockLabel = null!;
    private Label _descriptionLabel = null!;
    private Label _compositionLabel = null!;
    private Panel _delimiter = null!;
    private Label _guestHintLabel = null!;
    private Panel _detailsBottom = null!;
    private Label _sizeCaption = null!;
    private Label _quantityCaption = null!;
    private ComboBox _sizeComboBox = null!;
    private NumericUpDown _quantityNumeric = null!;
    private Button _addToCartButton = null!;
    private Label _cartHintLabel = null!;
    private DataGridView _cartGrid = null!;
    private DataGridViewTextBoxColumn _cartProductColumn = null!;
    private DataGridViewTextBoxColumn _cartSizeColumn = null!;
    private DataGridViewTextBoxColumn _cartPriceColumn = null!;
    private DataGridViewTextBoxColumn _cartQuantityColumn = null!;
    private DataGridViewTextBoxColumn _cartSumColumn = null!;
    private Panel _cartBottomPanel = null!;
    private Label _cartTotalLabel = null!;
    private Button _removeCartButton = null!;
    private Button _clearCartButton = null!;
    private Button _checkoutButton = null!;
    private Panel _ordersTopPanel = null!;
    private FlowLayoutPanel _ordersTopFlow = null!;
    private Label _ordersTitleLabel = null!;
    private Button _newOrderButton = null!;
    private Button _editOrderButton = null!;
    private Button _deleteOrderButton = null!;
    private Button _refreshOrdersButton = null!;
    private SplitContainer _ordersSplit = null!;
    private DataGridView _ordersGrid = null!;
    private DataGridViewTextBoxColumn _orderIdColumn = null!;
    private DataGridViewTextBoxColumn _orderDateColumn = null!;
    private DataGridViewTextBoxColumn _orderCustomerColumn = null!;
    private DataGridViewTextBoxColumn _orderCreatorColumn = null!;
    private DataGridViewTextBoxColumn _orderPositionsColumn = null!;
    private DataGridViewTextBoxColumn _orderTotalColumn = null!;
    private Panel _orderDetailsPanel = null!;
    private Panel _orderHeaderPanel = null!;
    private Label _orderHeaderLabel = null!;
    private Label _orderTotalLabel = null!;
    private DataGridView _orderItemsGrid = null!;
    private DataGridViewTextBoxColumn _orderItemProductColumn = null!;
    private DataGridViewTextBoxColumn _orderItemSizeColumn = null!;
    private DataGridViewTextBoxColumn _orderItemQuantityColumn = null!;
    private DataGridViewTextBoxColumn _orderItemPriceColumn = null!;
    private DataGridViewTextBoxColumn _orderItemSumColumn = null!;
    private StatusStrip _statusStrip = null!;
    private ToolStripStatusLabel _statusLabel = null!;
    private ToolStripStatusLabel _databaseStatusLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        _headerPanel = new Panel();
        _logoPictureBox = new PictureBox();
        _headerTitleLabel = new Label();
        _userLabel = new Label();
        _logoutButton = new Button();
        _tabs = new TabControl();
        _catalogTab = new TabPage();
        _cartTab = new TabPage();
        _ordersTab = new TabPage();

        _filterPanel = new Panel();
        _filterFlow = new FlowLayoutPanel();
        _searchLabel = new Label();
        _searchTextBox = new TextBox();
        _categoryLabel = new Label();
        _categoryComboBox = new ComboBox();
        _sortLabel = new Label();
        _sortComboBox = new ComboBox();
        _resetFiltersButton = new Button();

        _catalogSplit = new SplitContainer();
        _productsGrid = new DataGridView();
        _productImageColumn = new DataGridViewImageColumn();
        _productCategoryColumn = new DataGridViewTextBoxColumn();
        _productSubcategoryColumn = new DataGridViewTextBoxColumn();
        _productNameColumn = new DataGridViewTextBoxColumn();
        _productManufacturerColumn = new DataGridViewTextBoxColumn();
        _productCompositionColumn = new DataGridViewTextBoxColumn();
        _productPriceColumn = new DataGridViewTextBoxColumn();
        _productDiscountColumn = new DataGridViewTextBoxColumn();
        _productQuantityColumn = new DataGridViewTextBoxColumn();
        _productStockColumn = new DataGridViewTextBoxColumn();
        _detailsFlow = new FlowLayoutPanel();
        _productPicture = new PictureBox();
        _productNameLabel = new Label();
        _productInfoLabel = new Label();
        _priceLabel = new Label();
        _stockLabel = new Label();
        _descriptionLabel = new Label();
        _compositionLabel = new Label();
        _delimiter = new Panel();
        _guestHintLabel = new Label();
        _detailsBottom = new Panel();
        _sizeCaption = new Label();
        _sizeComboBox = new ComboBox();
        _quantityCaption = new Label();
        _quantityNumeric = new NumericUpDown();
        _addToCartButton = new Button();

        _cartHintLabel = new Label();
        _cartGrid = new DataGridView();
        _cartProductColumn = new DataGridViewTextBoxColumn();
        _cartSizeColumn = new DataGridViewTextBoxColumn();
        _cartPriceColumn = new DataGridViewTextBoxColumn();
        _cartQuantityColumn = new DataGridViewTextBoxColumn();
        _cartSumColumn = new DataGridViewTextBoxColumn();
        _cartBottomPanel = new Panel();
        _cartTotalLabel = new Label();
        _removeCartButton = new Button();
        _clearCartButton = new Button();
        _checkoutButton = new Button();

        _ordersTopPanel = new Panel();
        _ordersTopFlow = new FlowLayoutPanel();
        _ordersTitleLabel = new Label();
        _newOrderButton = new Button();
        _editOrderButton = new Button();
        _deleteOrderButton = new Button();
        _refreshOrdersButton = new Button();
        _ordersSplit = new SplitContainer();
        _ordersGrid = new DataGridView();
        _orderIdColumn = new DataGridViewTextBoxColumn();
        _orderDateColumn = new DataGridViewTextBoxColumn();
        _orderCustomerColumn = new DataGridViewTextBoxColumn();
        _orderCreatorColumn = new DataGridViewTextBoxColumn();
        _orderPositionsColumn = new DataGridViewTextBoxColumn();
        _orderTotalColumn = new DataGridViewTextBoxColumn();
        _orderDetailsPanel = new Panel();
        _orderHeaderPanel = new Panel();
        _orderHeaderLabel = new Label();
        _orderTotalLabel = new Label();
        _orderItemsGrid = new DataGridView();
        _orderItemProductColumn = new DataGridViewTextBoxColumn();
        _orderItemSizeColumn = new DataGridViewTextBoxColumn();
        _orderItemQuantityColumn = new DataGridViewTextBoxColumn();
        _orderItemPriceColumn = new DataGridViewTextBoxColumn();
        _orderItemSumColumn = new DataGridViewTextBoxColumn();
        _statusStrip = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel();
        _databaseStatusLabel = new ToolStripStatusLabel();

        _headerPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_logoPictureBox).BeginInit();
        _tabs.SuspendLayout();
        _catalogTab.SuspendLayout();
        _cartTab.SuspendLayout();
        _ordersTab.SuspendLayout();
        _filterPanel.SuspendLayout();
        _filterFlow.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_catalogSplit).BeginInit();
        _catalogSplit.Panel1.SuspendLayout();
        _catalogSplit.Panel2.SuspendLayout();
        _catalogSplit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_productsGrid).BeginInit();
        _detailsFlow.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_productPicture).BeginInit();
        _detailsBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_quantityNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cartGrid).BeginInit();
        _cartBottomPanel.SuspendLayout();
        _ordersTopPanel.SuspendLayout();
        _ordersTopFlow.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_ordersSplit).BeginInit();
        _ordersSplit.Panel1.SuspendLayout();
        _ordersSplit.Panel2.SuspendLayout();
        _ordersSplit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_ordersGrid).BeginInit();
        _orderDetailsPanel.SuspendLayout();
        _orderHeaderPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_orderItemsGrid).BeginInit();
        _statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // _headerPanel
        //
        _headerPanel.BackColor = Color.White;
        _headerPanel.Controls.Add(_logoutButton);
        _headerPanel.Controls.Add(_userLabel);
        _headerPanel.Controls.Add(_headerTitleLabel);
        _headerPanel.Controls.Add(_logoPictureBox);
        _headerPanel.Dock = DockStyle.Top;
        _headerPanel.Location = new Point(0, 0);
        _headerPanel.Name = "_headerPanel";
        _headerPanel.Padding = new Padding(8);
        _headerPanel.Size = new Size(1280, 64);
        _headerPanel.TabIndex = 2;
        //
        // _logoPictureBox
        //
        _logoPictureBox.BackColor = Color.White;
        _logoPictureBox.Location = new Point(10, 8);
        _logoPictureBox.Name = "_logoPictureBox";
        _logoPictureBox.Size = new Size(48, 48);
        _logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        _logoPictureBox.TabIndex = 0;
        _logoPictureBox.TabStop = false;
        //
        // _headerTitleLabel
        //
        _headerTitleLabel.AutoSize = true;
        _headerTitleLabel.Font = new Font("Calibri", 12F, FontStyle.Bold);
        _headerTitleLabel.Location = new Point(70, 19);
        _headerTitleLabel.Name = "_headerTitleLabel";
        _headerTitleLabel.TabIndex = 1;
        _headerTitleLabel.Text = "Чудо Обувь — оформление заказов";
        //
        // _userLabel
        //
        _userLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _userLabel.AutoSize = false;
        _userLabel.Font = new Font("Calibri", 10F);
        _userLabel.ForeColor = Color.DimGray;
        _userLabel.Location = new Point(740, 12);
        _userLabel.Name = "_userLabel";
        _userLabel.Size = new Size(430, 40);
        _userLabel.TabIndex = 2;
        _userLabel.Text = "Пользователь";
        _userLabel.TextAlign = ContentAlignment.MiddleRight;
        //
        // _logoutButton
        //
        _logoutButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _logoutButton.BackColor = Color.FromArgb(210, 246, 231);
        _logoutButton.Cursor = Cursors.Hand;
        _logoutButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _logoutButton.FlatAppearance.BorderSize = 1;
        _logoutButton.FlatStyle = FlatStyle.Flat;
        _logoutButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _logoutButton.ForeColor = Color.Black;
        _logoutButton.Location = new Point(1178, 16);
        _logoutButton.Name = "_logoutButton";
        _logoutButton.Size = new Size(90, 32);
        _logoutButton.TabIndex = 3;
        _logoutButton.Text = "Выход";
        _logoutButton.UseVisualStyleBackColor = false;
        _logoutButton.Click += LogoutButton_Click;
        //
        // _tabs
        //
        _tabs.Controls.Add(_catalogTab);
        _tabs.Controls.Add(_cartTab);
        _tabs.Controls.Add(_ordersTab);
        _tabs.Dock = DockStyle.Fill;
        _tabs.Font = new Font("Calibri", 10F);
        _tabs.Location = new Point(0, 64);
        _tabs.Name = "_tabs";
        _tabs.Padding = new Point(18, 6);
        _tabs.SelectedIndex = 0;
        _tabs.Size = new Size(1280, 714);
        _tabs.TabIndex = 1;
        //
        // _catalogTab
        //
        _catalogTab.BackColor = Color.White;
        _catalogTab.Controls.Add(_catalogSplit);
        _catalogTab.Controls.Add(_filterPanel);
        _catalogTab.Location = new Point(4, 30);
        _catalogTab.Name = "_catalogTab";
        _catalogTab.Padding = new Padding(6);
        _catalogTab.Size = new Size(1272, 680);
        _catalogTab.TabIndex = 0;
        _catalogTab.Text = "Каталог";
        _catalogTab.UseVisualStyleBackColor = false;
        //
        // _filterPanel
        //
        _filterPanel.BackColor = Color.FromArgb(210, 246, 231);
        _filterPanel.Controls.Add(_filterFlow);
        _filterPanel.Dock = DockStyle.Top;
        _filterPanel.Location = new Point(6, 6);
        _filterPanel.Name = "_filterPanel";
        _filterPanel.Padding = new Padding(4);
        _filterPanel.Size = new Size(1260, 50);
        _filterPanel.TabIndex = 0;
        //
        // _filterFlow
        //
        _filterFlow.AutoScroll = true;
        _filterFlow.Controls.Add(_searchLabel);
        _filterFlow.Controls.Add(_searchTextBox);
        _filterFlow.Controls.Add(_categoryLabel);
        _filterFlow.Controls.Add(_categoryComboBox);
        _filterFlow.Controls.Add(_sortLabel);
        _filterFlow.Controls.Add(_sortComboBox);
        _filterFlow.Controls.Add(_resetFiltersButton);
        _filterFlow.Dock = DockStyle.Fill;
        _filterFlow.FlowDirection = FlowDirection.LeftToRight;
        _filterFlow.Location = new Point(4, 4);
        _filterFlow.Name = "_filterFlow";
        _filterFlow.Size = new Size(1252, 42);
        _filterFlow.TabIndex = 0;
        _filterFlow.WrapContents = false;
        //
        // _searchLabel
        //
        _searchLabel.AutoSize = true;
        _searchLabel.Font = new Font("Calibri", 10F);
        _searchLabel.Location = new Point(4, 14);
        _searchLabel.Margin = new Padding(4, 14, 4, 0);
        _searchLabel.Name = "_searchLabel";
        _searchLabel.TabIndex = 0;
        _searchLabel.Text = "Поиск:";
        //
        // _searchTextBox
        //
        _searchTextBox.Font = new Font("Calibri", 10F);
        _searchTextBox.Location = new Point(50, 24);
        _searchTextBox.Margin = new Padding(0, 10, 14, 0);
        _searchTextBox.Name = "_searchTextBox";
        _searchTextBox.Size = new Size(230, 23);
        _searchTextBox.TabIndex = 1;
        _searchTextBox.TextChanged += SearchTextBox_TextChanged;
        //
        // _categoryLabel
        //
        _categoryLabel.AutoSize = true;
        _categoryLabel.Font = new Font("Calibri", 10F);
        _categoryLabel.Location = new Point(298, 14);
        _categoryLabel.Margin = new Padding(4, 14, 4, 0);
        _categoryLabel.Name = "_categoryLabel";
        _categoryLabel.TabIndex = 2;
        _categoryLabel.Text = "Категория:";
        //
        // _categoryComboBox
        //
        _categoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _categoryComboBox.Font = new Font("Calibri", 10F);
        _categoryComboBox.Location = new Point(390, 24);
        _categoryComboBox.Margin = new Padding(0, 10, 14, 0);
        _categoryComboBox.Name = "_categoryComboBox";
        _categoryComboBox.Size = new Size(190, 23);
        _categoryComboBox.TabIndex = 3;
        _categoryComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
        //
        // _sortLabel
        //
        _sortLabel.AutoSize = true;
        _sortLabel.Font = new Font("Calibri", 10F);
        _sortLabel.Location = new Point(598, 14);
        _sortLabel.Margin = new Padding(4, 14, 4, 0);
        _sortLabel.Name = "_sortLabel";
        _sortLabel.TabIndex = 4;
        _sortLabel.Text = "Сортировка:";
        //
        // _sortComboBox
        //
        _sortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _sortComboBox.Font = new Font("Calibri", 10F);
        _sortComboBox.Location = new Point(695, 24);
        _sortComboBox.Margin = new Padding(0, 10, 14, 0);
        _sortComboBox.Name = "_sortComboBox";
        _sortComboBox.Size = new Size(230, 23);
        _sortComboBox.TabIndex = 5;
        _sortComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
        //
        // _resetFiltersButton
        //
        _resetFiltersButton.BackColor = Color.FromArgb(210, 246, 231);
        _resetFiltersButton.Cursor = Cursors.Hand;
        _resetFiltersButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _resetFiltersButton.FlatAppearance.BorderSize = 1;
        _resetFiltersButton.FlatStyle = FlatStyle.Flat;
        _resetFiltersButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _resetFiltersButton.ForeColor = Color.Black;
        _resetFiltersButton.Location = new Point(939, 24);
        _resetFiltersButton.Margin = new Padding(0, 8, 0, 0);
        _resetFiltersButton.Name = "_resetFiltersButton";
        _resetFiltersButton.Size = new Size(100, 30);
        _resetFiltersButton.TabIndex = 6;
        _resetFiltersButton.Text = "Сбросить";
        _resetFiltersButton.UseVisualStyleBackColor = false;
        _resetFiltersButton.Click += ResetFiltersButton_Click;
        //
        // _catalogSplit
        //
        _catalogSplit.BackColor = Color.FromArgb(210, 246, 231);
        _catalogSplit.Dock = DockStyle.Fill;
        _catalogSplit.FixedPanel = FixedPanel.Panel2;
        _catalogSplit.Location = new Point(6, 56);
        _catalogSplit.Name = "_catalogSplit";
        _catalogSplit.Orientation = Orientation.Vertical;
        _catalogSplit.Panel1.BackColor = Color.White;
        _catalogSplit.Panel1.Controls.Add(_productsGrid);
        _catalogSplit.Panel2.BackColor = Color.FromArgb(210, 246, 231);
        _catalogSplit.Panel2.Controls.Add(_detailsFlow);
        _catalogSplit.Panel2.Controls.Add(_detailsBottom);
        _catalogSplit.Size = new Size(1260, 618);
        _catalogSplit.SplitterDistance = 900;
        _catalogSplit.SplitterWidth = 6;
        _catalogSplit.TabIndex = 1;
        //
        // _productsGrid
        //
        _productsGrid.AllowUserToAddRows = false;
        _productsGrid.AllowUserToDeleteRows = false;
        _productsGrid.AllowUserToResizeRows = false;
        _productsGrid.AutoGenerateColumns = false;
        _productsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _productsGrid.BackgroundColor = Color.White;
        _productsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        _productsGrid.Columns.AddRange(new DataGridViewColumn[]
        {
            _productImageColumn, _productCategoryColumn, _productSubcategoryColumn,
            _productNameColumn, _productManufacturerColumn, _productCompositionColumn,
            _productPriceColumn, _productDiscountColumn, _productQuantityColumn, _productStockColumn
        });
        _productsGrid.Dock = DockStyle.Fill;
        _productsGrid.Location = new Point(0, 0);
        _productsGrid.MultiSelect = false;
        _productsGrid.Name = "_productsGrid";
        _productsGrid.ReadOnly = true;
        _productsGrid.RowHeadersVisible = false;
        _productsGrid.RowTemplate.Height = 64;
        _productsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _productsGrid.Size = new Size(900, 618);
        _productsGrid.TabIndex = 0;
        _productsGrid.CellFormatting += ProductsGrid_CellFormatting;
        _productsGrid.SelectionChanged += ProductsGrid_SelectionChanged;
        //
        // _productImageColumn
        //
        _productImageColumn.FillWeight = 55F;
        _productImageColumn.HeaderText = "";
        _productImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        _productImageColumn.MinimumWidth = 50;
        _productImageColumn.Name = "_productImageColumn";
        _productImageColumn.ReadOnly = true;
        _productImageColumn.ValuesAreIcons = false;
        //
        // _productCategoryColumn
        //
        _productCategoryColumn.FillWeight = 80F;
        _productCategoryColumn.HeaderText = "Категория";
        _productCategoryColumn.MinimumWidth = 80;
        _productCategoryColumn.Name = "_productCategoryColumn";
        _productCategoryColumn.ReadOnly = true;
        //
        // _productSubcategoryColumn
        //
        _productSubcategoryColumn.FillWeight = 80F;
        _productSubcategoryColumn.HeaderText = "Подкатегория";
        _productSubcategoryColumn.MinimumWidth = 80;
        _productSubcategoryColumn.Name = "_productSubcategoryColumn";
        _productSubcategoryColumn.ReadOnly = true;
        //
        // _productNameColumn
        //
        _productNameColumn.FillWeight = 190F;
        _productNameColumn.HeaderText = "Наименование";
        _productNameColumn.MinimumWidth = 160;
        _productNameColumn.Name = "_productNameColumn";
        _productNameColumn.ReadOnly = true;
        //
        // _productManufacturerColumn
        //
        _productManufacturerColumn.FillWeight = 110F;
        _productManufacturerColumn.HeaderText = "Производство";
        _productManufacturerColumn.MinimumWidth = 100;
        _productManufacturerColumn.Name = "_productManufacturerColumn";
        _productManufacturerColumn.ReadOnly = true;
        //
        // _productCompositionColumn
        //
        _productCompositionColumn.FillWeight = 250F;
        _productCompositionColumn.HeaderText = "Состав";
        _productCompositionColumn.MinimumWidth = 180;
        _productCompositionColumn.Name = "_productCompositionColumn";
        _productCompositionColumn.ReadOnly = true;
        //
        // _productPriceColumn
        //
        _productPriceColumn.FillWeight = 95F;
        _productPriceColumn.HeaderText = "Цена со скидкой";
        _productPriceColumn.Name = "_productPriceColumn";
        _productPriceColumn.ReadOnly = true;
        DataGridViewCellStyle productPriceStyle = new DataGridViewCellStyle();
        productPriceStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _productPriceColumn.DefaultCellStyle = productPriceStyle;
        //
        // _productDiscountColumn
        //
        _productDiscountColumn.FillWeight = 60F;
        _productDiscountColumn.HeaderText = "Скидка";
        _productDiscountColumn.Name = "_productDiscountColumn";
        _productDiscountColumn.ReadOnly = true;
        DataGridViewCellStyle productDiscountStyle = new DataGridViewCellStyle();
        productDiscountStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _productDiscountColumn.DefaultCellStyle = productDiscountStyle;
        //
        // _productQuantityColumn
        //
        _productQuantityColumn.FillWeight = 60F;
        _productQuantityColumn.HeaderText = "Кол-во";
        _productQuantityColumn.Name = "_productQuantityColumn";
        _productQuantityColumn.ReadOnly = true;
        DataGridViewCellStyle productQuantityStyle = new DataGridViewCellStyle();
        productQuantityStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _productQuantityColumn.DefaultCellStyle = productQuantityStyle;
        //
        // _productStockColumn
        //
        _productStockColumn.FillWeight = 65F;
        _productStockColumn.HeaderText = "Наличие";
        _productStockColumn.Name = "_productStockColumn";
        _productStockColumn.ReadOnly = true;
        DataGridViewCellStyle productStockStyle = new DataGridViewCellStyle();
        productStockStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _productStockColumn.DefaultCellStyle = productStockStyle;
        //
        // _detailsFlow
        //
        _detailsFlow.AutoScroll = true;
        _detailsFlow.BackColor = Color.FromArgb(210, 246, 231);
        _detailsFlow.Controls.Add(_productPicture);
        _detailsFlow.Controls.Add(_productNameLabel);
        _detailsFlow.Controls.Add(_productInfoLabel);
        _detailsFlow.Controls.Add(_priceLabel);
        _detailsFlow.Controls.Add(_stockLabel);
        _detailsFlow.Controls.Add(_descriptionLabel);
        _detailsFlow.Controls.Add(_compositionLabel);
        _detailsFlow.Controls.Add(_delimiter);
        _detailsFlow.Controls.Add(_guestHintLabel);
        _detailsFlow.Dock = DockStyle.Fill;
        _detailsFlow.FlowDirection = FlowDirection.TopDown;
        _detailsFlow.Location = new Point(0, 0);
        _detailsFlow.Name = "_detailsFlow";
        _detailsFlow.Padding = new Padding(10);
        _detailsFlow.Size = new Size(354, 468);
        _detailsFlow.TabIndex = 0;
        _detailsFlow.WrapContents = false;
        //
        // _productPicture
        //
        _productPicture.BackColor = Color.White;
        _productPicture.Location = new Point(10, 10);
        _productPicture.Margin = new Padding(0, 0, 0, 8);
        _productPicture.Name = "_productPicture";
        _productPicture.Size = new Size(300, 210);
        _productPicture.SizeMode = PictureBoxSizeMode.Zoom;
        _productPicture.TabIndex = 0;
        _productPicture.TabStop = false;
        //
        // _productNameLabel
        //
        _productNameLabel.AutoSize = true;
        _productNameLabel.Font = new Font("Calibri", 12F, FontStyle.Bold);
        _productNameLabel.Location = new Point(10, 228);
        _productNameLabel.Margin = new Padding(0, 0, 0, 4);
        _productNameLabel.MaximumSize = new Size(300, 0);
        _productNameLabel.Name = "_productNameLabel";
        _productNameLabel.TabIndex = 1;
        //
        // _productInfoLabel
        //
        _productInfoLabel.AutoSize = true;
        _productInfoLabel.ForeColor = Color.DimGray;
        _productInfoLabel.Location = new Point(10, 252);
        _productInfoLabel.Margin = new Padding(0, 0, 0, 8);
        _productInfoLabel.MaximumSize = new Size(300, 0);
        _productInfoLabel.Name = "_productInfoLabel";
        _productInfoLabel.TabIndex = 2;
        //
        // _priceLabel
        //
        _priceLabel.AutoSize = true;
        _priceLabel.Font = new Font("Calibri", 12F, FontStyle.Bold);
        _priceLabel.ForeColor = Color.FromArgb(79, 145, 142);
        _priceLabel.Location = new Point(10, 280);
        _priceLabel.Margin = new Padding(0, 0, 0, 4);
        _priceLabel.Name = "_priceLabel";
        _priceLabel.TabIndex = 3;
        //
        // _stockLabel
        //
        _stockLabel.AutoSize = true;
        _stockLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _stockLabel.Location = new Point(10, 304);
        _stockLabel.Margin = new Padding(0, 0, 0, 8);
        _stockLabel.Name = "_stockLabel";
        _stockLabel.TabIndex = 4;
        //
        // _descriptionLabel
        //
        _descriptionLabel.AutoSize = true;
        _descriptionLabel.Location = new Point(10, 330);
        _descriptionLabel.Margin = new Padding(0, 0, 0, 6);
        _descriptionLabel.MaximumSize = new Size(300, 0);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.TabIndex = 5;
        //
        // _compositionLabel
        //
        _compositionLabel.AutoSize = true;
        _compositionLabel.ForeColor = Color.DimGray;
        _compositionLabel.Location = new Point(10, 356);
        _compositionLabel.Margin = new Padding(0, 0, 0, 8);
        _compositionLabel.MaximumSize = new Size(300, 0);
        _compositionLabel.Name = "_compositionLabel";
        _compositionLabel.TabIndex = 6;
        //
        // _delimiter
        //
        _delimiter.BackColor = Color.Silver;
        _delimiter.Location = new Point(10, 382);
        _delimiter.Margin = new Padding(0, 2, 0, 8);
        _delimiter.Name = "_delimiter";
        _delimiter.Size = new Size(300, 1);
        _delimiter.TabIndex = 7;
        //
        // _guestHintLabel
        //
        _guestHintLabel.AutoSize = true;
        _guestHintLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _guestHintLabel.ForeColor = Color.Firebrick;
        _guestHintLabel.Location = new Point(10, 401);
        _guestHintLabel.Margin = new Padding(0, 6, 0, 0);
        _guestHintLabel.MaximumSize = new Size(300, 0);
        _guestHintLabel.Name = "_guestHintLabel";
        _guestHintLabel.TabIndex = 8;
        _guestHintLabel.Text = "Для оформления заказа войдите как пользователь.";
        _guestHintLabel.Visible = false;
        //
        // _detailsBottom
        //
        _detailsBottom.BackColor = Color.FromArgb(210, 246, 231);
        _detailsBottom.Controls.Add(_addToCartButton);
        _detailsBottom.Controls.Add(_quantityNumeric);
        _detailsBottom.Controls.Add(_quantityCaption);
        _detailsBottom.Controls.Add(_sizeComboBox);
        _detailsBottom.Controls.Add(_sizeCaption);
        _detailsBottom.Dock = DockStyle.Bottom;
        _detailsBottom.Location = new Point(0, 468);
        _detailsBottom.Name = "_detailsBottom";
        _detailsBottom.Padding = new Padding(10);
        _detailsBottom.Size = new Size(354, 150);
        _detailsBottom.TabIndex = 1;
        //
        // _sizeCaption
        //
        _sizeCaption.AutoSize = true;
        _sizeCaption.Font = new Font("Calibri", 10F);
        _sizeCaption.Location = new Point(10, 4);
        _sizeCaption.Margin = Padding.Empty;
        _sizeCaption.Name = "_sizeCaption";
        _sizeCaption.TabIndex = 0;
        _sizeCaption.Text = "Размер:";
        //
        // _sizeComboBox
        //
        _sizeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _sizeComboBox.Font = new Font("Calibri", 10F);
        _sizeComboBox.Location = new Point(10, 24);
        _sizeComboBox.Name = "_sizeComboBox";
        _sizeComboBox.Size = new Size(300, 23);
        _sizeComboBox.TabIndex = 1;
        _sizeComboBox.SelectedIndexChanged += SizeComboBox_SelectedIndexChanged;
        //
        // _quantityCaption
        //
        _quantityCaption.AutoSize = true;
        _quantityCaption.Font = new Font("Calibri", 10F);
        _quantityCaption.Location = new Point(10, 58);
        _quantityCaption.Margin = Padding.Empty;
        _quantityCaption.Name = "_quantityCaption";
        _quantityCaption.TabIndex = 2;
        _quantityCaption.Text = "Количество:";
        //
        // _quantityNumeric
        //
        _quantityNumeric.Font = new Font("Calibri", 10F);
        _quantityNumeric.Location = new Point(10, 78);
        _quantityNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
        _quantityNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        _quantityNumeric.Name = "_quantityNumeric";
        _quantityNumeric.Size = new Size(100, 25);
        _quantityNumeric.TabIndex = 3;
        _quantityNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // _addToCartButton
        //
        _addToCartButton.BackColor = Color.FromArgb(112, 178, 175);
        _addToCartButton.Cursor = Cursors.Hand;
        _addToCartButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _addToCartButton.FlatAppearance.BorderSize = 1;
        _addToCartButton.FlatStyle = FlatStyle.Flat;
        _addToCartButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _addToCartButton.ForeColor = Color.White;
        _addToCartButton.Location = new Point(10, 108);
        _addToCartButton.Name = "_addToCartButton";
        _addToCartButton.Size = new Size(300, 34);
        _addToCartButton.TabIndex = 4;
        _addToCartButton.Text = "Добавить в корзину";
        _addToCartButton.UseVisualStyleBackColor = false;
        _addToCartButton.Click += AddToCartButton_Click;
        //
        // _cartTab
        //
        _cartTab.BackColor = Color.White;
        _cartTab.Controls.Add(_cartGrid);
        _cartTab.Controls.Add(_cartBottomPanel);
        _cartTab.Controls.Add(_cartHintLabel);
        _cartTab.Location = new Point(4, 30);
        _cartTab.Name = "_cartTab";
        _cartTab.Padding = new Padding(6);
        _cartTab.Size = new Size(1272, 680);
        _cartTab.TabIndex = 1;
        _cartTab.Text = "Корзина";
        _cartTab.UseVisualStyleBackColor = false;
        _cartTab.Enter += CartTab_Enter;
        //
        // _cartHintLabel
        //
        _cartHintLabel.Dock = DockStyle.Top;
        _cartHintLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _cartHintLabel.ForeColor = Color.DimGray;
        _cartHintLabel.Location = new Point(6, 6);
        _cartHintLabel.Name = "_cartHintLabel";
        _cartHintLabel.Size = new Size(1260, 32);
        _cartHintLabel.TabIndex = 0;
        _cartHintLabel.Text = "Проверьте состав заказа и оформите его.";
        _cartHintLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // _cartGrid
        //
        _cartGrid.AllowUserToAddRows = false;
        _cartGrid.AllowUserToDeleteRows = false;
        _cartGrid.AutoGenerateColumns = false;
        _cartGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _cartGrid.Columns.AddRange(new DataGridViewColumn[] { _cartProductColumn, _cartSizeColumn, _cartPriceColumn, _cartQuantityColumn, _cartSumColumn });
        _cartGrid.Dock = DockStyle.Fill;
        _cartGrid.Location = new Point(6, 38);
        _cartGrid.MultiSelect = false;
        _cartGrid.Name = "_cartGrid";
        _cartGrid.ReadOnly = true;
        _cartGrid.RowHeadersVisible = false;
        _cartGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _cartGrid.Size = new Size(1260, 572);
        _cartGrid.TabIndex = 1;
        //
        // _cartProductColumn
        //
        _cartProductColumn.FillWeight = 320F;
        _cartProductColumn.HeaderText = "Товар";
        _cartProductColumn.Name = "_cartProductColumn";
        _cartProductColumn.ReadOnly = true;
        //
        // _cartSizeColumn
        //
        _cartSizeColumn.FillWeight = 70F;
        _cartSizeColumn.HeaderText = "Размер";
        _cartSizeColumn.Name = "_cartSizeColumn";
        _cartSizeColumn.ReadOnly = true;
        //
        // _cartPriceColumn
        //
        _cartPriceColumn.FillWeight = 100F;
        _cartPriceColumn.HeaderText = "Цена";
        _cartPriceColumn.Name = "_cartPriceColumn";
        _cartPriceColumn.ReadOnly = true;
        DataGridViewCellStyle cartPriceStyle = new DataGridViewCellStyle();
        cartPriceStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _cartPriceColumn.DefaultCellStyle = cartPriceStyle;
        //
        // _cartQuantityColumn
        //
        _cartQuantityColumn.FillWeight = 70F;
        _cartQuantityColumn.HeaderText = "Кол-во";
        _cartQuantityColumn.Name = "_cartQuantityColumn";
        _cartQuantityColumn.ReadOnly = true;
        DataGridViewCellStyle cartQuantityStyle = new DataGridViewCellStyle();
        cartQuantityStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _cartQuantityColumn.DefaultCellStyle = cartQuantityStyle;
        //
        // _cartSumColumn
        //
        _cartSumColumn.FillWeight = 110F;
        _cartSumColumn.HeaderText = "Сумма";
        _cartSumColumn.Name = "_cartSumColumn";
        _cartSumColumn.ReadOnly = true;
        DataGridViewCellStyle cartSumStyle = new DataGridViewCellStyle();
        cartSumStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _cartSumColumn.DefaultCellStyle = cartSumStyle;
        //
        // _cartBottomPanel
        //
        _cartBottomPanel.BackColor = Color.FromArgb(210, 246, 231);
        _cartBottomPanel.Controls.Add(_checkoutButton);
        _cartBottomPanel.Controls.Add(_clearCartButton);
        _cartBottomPanel.Controls.Add(_removeCartButton);
        _cartBottomPanel.Controls.Add(_cartTotalLabel);
        _cartBottomPanel.Dock = DockStyle.Bottom;
        _cartBottomPanel.Location = new Point(6, 610);
        _cartBottomPanel.Name = "_cartBottomPanel";
        _cartBottomPanel.Padding = new Padding(8);
        _cartBottomPanel.Size = new Size(1260, 64);
        _cartBottomPanel.TabIndex = 2;
        //
        // _cartTotalLabel
        //
        _cartTotalLabel.AutoSize = true;
        _cartTotalLabel.Font = new Font("Calibri", 13F, FontStyle.Bold);
        _cartTotalLabel.Location = new Point(10, 18);
        _cartTotalLabel.Name = "_cartTotalLabel";
        _cartTotalLabel.TabIndex = 0;
        _cartTotalLabel.Text = "Итого: 0,00 руб.";
        //
        // _removeCartButton
        //
        _removeCartButton.BackColor = Color.FromArgb(210, 246, 231);
        _removeCartButton.Cursor = Cursors.Hand;
        _removeCartButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _removeCartButton.FlatAppearance.BorderSize = 1;
        _removeCartButton.FlatStyle = FlatStyle.Flat;
        _removeCartButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _removeCartButton.ForeColor = Color.Black;
        _removeCartButton.Location = new Point(360, 14);
        _removeCartButton.Name = "_removeCartButton";
        _removeCartButton.Size = new Size(150, 34);
        _removeCartButton.TabIndex = 1;
        _removeCartButton.Text = "Удалить позицию";
        _removeCartButton.UseVisualStyleBackColor = false;
        _removeCartButton.Click += RemoveCartButton_Click;
        //
        // _clearCartButton
        //
        _clearCartButton.BackColor = Color.FromArgb(210, 246, 231);
        _clearCartButton.Cursor = Cursors.Hand;
        _clearCartButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _clearCartButton.FlatAppearance.BorderSize = 1;
        _clearCartButton.FlatStyle = FlatStyle.Flat;
        _clearCartButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _clearCartButton.ForeColor = Color.Black;
        _clearCartButton.Location = new Point(520, 14);
        _clearCartButton.Name = "_clearCartButton";
        _clearCartButton.Size = new Size(110, 34);
        _clearCartButton.TabIndex = 2;
        _clearCartButton.Text = "Очистить";
        _clearCartButton.UseVisualStyleBackColor = false;
        _clearCartButton.Click += ClearCartButton_Click;
        //
        // _checkoutButton
        //
        _checkoutButton.BackColor = Color.FromArgb(112, 178, 175);
        _checkoutButton.Cursor = Cursors.Hand;
        _checkoutButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _checkoutButton.FlatAppearance.BorderSize = 1;
        _checkoutButton.FlatStyle = FlatStyle.Flat;
        _checkoutButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _checkoutButton.ForeColor = Color.White;
        _checkoutButton.Location = new Point(640, 14);
        _checkoutButton.Name = "_checkoutButton";
        _checkoutButton.Size = new Size(180, 34);
        _checkoutButton.TabIndex = 3;
        _checkoutButton.Text = "Оформить заказ";
        _checkoutButton.UseVisualStyleBackColor = false;
        _checkoutButton.Click += CheckoutButton_Click;
        //
        // _ordersTab
        //
        _ordersTab.BackColor = Color.White;
        _ordersTab.Controls.Add(_ordersSplit);
        _ordersTab.Controls.Add(_ordersTopPanel);
        _ordersTab.Location = new Point(4, 30);
        _ordersTab.Name = "_ordersTab";
        _ordersTab.Padding = new Padding(6);
        _ordersTab.Size = new Size(1272, 680);
        _ordersTab.TabIndex = 2;
        _ordersTab.Text = "Заказы";
        _ordersTab.UseVisualStyleBackColor = false;
        _ordersTab.Enter += OrdersTab_Enter;
        //
        // _ordersTopPanel
        //
        _ordersTopPanel.BackColor = Color.FromArgb(210, 246, 231);
        _ordersTopPanel.Controls.Add(_ordersTopFlow);
        _ordersTopPanel.Dock = DockStyle.Top;
        _ordersTopPanel.Location = new Point(6, 6);
        _ordersTopPanel.Name = "_ordersTopPanel";
        _ordersTopPanel.Padding = new Padding(6);
        _ordersTopPanel.Size = new Size(1260, 50);
        _ordersTopPanel.TabIndex = 0;
        //
        // _ordersTopFlow
        //
        _ordersTopFlow.Controls.Add(_ordersTitleLabel);
        _ordersTopFlow.Controls.Add(_newOrderButton);
        _ordersTopFlow.Controls.Add(_editOrderButton);
        _ordersTopFlow.Controls.Add(_deleteOrderButton);
        _ordersTopFlow.Controls.Add(_refreshOrdersButton);
        _ordersTopFlow.Dock = DockStyle.Fill;
        _ordersTopFlow.FlowDirection = FlowDirection.LeftToRight;
        _ordersTopFlow.Location = new Point(6, 6);
        _ordersTopFlow.Name = "_ordersTopFlow";
        _ordersTopFlow.Size = new Size(1248, 38);
        _ordersTopFlow.TabIndex = 0;
        _ordersTopFlow.WrapContents = false;
        //
        // _ordersTitleLabel
        //
        _ordersTitleLabel.AutoSize = true;
        _ordersTitleLabel.Font = new Font("Calibri", 12F, FontStyle.Bold);
        _ordersTitleLabel.Location = new Point(4, 8);
        _ordersTitleLabel.Margin = new Padding(4, 8, 20, 0);
        _ordersTitleLabel.Name = "_ordersTitleLabel";
        _ordersTitleLabel.TabIndex = 0;
        _ordersTitleLabel.Text = "Список заказов";
        //
        // _newOrderButton
        //
        _newOrderButton.BackColor = Color.FromArgb(112, 178, 175);
        _newOrderButton.Cursor = Cursors.Hand;
        _newOrderButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _newOrderButton.FlatAppearance.BorderSize = 1;
        _newOrderButton.FlatStyle = FlatStyle.Flat;
        _newOrderButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _newOrderButton.ForeColor = Color.White;
        _newOrderButton.Location = new Point(190, 6);
        _newOrderButton.Margin = new Padding(0, 6, 6, 0);
        _newOrderButton.Name = "_newOrderButton";
        _newOrderButton.Size = new Size(140, 30);
        _newOrderButton.TabIndex = 1;
        _newOrderButton.Text = "Новый заказ";
        _newOrderButton.UseVisualStyleBackColor = false;
        _newOrderButton.Click += NewOrderButton_Click;
        //
        // _editOrderButton
        //
        _editOrderButton.BackColor = Color.FromArgb(210, 246, 231);
        _editOrderButton.Cursor = Cursors.Hand;
        _editOrderButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _editOrderButton.FlatAppearance.BorderSize = 1;
        _editOrderButton.FlatStyle = FlatStyle.Flat;
        _editOrderButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _editOrderButton.ForeColor = Color.Black;
        _editOrderButton.Location = new Point(336, 6);
        _editOrderButton.Margin = new Padding(0, 6, 6, 0);
        _editOrderButton.Name = "_editOrderButton";
        _editOrderButton.Size = new Size(150, 30);
        _editOrderButton.TabIndex = 2;
        _editOrderButton.Text = "Редактировать";
        _editOrderButton.UseVisualStyleBackColor = false;
        _editOrderButton.Click += EditOrderButton_Click;
        //
        // _deleteOrderButton
        //
        _deleteOrderButton.BackColor = Color.FromArgb(210, 246, 231);
        _deleteOrderButton.Cursor = Cursors.Hand;
        _deleteOrderButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _deleteOrderButton.FlatAppearance.BorderSize = 1;
        _deleteOrderButton.FlatStyle = FlatStyle.Flat;
        _deleteOrderButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _deleteOrderButton.ForeColor = Color.Black;
        _deleteOrderButton.Location = new Point(492, 6);
        _deleteOrderButton.Margin = new Padding(0, 6, 6, 0);
        _deleteOrderButton.Name = "_deleteOrderButton";
        _deleteOrderButton.Size = new Size(110, 30);
        _deleteOrderButton.TabIndex = 3;
        _deleteOrderButton.Text = "Удалить";
        _deleteOrderButton.UseVisualStyleBackColor = false;
        _deleteOrderButton.Click += DeleteOrderButton_Click;
        //
        // _refreshOrdersButton
        //
        _refreshOrdersButton.BackColor = Color.FromArgb(210, 246, 231);
        _refreshOrdersButton.Cursor = Cursors.Hand;
        _refreshOrdersButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _refreshOrdersButton.FlatAppearance.BorderSize = 1;
        _refreshOrdersButton.FlatStyle = FlatStyle.Flat;
        _refreshOrdersButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _refreshOrdersButton.ForeColor = Color.Black;
        _refreshOrdersButton.Location = new Point(608, 6);
        _refreshOrdersButton.Margin = new Padding(0, 6, 0, 0);
        _refreshOrdersButton.Name = "_refreshOrdersButton";
        _refreshOrdersButton.Size = new Size(110, 30);
        _refreshOrdersButton.TabIndex = 4;
        _refreshOrdersButton.Text = "Обновить";
        _refreshOrdersButton.UseVisualStyleBackColor = false;
        _refreshOrdersButton.Click += RefreshOrdersButton_Click;
        //
        // _ordersSplit
        //
        _ordersSplit.BackColor = Color.FromArgb(210, 246, 231);
        _ordersSplit.Dock = DockStyle.Fill;
        _ordersSplit.FixedPanel = FixedPanel.Panel2;
        _ordersSplit.Location = new Point(6, 56);
        _ordersSplit.Name = "_ordersSplit";
        _ordersSplit.Orientation = Orientation.Vertical;
        _ordersSplit.Panel1.BackColor = Color.White;
        _ordersSplit.Panel1.Controls.Add(_ordersGrid);
        _ordersSplit.Panel2.BackColor = Color.White;
        _ordersSplit.Panel2.Controls.Add(_orderDetailsPanel);
        _ordersSplit.Size = new Size(1260, 618);
        _ordersSplit.SplitterDistance = 800;
        _ordersSplit.SplitterWidth = 6;
        _ordersSplit.TabIndex = 1;
        //
        // _ordersGrid
        //
        _ordersGrid.AllowUserToAddRows = false;
        _ordersGrid.AllowUserToDeleteRows = false;
        _ordersGrid.AutoGenerateColumns = false;
        _ordersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _ordersGrid.Columns.AddRange(new DataGridViewColumn[]
        {
            _orderIdColumn, _orderDateColumn, _orderCustomerColumn,
            _orderCreatorColumn, _orderPositionsColumn, _orderTotalColumn
        });
        _ordersGrid.Dock = DockStyle.Fill;
        _ordersGrid.Location = new Point(0, 0);
        _ordersGrid.MultiSelect = false;
        _ordersGrid.Name = "_ordersGrid";
        _ordersGrid.ReadOnly = true;
        _ordersGrid.RowHeadersVisible = false;
        _ordersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _ordersGrid.Size = new Size(800, 618);
        _ordersGrid.TabIndex = 0;
        _ordersGrid.SelectionChanged += OrdersGrid_SelectionChanged;
        //
        // _orderIdColumn
        //
        _orderIdColumn.FillWeight = 45F;
        _orderIdColumn.HeaderText = "№";
        _orderIdColumn.Name = "_orderIdColumn";
        _orderIdColumn.ReadOnly = true;
        //
        // _orderDateColumn
        //
        _orderDateColumn.FillWeight = 85F;
        _orderDateColumn.HeaderText = "Дата";
        _orderDateColumn.Name = "_orderDateColumn";
        _orderDateColumn.ReadOnly = true;
        //
        // _orderCustomerColumn
        //
        _orderCustomerColumn.FillWeight = 190F;
        _orderCustomerColumn.HeaderText = "Клиент";
        _orderCustomerColumn.Name = "_orderCustomerColumn";
        _orderCustomerColumn.ReadOnly = true;
        //
        // _orderCreatorColumn
        //
        _orderCreatorColumn.FillWeight = 150F;
        _orderCreatorColumn.HeaderText = "Оформил";
        _orderCreatorColumn.Name = "_orderCreatorColumn";
        _orderCreatorColumn.ReadOnly = true;
        //
        // _orderPositionsColumn
        //
        _orderPositionsColumn.FillWeight = 65F;
        _orderPositionsColumn.HeaderText = "Позиций";
        _orderPositionsColumn.Name = "_orderPositionsColumn";
        _orderPositionsColumn.ReadOnly = true;
        DataGridViewCellStyle orderPositionsStyle = new DataGridViewCellStyle();
        orderPositionsStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _orderPositionsColumn.DefaultCellStyle = orderPositionsStyle;
        //
        // _orderTotalColumn
        //
        _orderTotalColumn.FillWeight = 120F;
        _orderTotalColumn.HeaderText = "Сумма";
        _orderTotalColumn.MinimumWidth = 110;
        _orderTotalColumn.Name = "_orderTotalColumn";
        _orderTotalColumn.ReadOnly = true;
        DataGridViewCellStyle orderTotalStyle = new DataGridViewCellStyle();
        orderTotalStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _orderTotalColumn.DefaultCellStyle = orderTotalStyle;
        //
        // _orderDetailsPanel
        //
        _orderDetailsPanel.BackColor = Color.White;
        _orderDetailsPanel.Controls.Add(_orderItemsGrid);
        _orderDetailsPanel.Controls.Add(_orderHeaderPanel);
        _orderDetailsPanel.Dock = DockStyle.Fill;
        _orderDetailsPanel.Location = new Point(0, 0);
        _orderDetailsPanel.Name = "_orderDetailsPanel";
        _orderDetailsPanel.Padding = new Padding(8);
        _orderDetailsPanel.Size = new Size(454, 618);
        _orderDetailsPanel.TabIndex = 0;
        //
        // _orderHeaderPanel
        //
        _orderHeaderPanel.BackColor = Color.FromArgb(210, 246, 231);
        _orderHeaderPanel.Controls.Add(_orderHeaderLabel);
        _orderHeaderPanel.Controls.Add(_orderTotalLabel);
        _orderHeaderPanel.Dock = DockStyle.Top;
        _orderHeaderPanel.Location = new Point(8, 8);
        _orderHeaderPanel.Name = "_orderHeaderPanel";
        _orderHeaderPanel.Padding = new Padding(8);
        _orderHeaderPanel.Size = new Size(438, 100);
        _orderHeaderPanel.TabIndex = 0;
        //
        // _orderHeaderLabel
        //
        _orderHeaderLabel.Dock = DockStyle.Fill;
        _orderHeaderLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _orderHeaderLabel.Location = new Point(8, 8);
        _orderHeaderLabel.Name = "_orderHeaderLabel";
        _orderHeaderLabel.Size = new Size(422, 58);
        _orderHeaderLabel.TabIndex = 0;
        _orderHeaderLabel.TextAlign = ContentAlignment.TopLeft;
        //
        // _orderTotalLabel
        //
        _orderTotalLabel.Dock = DockStyle.Bottom;
        _orderTotalLabel.Font = new Font("Calibri", 12F, FontStyle.Bold);
        _orderTotalLabel.ForeColor = Color.FromArgb(79, 145, 142);
        _orderTotalLabel.Location = new Point(8, 66);
        _orderTotalLabel.Name = "_orderTotalLabel";
        _orderTotalLabel.Size = new Size(422, 26);
        _orderTotalLabel.TabIndex = 1;
        _orderTotalLabel.TextAlign = ContentAlignment.MiddleRight;
        //
        // _orderItemsGrid
        //
        _orderItemsGrid.AllowUserToAddRows = false;
        _orderItemsGrid.AllowUserToDeleteRows = false;
        _orderItemsGrid.AutoGenerateColumns = false;
        _orderItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _orderItemsGrid.Columns.AddRange(new DataGridViewColumn[]
        {
            _orderItemProductColumn, _orderItemSizeColumn, _orderItemQuantityColumn,
            _orderItemPriceColumn, _orderItemSumColumn
        });
        _orderItemsGrid.Dock = DockStyle.Fill;
        _orderItemsGrid.Location = new Point(8, 108);
        _orderItemsGrid.MultiSelect = false;
        _orderItemsGrid.Name = "_orderItemsGrid";
        _orderItemsGrid.ReadOnly = true;
        _orderItemsGrid.RowHeadersVisible = false;
        _orderItemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _orderItemsGrid.Size = new Size(438, 502);
        _orderItemsGrid.TabIndex = 1;
        //
        // _orderItemProductColumn
        //
        _orderItemProductColumn.FillWeight = 260F;
        _orderItemProductColumn.HeaderText = "Товар";
        _orderItemProductColumn.Name = "_orderItemProductColumn";
        _orderItemProductColumn.ReadOnly = true;
        //
        // _orderItemSizeColumn
        //
        _orderItemSizeColumn.FillWeight = 60F;
        _orderItemSizeColumn.HeaderText = "Размер";
        _orderItemSizeColumn.Name = "_orderItemSizeColumn";
        _orderItemSizeColumn.ReadOnly = true;
        //
        // _orderItemQuantityColumn
        //
        _orderItemQuantityColumn.FillWeight = 60F;
        _orderItemQuantityColumn.HeaderText = "Кол-во";
        _orderItemQuantityColumn.Name = "_orderItemQuantityColumn";
        _orderItemQuantityColumn.ReadOnly = true;
        DataGridViewCellStyle orderItemQuantityStyle = new DataGridViewCellStyle();
        orderItemQuantityStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _orderItemQuantityColumn.DefaultCellStyle = orderItemQuantityStyle;
        //
        // _orderItemPriceColumn
        //
        _orderItemPriceColumn.FillWeight = 90F;
        _orderItemPriceColumn.HeaderText = "Цена";
        _orderItemPriceColumn.Name = "_orderItemPriceColumn";
        _orderItemPriceColumn.ReadOnly = true;
        DataGridViewCellStyle orderItemPriceStyle = new DataGridViewCellStyle();
        orderItemPriceStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _orderItemPriceColumn.DefaultCellStyle = orderItemPriceStyle;
        //
        // _orderItemSumColumn
        //
        _orderItemSumColumn.FillWeight = 100F;
        _orderItemSumColumn.HeaderText = "Сумма";
        _orderItemSumColumn.Name = "_orderItemSumColumn";
        _orderItemSumColumn.ReadOnly = true;
        DataGridViewCellStyle orderItemSumStyle = new DataGridViewCellStyle();
        orderItemSumStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _orderItemSumColumn.DefaultCellStyle = orderItemSumStyle;
        //
        // _statusStrip
        //
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _databaseStatusLabel });
        _statusStrip.Dock = DockStyle.Bottom;
        _statusStrip.Location = new Point(0, 778);
        _statusStrip.Name = "_statusStrip";
        _statusStrip.SizingGrip = false;
        _statusStrip.Size = new Size(1280, 22);
        _statusStrip.TabIndex = 0;
        //
        // _statusLabel
        //
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(1000, 17);
        _statusLabel.Spring = true;
        _statusLabel.Text = "Готово";
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // _databaseStatusLabel
        //
        _databaseStatusLabel.ForeColor = Color.DimGray;
        _databaseStatusLabel.Name = "_databaseStatusLabel";
        _databaseStatusLabel.Size = new Size(180, 17);
        _databaseStatusLabel.Text = "БД";
        _databaseStatusLabel.TextAlign = ContentAlignment.MiddleRight;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1280, 800);
        Controls.Add(_tabs);
        Controls.Add(_statusStrip);
        Controls.Add(_headerPanel);
        Font = new Font("Calibri", 10F);
        MinimumSize = new Size(1100, 700);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Чудо Обувь — оформление заказов";
        FormClosing += MainForm_FormClosing;
        Shown += MainForm_Shown;

        _headerPanel.ResumeLayout(false);
        _headerPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_logoPictureBox).EndInit();
        _tabs.ResumeLayout(false);
        _catalogTab.ResumeLayout(false);
        _cartTab.ResumeLayout(false);
        _ordersTab.ResumeLayout(false);
        _filterFlow.ResumeLayout(false);
        _filterPanel.ResumeLayout(false);
        _filterPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_catalogSplit).EndInit();
        _catalogSplit.Panel1.ResumeLayout(false);
        _catalogSplit.Panel2.ResumeLayout(false);
        _catalogSplit.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_productsGrid).EndInit();
        _detailsFlow.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_productPicture).EndInit();
        _detailsBottom.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_quantityNumeric).EndInit();
        _cartTab.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_cartGrid).EndInit();
        _cartBottomPanel.ResumeLayout(false);
        _ordersTopPanel.ResumeLayout(false);
        _ordersTopFlow.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_ordersSplit).EndInit();
        _ordersSplit.Panel1.ResumeLayout(false);
        _ordersSplit.Panel2.ResumeLayout(false);
        _ordersSplit.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_ordersGrid).EndInit();
        _orderDetailsPanel.ResumeLayout(false);
        _orderHeaderPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_orderItemsGrid).EndInit();
        _statusStrip.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
