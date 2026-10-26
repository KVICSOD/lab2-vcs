using Serilog;

namespace AuthSystem;

public class PasswordAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _passwords;

    public PasswordAuthenticator(Dictionary<string, string> passwords)
    {
        _passwords = passwords;
    }

    public string Name => "PasswordAuthenticator";

    public bool Authenticate(Credentials credentials)
    {
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
