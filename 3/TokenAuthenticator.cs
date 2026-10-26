using Serilog;

namespace AuthSystem;

public class TokenAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _tokens;

    public TokenAuthenticator(Dictionary<string, string> tokens)
    {
        _tokens = tokens;
    }

    public string Name => "TokenAuthenticator";

    public bool Authenticate(Credentials credentials)
    {
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
