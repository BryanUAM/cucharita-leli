using System.ComponentModel.DataAnnotations;

namespace CucharitaLeliQR.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50)]
        public string Usuario { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}