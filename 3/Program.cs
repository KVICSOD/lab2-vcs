using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace AuthSystem;

class Program
{
    static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/auth.log")
            .CreateLogger();

        try
        {
            Log.Information("========== СИСТЕМА АУТЕНТИФИКАЦИИ ==========");

            var password = new PasswordAuthenticator(new Dictionary<string, string>
            {
                ["ivan"] = "P@ssw0rd",
                ["maria"] = "Secret1"
            });
            var fingerprint = new FingerprintAuthenticator(new Dictionary<string, string>
            {
                ["ivan"] = "fp-ivan-001",
                ["maria"] = "fp-maria-002"
            });
            var qr = new QrAuthenticator(new Dictionary<string, string>
            {
                ["ivan"] = "qr-ivan-101"
            });

            var authenticators = new List<IAuthenticator>
            {
                password,
                fingerprint,
                qr
            };
            var service = new AuthenticationService(authenticators);
            var history = new List<AuthResult>();

            history.Add(Run(service, "Иван: верный пароль, проверка останавливается на первом успехе", new Credentials
            {
                Login = "ivan",
                Password = "P@ssw0rd",
                Fingerprint = "fp-ivan-001",
                QrCode = "qr-ivan-101"
            }));

            history.Add(Run(service, "Мария: пароль неверный, отпечаток верный", new Credentials
            {
                Login = "maria",
                Password = "wrong-password",
                Fingerprint = "fp-maria-002",
                QrCode = "qr-unknown"
            }));

            history.Add(Run(service, "Гость: ни один способ не подтвердил личность", new Credentials
            {
                Login = "guest",
                Password = "nope",
                Fingerprint = "fp-unknown",
                QrCode = "qr-unknown"
            }));

            Log.Information("--- Добавляем TokenAuthenticator без изменения AuthenticationService ---");
            var token = new TokenAuthenticator(new Dictionary<string, string>
            {
                ["bot"] = "tok-bot-777"
            });
            var extended = new AuthenticationService(new List<IAuthenticator>(authenticators) { token });

            history.Add(Run(extended, "Бот: пароль, отпечаток и QR не подходят, токен верный", new Credentials
            {
                Login = "bot",
                Password = "wrong-password",
                Fingerprint = "fp-none",
                QrCode = "qr-none",
                Token = "tok-bot-777"
            }));

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            var json = JsonConvert.SerializeObject(history, settings);
            File.WriteAllText("auth-attempts.json", json);
            Log.Information("История попыток сохранена в auth-attempts.json");

            Log.Information("=== Проверка через FluentValidation ===");
            var validator = new UserDataValidator();
            ShowValidation(validator, new UserData
            {
                Email = "test@mail.ru",
                Phone = "+375291946690",
                Password = "12345678",
                Age = 20
            });
            ShowValidation(validator, new UserData
            {
                Email = "testmail.ru",
                Phone = "+375291946690",
                Password = "12345678",
                Age = 20
            });

            var validators = new[] { nameof(UserDataValidator) };
            File.WriteAllText("validators.json", JsonConvert.SerializeObject(validators, Formatting.Indented));
            Log.Information("Список валидаторов сохранён в validators.json");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    static AuthResult Run(AuthenticationService service, string title, Credentials credentials)
    {
        Log.Information("--- {Title} ---", title);
        var result = service.Authenticate(credentials);
        Log.Information(
            "Результат для '{Login}': {Status}. Метод: {Method}. Проверены: {Methods}",
            result.Login,
            result.Success ? "успех" : "отказ",
            result.Method ?? "—",
            string.Join(", ", result.AttemptedMethods));
        return result;
    }

    static void ShowValidation(UserDataValidator validator, UserData user)
    {
        ValidationResult result = validator.Validate(user);
        if (result.IsValid)
        {
            Log.Information("FluentValidation: данные корректны ({Email}).", user.Email);
            return;
        }

        Log.Information("FluentValidation: найдены ошибки для {Email}.", user.Email);
        foreach (var error in result.Errors)
        {
            Log.Information("Ошибка: {Message}", error.ErrorMessage);
        }
    }
}
