using System.ComponentModel.DataAnnotations;
namespace PPMP.Models
{
    public class ClientLoginViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}

