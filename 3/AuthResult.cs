namespace AuthSystem;

/// <summary>
/// Результат попытки аутентификации.
/// </summary>
public class AuthResult
{
    /// <summary>
    /// Создаёт результат попытки входа.
    /// </summary>
    public AuthResult(string login, bool success, string? method, IReadOnlyList<string> attemptedMethods)
    {
        Login = login ?? throw new ArgumentNullException(nameof(login));
        Success = success;
        Method = method;
        AttemptedMethods = attemptedMethods ?? throw new ArgumentNullException(nameof(attemptedMethods));
    }

    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Успех или отказ.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Метод, которым удалось войти (или null).
    /// </summary>
    public string? Method { get; }

    /// <summary>
    /// Список проверенных методов.
    /// </summary>
    public IReadOnlyList<string> AttemptedMethods { get; }
}
