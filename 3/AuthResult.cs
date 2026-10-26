namespace AuthSystem;

public class AuthResult
{
    public AuthResult(string login, bool success, string? method, IReadOnlyList<string> attemptedMethods)
    {
        Login = login;
        Success = success;
        Method = method;
        AttemptedMethods = attemptedMethods;
    }

    public string Login { get; }
    public bool Success { get; }
    public string? Method { get; }
    public IReadOnlyList<string> AttemptedMethods { get; }
}
