using System.Reflection;
using Microsoft.Data.Sqlite;

namespace ChudoObuv.Core;

/// <summary>
/// Подключение к SQLite-базе. При первом запуске база создаётся
/// из встроенных скриптов schema.sql и seed.sql.
/// </summary>
public sealed class Database
{
    public string DatabasePath { get; }

    public Database(string databasePath)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
        {
            throw new ArgumentException("Путь к базе данных не задан.", nameof(databasePath));
        }

        DatabasePath = Path.GetFullPath(databasePath);
    }

    /// <summary>
    /// База лежит рядом с exe. Если папка недоступна для записи,
    /// используется %LOCALAPPDATA%\ChudoObuv.
    /// </summary>
    public static string ResolveDefaultPath()
    {
        var localPath = Path.Combine(AppContext.BaseDirectory, "chudo_obuv.db");
        try
        {
            var directory = Path.GetDirectoryName(localPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var probe = new FileStream(
                localPath,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
            return localPath;
        }
        catch (Exception)
        {
            var fallbackDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ChudoObuv");
            Directory.CreateDirectory(fallbackDirectory);
            return Path.Combine(fallbackDirectory, "chudo_obuv.db");
        }
    }

    public SqliteConnection Open()
    {
        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = DatabasePath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Cache = SqliteCacheMode.Shared,
            ForeignKeys = true,
            DefaultTimeout = 10
        };

        var connection = new SqliteConnection(builder.ToString());
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();

        return connection;
    }

    public bool IsInitialized()
    {
        if (!File.Exists(DatabasePath))
        {
            return false;
        }

        try
        {
            using var connection = Open();
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'Products';";
            return Convert.ToInt64(command.ExecuteScalar()) > 0;
        }
        catch (SqliteException)
        {
            return false;
        }
    }

    /// <summary>Создаёт схему и загружает тестовые данные, если база ещё не создана.</summary>
    public void EnsureCreated()
    {
        var directory = Path.GetDirectoryName(DatabasePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (IsInitialized())
        {
            return;
        }

        if (File.Exists(DatabasePath))
        {
            File.Delete(DatabasePath);
        }

        try
        {
            using var connection = Open();
            ExecuteEmbeddedScript(connection, "ChudoObuv.Core.schema.sql");
            ExecuteEmbeddedScript(connection, "ChudoObuv.Core.seed.sql");
        }
        catch
        {
            TryDeleteBrokenDatabase();
            throw;
        }
    }

    private static void ExecuteEmbeddedScript(SqliteConnection connection, string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Встроенный ресурс «{resourceName}» не найден.");
        using var reader = new StreamReader(stream);
        var sql = reader.ReadToEnd();

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    private void TryDeleteBrokenDatabase()
    {
        try
        {
            if (File.Exists(DatabasePath))
            {
                File.Delete(DatabasePath);
            }
        }
        catch
        {
            // Если файл занят, пользователь увидит исходную ошибку создания БД.
        }
    }
}
