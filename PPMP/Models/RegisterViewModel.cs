using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PPMP.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

}
