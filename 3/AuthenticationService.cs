namespace AuthSystem;

public class AuthenticationService
{
    private readonly IReadOnlyList<IAuthenticator> _authenticators;

    public AuthenticationService(IEnumerable<IAuthenticator> authenticators)
    {
        _authenticators = authenticators.ToList();
    }

    public AuthResult Authenticate(Credentials credentials)
    {
        var attempted = new List<string>();

        foreach (var authenticator in _authenticators)
        {
            attempted.Add(authenticator.Name);

            if (authenticator.Authenticate(credentials))
            {
                return new AuthResult(credentials.Login, true, authenticator.Name, attempted);
            }
        }

        return new AuthResult(credentials.Login, false, null, attempted);
    }
}
