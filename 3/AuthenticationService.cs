using Serilog;

namespace AuthSystem;

/// <summary>
/// Основной модуль аутентификации со слабой связностью:
/// зависит только от абстракции <see cref="IAuthenticator"/>.
/// </summary>
public class AuthenticationService
{
    private readonly IReadOnlyList<IAuthenticator> _authenticators;

    /// <summary>
    /// Создаёт сервис с внедрённым списком аутентификаторов.
    /// </summary>
    /// <param name="authenticators">Реализации <see cref="IAuthenticator"/>.</param>
    /// <exception cref="ArgumentNullException">Если список равен null.</exception>
    /// <exception cref="ArgumentException">Если список пуст.</exception>
    public AuthenticationService(IEnumerable<IAuthenticator> authenticators)
    {
        if (authenticators is null)
        {
            Log.Error("AuthenticationService: передан null вместо списка аутентификаторов");
            throw new ArgumentNullException(nameof(authenticators));
        }

        _authenticators = authenticators.ToList();
        if (_authenticators.Count == 0)
        {
            Log.Warning("AuthenticationService создан без аутентификаторов");
            throw new ArgumentException("Нужен хотя бы один аутентификатор.", nameof(authenticators));
        }

        Log.Debug("AuthenticationService инициализирован, методов: {Count}", _authenticators.Count);
    }

    /// <summary>
    /// Пробует методы аутентификации по очереди до первого успеха.
    /// </summary>
    /// <param name="credentials">Учётные данные пользователя.</param>
    /// <returns>Результат попытки входа.</returns>
    /// <exception cref="ArgumentNullException">Если credentials равен null.</exception>
    public AuthResult Authenticate(Credentials credentials)
    {
        if (credentials is null)
        {
            Log.Error("Authenticate вызван с credentials = null");
            throw new ArgumentNullException(nameof(credentials));
        }

        if (string.IsNullOrWhiteSpace(credentials.Login))
        {
            Log.Warning("Authenticate: пустой логин");
            throw new ArgumentException("Логин не может быть пустым.", nameof(credentials));
        }

        Log.Information("Начало аутентификации пользователя {Login}", credentials.Login);
        var attempted = new List<string>();

        try
        {
            foreach (var authenticator in _authenticators)
            {
                attempted.Add(authenticator.Name);
                Log.Debug("Проверка методом {Method} для {Login}", authenticator.Name, credentials.Login);

                if (authenticator.Authenticate(credentials))
                {
                    Log.Information(
                        "Успешный вход {Login} через {Method}",
                        credentials.Login,
                        authenticator.Name);
                    return new AuthResult(credentials.Login, true, authenticator.Name, attempted);
                }
            }

            Log.Warning(
                "Отказ во входе для {Login}. Проверены: {Methods}",
                credentials.Login,
                string.Join(", ", attempted));
            return new AuthResult(credentials.Login, false, null, attempted);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Log.Error(ex, "Ошибка аутентификации пользователя {Login}", credentials.Login);
            throw;
        }
    }
}
