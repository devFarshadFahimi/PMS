namespace Infrastructure.Options;

public class AuthTokenOption
{
    public bool IsEnabled { get; set; }
    public string SigningKey { get; set; } = string.Empty;
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public long TokenExpireMinutes { get; set; }
}