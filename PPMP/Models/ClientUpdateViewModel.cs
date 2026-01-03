using System.ComponentModel.DataAnnotations;
    public class ClientUpdateViewModel
    {
        [Required]
        public string Token { get; set; }
        
         [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }


        [Required]
        public bool RememberMe {get; set; }
    }