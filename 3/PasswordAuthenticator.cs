using Serilog;

namespace AuthSystem;

/// <summary>
/// Аутентификация по логину и паролю.
/// </summary>
public class PasswordAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _passwords;

    /// <summary>
    /// Создаёт аутентификатор паролей.
    /// </summary>
    /// <param name="passwords">Словарь логин → пароль.</param>
    public PasswordAuthenticator(Dictionary<string, string> passwords)
    {
        _passwords = passwords ?? throw new ArgumentNullException(nameof(passwords));
    }

    /// <inheritdoc />
    public string Name => "PasswordAuthenticator";

    /// <inheritdoc />
    public bool Authenticate(Credentials credentials)
    {
        if (credentials is null)
        {
            throw new ArgumentNullException(nameof(credentials));
        }

        var success = credentials.Password is not null
            && _passwords.TryGetValue(credentials.Login, out var expected)
            && expected == credentials.Password;

        Log.Information(
            "[Password] Проверка '{Login}': {Result}",
            credentials.Login,
            success ? "успех" : "отказ");

        return success;
    }
}
