namespace AuthSystem;

/// <summary>
/// Набор учётных данных для разных способов входа.
/// </summary>
public class Credentials
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; } = "";

    /// <summary>
    /// Пароль (не логировать).
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Шаблон отпечатка пальца.
    /// </summary>
    public string? Fingerprint { get; set; }

    /// <summary>
    /// QR-код.
    /// </summary>
    public string? QrCode { get; set; }

    /// <summary>
    /// Токен доступа.
    /// </summary>
    public string? Token { get; set; }
}
