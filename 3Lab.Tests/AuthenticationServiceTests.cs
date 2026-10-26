using AuthSystem;

namespace AuthSystem.Tests;

public class MockAuthenticator : IAuthenticator
{
    public MockAuthenticator(string name, bool shouldSucceed)
    {
        Name = name;
        ShouldSucceed = shouldSucceed;
    }

    public string Name { get; }
    public bool ShouldSucceed { get; set; }
    public List<string> Logins { get; } = new();

    public bool Authenticate(Credentials credentials)
    {
        Logins.Add(credentials.Login);
        return ShouldSucceed;
    }
}

public class AuthenticationServiceTests
{
    [Fact]
    public void Authenticate_Should_Stop_After_First_Success()
    {
        var first = new MockAuthenticator("PasswordAuthenticator", shouldSucceed: true);
        var second = new MockAuthenticator("FingerprintAuthenticator", shouldSucceed: true);
        var service = new AuthenticationService(new List<IAuthenticator> { first, second });

        var result = service.Authenticate(new Credentials { Login = "ivan" });

        Assert.True(result.Success);
        Assert.Equal("PasswordAuthenticator", result.Method);
        Assert.Contains("ivan", first.Logins);
        Assert.DoesNotContain("ivan", second.Logins);
    }

    [Fact]
    public void Authenticate_Should_Continue_Until_First_Successful_Method()
    {
        var password = new MockAuthenticator("PasswordAuthenticator", shouldSucceed: false);
        var fingerprint = new MockAuthenticator("FingerprintAuthenticator", shouldSucceed: true);
        var qr = new MockAuthenticator("QrAuthenticator", shouldSucceed: true);
        var service = new AuthenticationService(new List<IAuthenticator> { password, fingerprint, qr });

        var result = service.Authenticate(new Credentials { Login = "maria" });

        Assert.True(result.Success);
        Assert.Equal("FingerprintAuthenticator", result.Method);
        Assert.Contains("maria", password.Logins);
        Assert.Contains("maria", fingerprint.Logins);
        Assert.DoesNotContain("maria", qr.Logins);
    }

    [Fact]
    public void Authenticate_Should_Return_Failure_When_Every_Method_Rejects()
    {
        var password = new MockAuthenticator("PasswordAuthenticator", shouldSucceed: false);
        var fingerprint = new MockAuthenticator("FingerprintAuthenticator", shouldSucceed: false);
        var qr = new MockAuthenticator("QrAuthenticator", shouldSucceed: false);
        var service = new AuthenticationService(new List<IAuthenticator> { password, fingerprint, qr });

        var result = service.Authenticate(new Credentials { Login = "guest" });

        Assert.False(result.Success);
        Assert.Null(result.Method);
        Assert.Contains("guest", password.Logins);
        Assert.Contains("guest", fingerprint.Logins);
        Assert.Contains("guest", qr.Logins);
    }

    [Fact]
    public void Authenticate_Should_Call_Methods_In_Injection_Order()
    {
        var first = new MockAuthenticator("first", shouldSucceed: false);
        var second = new MockAuthenticator("second", shouldSucceed: false);
        var third = new MockAuthenticator("third", shouldSucceed: false);
        var service = new AuthenticationService(new List<IAuthenticator> { first, second, third });

        var result = service.Authenticate(new Credentials { Login = "user" });

        Assert.Equal(new[] { "first", "second", "third" }, result.AttemptedMethods);
        Assert.Equal(new[] { "user" }, first.Logins);
        Assert.Equal(new[] { "user" }, second.Logins);
        Assert.Equal(new[] { "user" }, third.Logins);
    }

    [Fact]
    public void TokenAuthenticator_Should_Work_Without_Changing_AuthenticationService()
    {
        var password = new MockAuthenticator("PasswordAuthenticator", shouldSucceed: false);
        var token = new TokenAuthenticator(new Dictionary<string, string>
        {
            ["bot"] = "tok-bot-777"
        });
        var service = new AuthenticationService(new List<IAuthenticator> { password, token });

        var result = service.Authenticate(new Credentials
        {
            Login = "bot",
            Password = "wrong-password",
            Token = "tok-bot-777"
        });

        Assert.True(result.Success);
        Assert.Equal("TokenAuthenticator", result.Method);
        Assert.Contains("bot", password.Logins);
        Assert.Equal(
            new[] { "PasswordAuthenticator", "TokenAuthenticator" },
            result.AttemptedMethods);
    }
}
