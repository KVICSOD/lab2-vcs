using Serilog;

namespace AuthSystem;

public class QrAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _qrCodes;

    public QrAuthenticator(Dictionary<string, string> qrCodes)
    {
        _qrCodes = qrCodes;
    }

    public string Name => "QrAuthenticator";

    public bool Authenticate(Credentials credentials)
    {
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
