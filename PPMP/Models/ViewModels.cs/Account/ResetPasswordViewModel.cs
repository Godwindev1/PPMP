using System.ComponentModel.DataAnnotations;
public class ResetPasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string token {get; set;}

    [Required]
    public string UserID {get; set;}
}