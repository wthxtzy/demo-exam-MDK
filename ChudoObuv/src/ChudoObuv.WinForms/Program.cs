using ChudoObuv.Core;

namespace ChudoObuv.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            var database = new Database(Database.ResolveDefaultPath());
            database.EnsureCreated();

            using var loginForm = new LoginForm(database);
            if (loginForm.ShowDialog() != DialogResult.OK || loginForm.Session is null)
            {
                return;
            }

            Application.Run(new MainForm(database, loginForm.Session));
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось запустить приложение.\n\n" + ex.Message,
                "Чудо Обувь",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
