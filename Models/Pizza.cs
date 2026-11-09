using System.ComponentModel.DataAnnotations;

namespace JakubKalinaLab7.Models
{
    /// <summary>
    /// Reprezentuje pizzę
    /// </summary>
    public class Pizza
    {
        // prop + tab + tab

        /// <summary>
        /// Identyfikator pizzy
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa pizzy
        /// </summary>
        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [StringLength(100, ErrorMessage = "Pole {0} może mieć maksymalnie {1} znaków.")]
        public string Name { get; set; }

        /// <summary>
        /// Opis pizzy
        /// </summary>
        [Display(Name = "Opis")]
        [StringLength(500, ErrorMessage = "Pole {0} może mieć maksymalnie {1} znaków.")]
        public string Description { get; set; }

        /// <summary>
        /// Cena pizzy
        /// </summary>
        [Display(Name = "Cena")]
        [Range(0.0, 9999.99, ErrorMessage = "Pole {0} musi być w zakresie od {1} do {2}.")]
        public float Cost { get; set; }
    }
}
