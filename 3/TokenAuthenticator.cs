using Serilog;

namespace AuthSystem;

/// <summary>
/// Аутентификация по токену (расширение без изменения AuthenticationService).
/// </summary>
public class TokenAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _tokens;

    /// <summary>
    /// Создаёт токен-аутентификатор.
    /// </summary>
    public TokenAuthenticator(Dictionary<string, string> tokens)
    {
        _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
    }

    /// <inheritdoc />
    public string Name => "TokenAuthenticator";

    /// <inheritdoc />
    public bool Authenticate(Credentials credentials)
    {
        if (credentials is null)
        {
            throw new ArgumentNullException(nameof(credentials));
        }

        var success = credentials.Token is not null
            && _tokens.TryGetValue(credentials.Login, out var expected)
            && expected == credentials.Token;

        Log.Information(
            "[Token] Проверка '{Login}': {Result}",
            credentials.Login,
            success ? "успех" : "отказ");

        return success;
    }
}
