using ChudoObuv.Core;

namespace ChudoObuv.WinForms;

/// <summary>Вход по логину. Пароль по заданию не требуется.</summary>
public sealed class LoginForm : Form
{
    private readonly Database _database;
    private readonly AuthService _authService;
    private readonly ComboBox _loginComboBox;
    private readonly Label _userInfoLabel;
    private readonly Button _loginButton;

    public UserSession? Session { get; private set; }

    public LoginForm(Database database)
    {
        _database = database;
        _authService = new AuthService(database);

        Text = "Чудо Обувь — вход в систему";
        ClientSize = new Size(480, 450);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        UiTheme.ApplyForm(this);

        var logo = new PictureBox
        {
            Location = new Point(180, 18),
            Size = new Size(120, 120),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = ImageResources.Logo,
            BackColor = Color.White
        };

        var titleLabel = new Label
        {
            Text = "Чудо Обувь",
            Font = new Font("Calibri", 20F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(0, 148),
            Size = new Size(480, 38)
        };

        var subtitleLabel = new Label
        {
            Text = "Система оформления заказов",
            Font = UiTheme.SubtitleFont,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.DimGray,
            Location = new Point(0, 184),
            Size = new Size(480, 28)
        };

        var loginLabel = new Label
        {
            Text = "Логин:",
            Location = new Point(50, 230),
            AutoSize = true,
            Font = UiTheme.BoldFont
        };

        _loginComboBox = new ComboBox
        {
            Location = new Point(50, 253),
            Width = 380,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = UiTheme.BaseFont
        };
        _loginComboBox.SelectedIndexChanged += LoginComboBox_SelectedIndexChanged;

        _userInfoLabel = new Label
        {
            Location = new Point(50, 292),
            Size = new Size(380, 42),
            ForeColor = Color.DimGray,
            Font = UiTheme.BaseFont
        };

        _loginButton = UiTheme.CreateButton("Войти", accent: true, width: 180);
        _loginButton.Location = new Point(50, 348);
        _loginButton.Click += LoginButton_Click;

        var guestButton = UiTheme.CreateButton("Продолжить как гость", width: 180);
        guestButton.Location = new Point(250, 348);
        guestButton.Click += GuestButton_Click;

        var hintLabel = new Label
        {
            Text = "Пароль не требуется — выберите логин из списка",
            Location = new Point(0, 405),
            Size = new Size(480, 24),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.Gray,
            Font = UiTheme.SmallFont
        };

        Controls.AddRange(new Control[]
        {
            logo, titleLabel, subtitleLabel, loginLabel, _loginComboBox,
            _userInfoLabel, _loginButton, guestButton, hintLabel
        });

        AcceptButton = _loginButton;
        Load += LoginForm_Load;
    }

    private void LoginForm_Load(object? sender, EventArgs e)
    {
        try
        {
            var options = _authService.GetLoginOptions();
            _loginComboBox.DataSource = options;
            _loginComboBox.DisplayMember = nameof(LoginOption.Login);

            if (options.Count == 0)
            {
                _loginButton.Enabled = false;
                _userInfoLabel.Text = "В базе нет пользователей.";
            }
            else
            {
                _loginComboBox.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось загрузить список пользователей.\n\n" + ex.Message,
                "Чудо Обувь",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            _loginButton.Enabled = false;
        }
    }

    private void LoginComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loginComboBox.SelectedItem is LoginOption option)
        {
            _userInfoLabel.Text = $"{option.FullName}\n{option.RoleName}";
        }
    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        if (_loginComboBox.SelectedItem is not LoginOption option)
        {
            MessageBox.Show("Выберите логин.", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var session = _authService.Authenticate(option.Login);
        if (session is null)
        {
            MessageBox.Show("Пользователь не найден.", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Session = session;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void GuestButton_Click(object? sender, EventArgs e)
    {
        Session = new UserSession(null, string.Empty, "Гость", UserRole.Guest);
        DialogResult = DialogResult.OK;
        Close();
    }
}
