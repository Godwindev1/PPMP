using System.ComponentModel.DataAnnotations;

namespace PPMP.Models
{
    public class CreateClientModel
    {
        [Required(ErrorMessage = "Client name is required")]
        [Display(Name = "Client name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}



