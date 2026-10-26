using Serilog;

namespace AuthSystem;

/// <summary>
/// Аутентификация по QR-коду.
/// </summary>
public class QrAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _qrCodes;

    /// <summary>
    /// Создаёт QR-аутентификатор.
    /// </summary>
    public QrAuthenticator(Dictionary<string, string> qrCodes)
    {
        _qrCodes = qrCodes ?? throw new ArgumentNullException(nameof(qrCodes));
    }

    /// <inheritdoc />
    public string Name => "QrAuthenticator";

    /// <inheritdoc />
    public bool Authenticate(Credentials credentials)
    {
        if (credentials is null)
        {
            throw new ArgumentNullException(nameof(credentials));
        }

        var success = credentials.QrCode is not null
            && _qrCodes.TryGetValue(credentials.Login, out var expected)
            && expected == credentials.QrCode;

        Log.Information(
            "[QR] Проверка '{Login}': {Result}",
            credentials.Login,
            success ? "успех" : "отказ");

        return success;
    }
}
