#nullable enable

using System.Drawing;
using System.Windows.Forms;

namespace ChudoObuv.WinForms;

partial class OrderEditForm
{
    private System.ComponentModel.IContainer? components;
    private Panel _topPanel = null!;
    private Panel _addPanel = null!;
    private Panel _bottomPanel = null!;
    private Label _titleLabel = null!;
    private Label _dateLabel = null!;
    private Label _customerLabel = null!;
    private Label _productLabel = null!;
    private Label _sizeLabel = null!;
    private Label _quantityLabel = null!;
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
    private Button _cancelButton = null!;
    private DataGridViewTextBoxColumn _columnProduct = null!;
    private DataGridViewTextBoxColumn _columnSize = null!;
    private DataGridViewTextBoxColumn _columnPrice = null!;
    private DataGridViewTextBoxColumn _columnQuantity = null!;
    private DataGridViewTextBoxColumn _columnSum = null!;

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
        _topPanel = new Panel();
        _titleLabel = new Label();
        _dateLabel = new Label();
        _datePicker = new DateTimePicker();
        _customerLabel = new Label();
        _customerTextBox = new TextBox();
        _addPanel = new Panel();
        _productLabel = new Label();
        _productComboBox = new ComboBox();
        _sizeLabel = new Label();
        _sizeComboBox = new ComboBox();
        _quantityLabel = new Label();
        _quantityNumeric = new NumericUpDown();
        _addButton = new Button();
        _itemsGrid = new DataGridView();
        _columnProduct = new DataGridViewTextBoxColumn();
        _columnSize = new DataGridViewTextBoxColumn();
        _columnPrice = new DataGridViewTextBoxColumn();
        _columnQuantity = new DataGridViewTextBoxColumn();
        _columnSum = new DataGridViewTextBoxColumn();
        _bottomPanel = new Panel();
        _totalLabel = new Label();
        _removeButton = new Button();
        _clearButton = new Button();
        _saveButton = new Button();
        _cancelButton = new Button();
        ((System.ComponentModel.ISupportInitialize)_quantityNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_itemsGrid).BeginInit();
        _topPanel.SuspendLayout();
        _addPanel.SuspendLayout();
        _bottomPanel.SuspendLayout();
        SuspendLayout();
        //
        // _topPanel
        //
        _topPanel.BackColor = Color.FromArgb(210, 246, 231);
        _topPanel.Controls.Add(_customerTextBox);
        _topPanel.Controls.Add(_customerLabel);
        _topPanel.Controls.Add(_datePicker);
        _topPanel.Controls.Add(_dateLabel);
        _topPanel.Controls.Add(_titleLabel);
        _topPanel.Dock = DockStyle.Top;
        _topPanel.Location = new Point(0, 0);
        _topPanel.Name = "_topPanel";
        _topPanel.Padding = new Padding(10);
        _topPanel.Size = new Size(1000, 104);
        _topPanel.TabIndex = 0;
        //
        // _titleLabel
        //
        _titleLabel.AutoSize = true;
        _titleLabel.Font = new Font("Calibri", 12F, FontStyle.Bold);
        _titleLabel.Location = new Point(10, 8);
        _titleLabel.Name = "_titleLabel";
        _titleLabel.TabIndex = 0;
        _titleLabel.Text = "Оформление заказа";
        //
        // _dateLabel
        //
        _dateLabel.AutoSize = true;
        _dateLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _dateLabel.Location = new Point(10, 56);
        _dateLabel.Name = "_dateLabel";
        _dateLabel.TabIndex = 1;
        _dateLabel.Text = "Дата заказа:";
        //
        // _datePicker
        //
        _datePicker.CustomFormat = "dd.MM.yyyy";
        _datePicker.Font = new Font("Calibri", 10F);
        _datePicker.Format = DateTimePickerFormat.Custom;
        _datePicker.Location = new Point(115, 52);
        _datePicker.Name = "_datePicker";
        _datePicker.Size = new Size(140, 25);
        _datePicker.TabIndex = 2;
        //
        // _customerLabel
        //
        _customerLabel.AutoSize = true;
        _customerLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _customerLabel.Location = new Point(280, 56);
        _customerLabel.Name = "_customerLabel";
        _customerLabel.TabIndex = 3;
        _customerLabel.Text = "ФИО клиента:";
        //
        // _customerTextBox
        //
        _customerTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        _customerTextBox.BorderStyle = BorderStyle.FixedSingle;
        _customerTextBox.Font = new Font("Calibri", 10F);
        _customerTextBox.Location = new Point(385, 50);
        _customerTextBox.Name = "_customerTextBox";
        _customerTextBox.Size = new Size(560, 26);
        _customerTextBox.TabIndex = 4;
        //
        // _addPanel
        //
        _addPanel.BackColor = Color.White;
        _addPanel.Controls.Add(_addButton);
        _addPanel.Controls.Add(_quantityNumeric);
        _addPanel.Controls.Add(_quantityLabel);
        _addPanel.Controls.Add(_sizeComboBox);
        _addPanel.Controls.Add(_sizeLabel);
        _addPanel.Controls.Add(_productComboBox);
        _addPanel.Controls.Add(_productLabel);
        _addPanel.Dock = DockStyle.Top;
        _addPanel.Location = new Point(0, 104);
        _addPanel.Name = "_addPanel";
        _addPanel.Padding = new Padding(10);
        _addPanel.Size = new Size(1000, 84);
        _addPanel.TabIndex = 1;
        //
        // _productLabel
        //
        _productLabel.AutoSize = true;
        _productLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _productLabel.Location = new Point(10, 4);
        _productLabel.Name = "_productLabel";
        _productLabel.TabIndex = 0;
        _productLabel.Text = "Товар:";
        //
        // _productComboBox
        //
        _productComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _productComboBox.Font = new Font("Calibri", 10F);
        _productComboBox.Location = new Point(10, 26);
        _productComboBox.Name = "_productComboBox";
        _productComboBox.Size = new Size(500, 23);
        _productComboBox.TabIndex = 1;
        _productComboBox.SelectedIndexChanged += ProductComboBox_SelectedIndexChanged;
        //
        // _sizeLabel
        //
        _sizeLabel.AutoSize = true;
        _sizeLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _sizeLabel.Location = new Point(525, 4);
        _sizeLabel.Name = "_sizeLabel";
        _sizeLabel.TabIndex = 2;
        _sizeLabel.Text = "Размер:";
        //
        // _sizeComboBox
        //
        _sizeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _sizeComboBox.Font = new Font("Calibri", 10F);
        _sizeComboBox.Location = new Point(525, 26);
        _sizeComboBox.Name = "_sizeComboBox";
        _sizeComboBox.Size = new Size(140, 23);
        _sizeComboBox.TabIndex = 3;
        _sizeComboBox.SelectedIndexChanged += SizeComboBox_SelectedIndexChanged;
        //
        // _quantityLabel
        //
        _quantityLabel.AutoSize = true;
        _quantityLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _quantityLabel.Location = new Point(680, 4);
        _quantityLabel.Name = "_quantityLabel";
        _quantityLabel.TabIndex = 4;
        _quantityLabel.Text = "Кол-во:";
        //
        // _quantityNumeric
        //
        _quantityNumeric.Font = new Font("Calibri", 10F);
        _quantityNumeric.Location = new Point(680, 26);
        _quantityNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
        _quantityNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        _quantityNumeric.Name = "_quantityNumeric";
        _quantityNumeric.Size = new Size(90, 25);
        _quantityNumeric.TabIndex = 5;
        _quantityNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // _addButton
        //
        _addButton.BackColor = Color.FromArgb(112, 178, 175);
        _addButton.Cursor = Cursors.Hand;
        _addButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _addButton.FlatAppearance.BorderSize = 1;
        _addButton.FlatStyle = FlatStyle.Flat;
        _addButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _addButton.ForeColor = Color.White;
        _addButton.Location = new Point(790, 25);
        _addButton.Name = "_addButton";
        _addButton.Size = new Size(130, 34);
        _addButton.TabIndex = 6;
        _addButton.Text = "Добавить";
        _addButton.UseVisualStyleBackColor = false;
        _addButton.Click += AddButton_Click;
        //
        // _itemsGrid
        //
        _itemsGrid.AllowUserToAddRows = false;
        _itemsGrid.AllowUserToDeleteRows = false;
        _itemsGrid.AutoGenerateColumns = false;
        _itemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _itemsGrid.BackgroundColor = Color.White;
        _itemsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        _itemsGrid.Columns.AddRange(new DataGridViewColumn[] { _columnProduct, _columnSize, _columnPrice, _columnQuantity, _columnSum });
        _itemsGrid.Dock = DockStyle.Fill;
        _itemsGrid.Location = new Point(0, 188);
        _itemsGrid.MultiSelect = false;
        _itemsGrid.Name = "_itemsGrid";
        _itemsGrid.ReadOnly = true;
        _itemsGrid.RowHeadersVisible = false;
        _itemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _itemsGrid.Size = new Size(1000, 386);
        _itemsGrid.TabIndex = 2;
        //
        // _columnProduct
        //
        _columnProduct.FillWeight = 300F;
        _columnProduct.HeaderText = "Товар";
        _columnProduct.Name = "_columnProduct";
        _columnProduct.ReadOnly = true;
        //
        // _columnSize
        //
        _columnSize.FillWeight = 70F;
        _columnSize.HeaderText = "Размер";
        _columnSize.Name = "_columnSize";
        _columnSize.ReadOnly = true;
        //
        // _columnPrice
        //
        _columnPrice.FillWeight = 100F;
        _columnPrice.HeaderText = "Цена";
        _columnPrice.Name = "_columnPrice";
        _columnPrice.ReadOnly = true;
        DataGridViewCellStyle priceCellStyle = new DataGridViewCellStyle();
        priceCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _columnPrice.DefaultCellStyle = priceCellStyle;
        //
        // _columnQuantity
        //
        _columnQuantity.FillWeight = 70F;
        _columnQuantity.HeaderText = "Кол-во";
        _columnQuantity.Name = "_columnQuantity";
        _columnQuantity.ReadOnly = true;
        DataGridViewCellStyle quantityCellStyle = new DataGridViewCellStyle();
        quantityCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _columnQuantity.DefaultCellStyle = quantityCellStyle;
        //
        // _columnSum
        //
        _columnSum.FillWeight = 110F;
        _columnSum.HeaderText = "Сумма";
        _columnSum.Name = "_columnSum";
        _columnSum.ReadOnly = true;
        DataGridViewCellStyle sumCellStyle = new DataGridViewCellStyle();
        sumCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _columnSum.DefaultCellStyle = sumCellStyle;
        //
        // _bottomPanel
        //
        _bottomPanel.BackColor = Color.FromArgb(210, 246, 231);
        _bottomPanel.Controls.Add(_cancelButton);
        _bottomPanel.Controls.Add(_saveButton);
        _bottomPanel.Controls.Add(_clearButton);
        _bottomPanel.Controls.Add(_removeButton);
        _bottomPanel.Controls.Add(_totalLabel);
        _bottomPanel.Dock = DockStyle.Bottom;
        _bottomPanel.Location = new Point(0, 574);
        _bottomPanel.Name = "_bottomPanel";
        _bottomPanel.Padding = new Padding(10);
        _bottomPanel.Size = new Size(1000, 66);
        _bottomPanel.TabIndex = 3;
        _bottomPanel.SizeChanged += BottomPanel_SizeChanged;
        //
        // _totalLabel
        //
        _totalLabel.AutoSize = true;
        _totalLabel.Font = new Font("Calibri", 13F, FontStyle.Bold);
        _totalLabel.Location = new Point(10, 18);
        _totalLabel.Name = "_totalLabel";
        _totalLabel.TabIndex = 0;
        _totalLabel.Text = "Итого: 0,00 руб.";
        //
        // _removeButton
        //
        _removeButton.BackColor = Color.FromArgb(210, 246, 231);
        _removeButton.Cursor = Cursors.Hand;
        _removeButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _removeButton.FlatAppearance.BorderSize = 1;
        _removeButton.FlatStyle = FlatStyle.Flat;
        _removeButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _removeButton.ForeColor = Color.Black;
        _removeButton.Location = new Point(360, 15);
        _removeButton.Name = "_removeButton";
        _removeButton.Size = new Size(160, 34);
        _removeButton.TabIndex = 1;
        _removeButton.Text = "Удалить позицию";
        _removeButton.UseVisualStyleBackColor = false;
        _removeButton.Click += RemoveButton_Click;
        //
        // _clearButton
        //
        _clearButton.BackColor = Color.FromArgb(210, 246, 231);
        _clearButton.Cursor = Cursors.Hand;
        _clearButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _clearButton.FlatAppearance.BorderSize = 1;
        _clearButton.FlatStyle = FlatStyle.Flat;
        _clearButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _clearButton.ForeColor = Color.Black;
        _clearButton.Location = new Point(530, 15);
        _clearButton.Name = "_clearButton";
        _clearButton.Size = new Size(110, 34);
        _clearButton.TabIndex = 2;
        _clearButton.Text = "Очистить";
        _clearButton.UseVisualStyleBackColor = false;
        _clearButton.Click += ClearButton_Click;
        //
        // _saveButton
        //
        _saveButton.BackColor = Color.FromArgb(112, 178, 175);
        _saveButton.Cursor = Cursors.Hand;
        _saveButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _saveButton.FlatAppearance.BorderSize = 1;
        _saveButton.FlatStyle = FlatStyle.Flat;
        _saveButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _saveButton.ForeColor = Color.White;
        _saveButton.Location = new Point(690, 15);
        _saveButton.Name = "_saveButton";
        _saveButton.Size = new Size(160, 34);
        _saveButton.TabIndex = 3;
        _saveButton.Text = "Сохранить";
        _saveButton.UseVisualStyleBackColor = false;
        _saveButton.Click += SaveButton_Click;
        //
        // _cancelButton
        //
        _cancelButton.BackColor = Color.FromArgb(210, 246, 231);
        _cancelButton.Cursor = Cursors.Hand;
        _cancelButton.DialogResult = DialogResult.Cancel;
        _cancelButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _cancelButton.FlatAppearance.BorderSize = 1;
        _cancelButton.FlatStyle = FlatStyle.Flat;
        _cancelButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _cancelButton.ForeColor = Color.Black;
        _cancelButton.Location = new Point(860, 15);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new Size(110, 34);
        _cancelButton.TabIndex = 4;
        _cancelButton.Text = "Отмена";
        _cancelButton.UseVisualStyleBackColor = false;
        //
        // OrderEditForm
        //
        AcceptButton = _saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        CancelButton = _cancelButton;
        ClientSize = new Size(1000, 640);
        Controls.Add(_itemsGrid);
        Controls.Add(_bottomPanel);
        Controls.Add(_addPanel);
        Controls.Add(_topPanel);
        Font = new Font("Calibri", 10F);
        MinimumSize = new Size(900, 560);
        Name = "OrderEditForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Чудо Обувь — оформление заказа";
        ((System.ComponentModel.ISupportInitialize)_quantityNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)_itemsGrid).EndInit();
        _topPanel.ResumeLayout(false);
        _topPanel.PerformLayout();
        _addPanel.ResumeLayout(false);
        _addPanel.PerformLayout();
        _bottomPanel.ResumeLayout(false);
        _bottomPanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
