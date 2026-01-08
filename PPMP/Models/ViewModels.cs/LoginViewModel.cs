using System.ComponentModel.DataAnnotations;

namespace PPMP.Models
{
    public class LoginViewModel
    {
            [EmailAddress]
            [Required]
            public string Email { get; set; }

            [Required]
            [MinLength(8)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }

    }
}
