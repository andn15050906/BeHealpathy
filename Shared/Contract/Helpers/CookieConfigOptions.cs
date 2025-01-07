namespace Contract.Helpers;

public sealed class CookieConfigOptions
{
    public bool Secure { get; set; }
    public short Expires { get; set; }        //in minutes
}