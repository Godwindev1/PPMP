using System.ComponentModel.DataAnnotations;

namespace PPMP.Models
{
    public class ClientViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate {get; set;}
        [Required]
        public DateTime EndDate {get; set; }
    }
}

