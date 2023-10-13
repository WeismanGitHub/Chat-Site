namespace API.Auth;

internal sealed class JwtToken {
    public string Value { get; set; }
    public string Expiry { get; set; }
}
