namespace Contract.Domain.UserAggregate;

public sealed class UserLogin : DomainObject
{
#pragma warning disable CS8618
    // Attributes
    public string LoginProvider { get; set; }
    public string ProviderKey { get; private set; }
#pragma warning restore CS8618

    public UserLogin(string loginProvider, string providerKey)
    {
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
    }
}