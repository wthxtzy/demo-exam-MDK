#nullable enable

using System.Drawing;
using System.Windows.Forms;

namespace ChudoObuv.WinForms;

partial class LoginForm
{
    private System.ComponentModel.IContainer? components;
    private PictureBox logoPictureBox = null!;
    private Label titleLabel = null!;
    private Label subtitleLabel = null!;
    private Label loginLabel = null!;
    private ComboBox _loginComboBox = null!;
    private Label _userInfoLabel = null!;
    private Button _loginButton = null!;
    private Button guestButton = null!;
    private Label hintLabel = null!;

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
        logoPictureBox = new PictureBox();
        titleLabel = new Label();
        subtitleLabel = new Label();
        loginLabel = new Label();
        _loginComboBox = new ComboBox();
        _userInfoLabel = new Label();
        _loginButton = new Button();
        guestButton = new Button();
        hintLabel = new Label();
        SuspendLayout();
        //
        // logoPictureBox
        //
        logoPictureBox.BackColor = Color.White;
        logoPictureBox.Location = new Point(180, 18);
        logoPictureBox.Name = "logoPictureBox";
        logoPictureBox.Size = new Size(120, 120);
        logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        logoPictureBox.TabIndex = 0;
        logoPictureBox.TabStop = false;
        //
        // titleLabel
        //
        titleLabel.Font = new Font("Calibri", 20F, FontStyle.Bold);
        titleLabel.Location = new Point(0, 148);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(480, 38);
        titleLabel.TabIndex = 1;
        titleLabel.Text = "Чудо Обувь";
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // subtitleLabel
        //
        subtitleLabel.Font = new Font("Calibri", 12F, FontStyle.Bold);
        subtitleLabel.ForeColor = Color.DimGray;
        subtitleLabel.Location = new Point(0, 184);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Size = new Size(480, 28);
        subtitleLabel.TabIndex = 2;
        subtitleLabel.Text = "Система оформления заказов";
        subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // loginLabel
        //
        loginLabel.AutoSize = true;
        loginLabel.Font = new Font("Calibri", 10F, FontStyle.Bold);
        loginLabel.Location = new Point(50, 230);
        loginLabel.Name = "loginLabel";
        loginLabel.TabIndex = 3;
        loginLabel.Text = "Логин:";
        //
        // _loginComboBox
        //
        _loginComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _loginComboBox.Font = new Font("Calibri", 10F);
        _loginComboBox.Location = new Point(50, 253);
        _loginComboBox.Name = "_loginComboBox";
        _loginComboBox.Size = new Size(380, 23);
        _loginComboBox.TabIndex = 4;
        _loginComboBox.SelectedIndexChanged += LoginComboBox_SelectedIndexChanged;
        //
        // _userInfoLabel
        //
        _userInfoLabel.Font = new Font("Calibri", 10F);
        _userInfoLabel.ForeColor = Color.DimGray;
        _userInfoLabel.Location = new Point(50, 292);
        _userInfoLabel.Name = "_userInfoLabel";
        _userInfoLabel.Size = new Size(380, 42);
        _userInfoLabel.TabIndex = 5;
        //
        // _loginButton
        //
        _loginButton.BackColor = Color.FromArgb(112, 178, 175);
        _loginButton.Cursor = Cursors.Hand;
        _loginButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        _loginButton.FlatAppearance.BorderSize = 1;
        _loginButton.FlatStyle = FlatStyle.Flat;
        _loginButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        _loginButton.ForeColor = Color.White;
        _loginButton.Location = new Point(50, 348);
        _loginButton.Name = "_loginButton";
        _loginButton.Size = new Size(180, 34);
        _loginButton.TabIndex = 6;
        _loginButton.Text = "Войти";
        _loginButton.UseVisualStyleBackColor = false;
        _loginButton.Click += LoginButton_Click;
        //
        // guestButton
        //
        guestButton.BackColor = Color.FromArgb(210, 246, 231);
        guestButton.Cursor = Cursors.Hand;
        guestButton.FlatAppearance.BorderColor = Color.FromArgb(112, 178, 175);
        guestButton.FlatAppearance.BorderSize = 1;
        guestButton.FlatStyle = FlatStyle.Flat;
        guestButton.Font = new Font("Calibri", 10F, FontStyle.Bold);
        guestButton.ForeColor = Color.Black;
        guestButton.Location = new Point(250, 348);
        guestButton.Name = "guestButton";
        guestButton.Size = new Size(180, 34);
        guestButton.TabIndex = 7;
        guestButton.Text = "Продолжить как гость";
        guestButton.UseVisualStyleBackColor = false;
        guestButton.Click += GuestButton_Click;
        //
        // hintLabel
        //
        hintLabel.Font = new Font("Calibri", 9F);
        hintLabel.ForeColor = Color.Gray;
        hintLabel.Location = new Point(0, 405);
        hintLabel.Name = "hintLabel";
        hintLabel.Size = new Size(480, 24);
        hintLabel.TabIndex = 8;
        hintLabel.Text = "Пароль не требуется — выберите логин из списка";
        hintLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // LoginForm
        //
        AcceptButton = _loginButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(480, 450);
        Controls.Add(hintLabel);
        Controls.Add(guestButton);
        Controls.Add(_loginButton);
        Controls.Add(_userInfoLabel);
        Controls.Add(_loginComboBox);
        Controls.Add(loginLabel);
        Controls.Add(subtitleLabel);
        Controls.Add(titleLabel);
        Controls.Add(logoPictureBox);
        Font = new Font("Calibri", 10F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Чудо Обувь — вход в систему";
        Load += LoginForm_Load;
        ResumeLayout(false);
        PerformLayout();
    }
}
