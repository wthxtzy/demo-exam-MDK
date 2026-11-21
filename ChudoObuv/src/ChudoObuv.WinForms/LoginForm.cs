using ChudoObuv.Core;

namespace ChudoObuv.WinForms;

/// <summary>Вход по логину. Пароль по заданию не требуется.</summary>
public partial class LoginForm : Form
{
    private AuthService? _authService;

    public UserSession? Session { get; private set; }

    public LoginForm()
    {
        InitializeComponent();
        ApplyTheme();
    }

    public LoginForm(Database database) : this()
    {
        _authService = new AuthService(database);
    }

    private void ApplyTheme()
    {
        UiTheme.ApplyForm(this);
        logoPictureBox.Image = ImageResources.Logo;
    }

    private void LoginForm_Load(object? sender, EventArgs e)
    {
        if (_authService is null)
        {
            return;
        }

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
        if (_authService is null)
        {
            return;
        }

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
