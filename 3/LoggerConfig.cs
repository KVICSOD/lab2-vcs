using Serilog;
using Serilog.Events;

namespace AuthSystem;

/// <summary>
/// Централизованная настройка Serilog: консоль (INFO) и файл (DEBUG).
/// </summary>
public static class LoggerConfig
{
    /// <summary>
    /// Инициализирует глобальный логгер.
    /// </summary>
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "logs/auth_.log",
                restrictedToMinimumLevel: LogEventLevel.Debug,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();

        Log.Information("Logger initialized");
    }
}
