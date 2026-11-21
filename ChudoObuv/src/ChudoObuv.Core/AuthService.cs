namespace ChudoObuv.Core;

/// <summary>Авторизация только по логину (пароль не требуется по заданию).</summary>
public sealed class AuthService
{
    private readonly Database _database;

    public AuthService(Database database)
    {
        _database = database;
    }

    public IReadOnlyList<LoginOption> GetLoginOptions()
    {
        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT u.Login,
                   u.LastName || ' ' || u.FirstName || ' ' || u.MiddleName AS FullName,
                   r.RoleName
            FROM Users u
            JOIN Roles r ON r.RoleId = u.RoleId
            ORDER BY u.Login;
            """;

        using var reader = command.ExecuteReader();
        var result = new List<LoginOption>();
        while (reader.Read())
        {
            result.Add(new LoginOption(
                reader.GetString(0),
                reader.GetString(1).Trim(),
                reader.GetString(2)));
        }

        return result;
    }

    public UserSession? Authenticate(string login)
    {
        var normalized = (login ?? string.Empty).Trim();
        if (normalized.Length == 0)
        {
            return null;
        }

        using var connection = _database.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT u.UserId,
                   u.Login,
                   u.LastName || ' ' || u.FirstName || ' ' || u.MiddleName AS FullName,
                   r.RoleId
            FROM Users u
            JOIN Roles r ON r.RoleId = u.RoleId
            WHERE u.Login = @login COLLATE NOCASE
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("@login", normalized);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new UserSession(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2).Trim(),
            (UserRole)reader.GetInt32(3));
    }
}
