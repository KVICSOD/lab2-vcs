namespace AuthSystem;

/// <summary>
/// Абстракция способа проверки подлинности пользователя.
/// </summary>
public interface IAuthenticator
{
    /// <summary>
    /// Человекочитаемое имя метода.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Проверяет учётные данные выбранным способом.
    /// </summary>
    /// <param name="credentials">Учётные данные.</param>
    /// <returns>true, если проверка пройдена.</returns>
    bool Authenticate(Credentials credentials);
}
