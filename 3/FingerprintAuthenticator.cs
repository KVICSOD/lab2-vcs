using Serilog;

namespace AuthSystem;

public class FingerprintAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _fingerprints;

    public FingerprintAuthenticator(Dictionary<string, string> fingerprints)
    {
        _fingerprints = fingerprints;
    }

    public string Name => "FingerprintAuthenticator";

    public bool Authenticate(Credentials credentials)
    {
        var success = credentials.Fingerprint is not null
            && _fingerprints.TryGetValue(credentials.Login, out var expected)
            && expected == credentials.Fingerprint;

        Log.Information(
            "[Fingerprint] Проверка '{Login}': {Result}",
            credentials.Login,
            success ? "успех" : "отказ");

        return success;
    }
}
