namespace AuthSystem;

public class Credentials
{
    public string Login { get; set; } = "";
    public string? Password { get; set; }
    public string? Fingerprint { get; set; }
    public string? QrCode { get; set; }
    public string? Token { get; set; }
}
