public class ClientSigninResult
{
    public bool Succeeded { get; set; }
    public bool PasswordNotSet { get; set; }
    public bool IsNotAllowed { get; set; }
    public bool RequiresTwoFactor { get; set; }
}