using Serilog;

namespace AuthSystem;

/// <summary>
/// Модуль аудита попыток входа (ветка feature/new-module).
/// </summary>
public class AuthAudit
{
    private readonly List<string> _entries = new List<string>();

    /// <summary>
    /// Записи аудита.
    /// </summary>
    public IReadOnlyList<string> Entries => _entries;

    /// <summary>
    /// Фиксирует результат попытки входа.
    /// </summary>
    /// <param name="result">Результат аутентификации.</param>
    public void Record(AuthResult result)
    {
        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        var entry = $"{DateTime.UtcNow:O} | {result.Login} | {(result.Success ? "OK" : "FAIL")} | {result.Method ?? "-"}";
        _entries.Add(entry);
        Log.Information("Audit: {Entry}", entry);
    }
}
