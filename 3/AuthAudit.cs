using Serilog;

namespace AuthSystem;

/// <summary>
/// Модуль аудита попыток входа (ветка feature/new-module).
/// </summary>
public class AuthAudit
{
    private readonly List<string> _entries = new();

    public IReadOnlyList<string> Entries => _entries;

    public void Record(AuthResult result)
    {
        var entry = $"{DateTime.UtcNow:O} | {result.Login} | {(result.Success ? "OK" : "FAIL")} | {result.Method ?? "-"}";
        _entries.Add(entry);
        Log.Information("Audit: {Entry}", entry);
    }
}
