using Serilog;

namespace AuthSystem;

/// <summary>
/// Аутентификация по отпечатку пальца.
/// </summary>
public class FingerprintAuthenticator : IAuthenticator
{
    private readonly Dictionary<string, string> _fingerprints;

    /// <summary>
    /// Создаёт аутентификатор отпечатков.
    /// </summary>
    public FingerprintAuthenticator(Dictionary<string, string> fingerprints)
    {
        _fingerprints = fingerprints ?? throw new ArgumentNullException(nameof(fingerprints));
    }

    /// <inheritdoc />
    public string Name => "FingerprintAuthenticator";

    /// <inheritdoc />
    public bool Authenticate(Credentials credentials)
    {
        if (credentials is null)
        {
            throw new ArgumentNullException(nameof(credentials));
        }

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
