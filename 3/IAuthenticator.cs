namespace AuthSystem;

public interface IAuthenticator
{
    string Name { get; }

    bool Authenticate(Credentials credentials);
}
