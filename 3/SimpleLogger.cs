using Serilog;

namespace AuthSystem;

/// <summary>
/// Простая обёртка логирования для демонстрации работы с ветками Git.
/// </summary>
public static class SimpleLogger
{
    public static void Info(string message)
    {
        Log.Information("[SimpleLogger] {Message}", message);
    }

    public static void Debug(string message)
    {
        Log.Debug("[SimpleLogger] {Message}", message);
    }
}
