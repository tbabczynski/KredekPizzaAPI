using System.ComponentModel.DataAnnotations;

namespace JakubKalinaLab7.Models
{
    public class Pizza
    {
        // prop + tab + tab

        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [StringLength(100, ErrorMessage = "Pole {0} może mieć maksymalnie {1} znaków.")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        [StringLength(500, ErrorMessage = "Pole {0} może mieć maksymalnie {1} znaków.")]
        public string Description { get; set; }

        [Display(Name = "Cena")]
        [Range(0.0, 9999.99, ErrorMessage = "Pole {0} musi być w zakresie od {1} do {2}.")]
        public float Cost { get; set; }
    }
}
