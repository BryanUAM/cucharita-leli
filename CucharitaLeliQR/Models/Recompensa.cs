using System;
using System.ComponentModel.DataAnnotations;

namespace CucharitaLeliQR.Models
{
    public class Recompensa
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public string CodigoQR { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activo";

        public DateTime FechaGenerado { get; set; } = DateTime.Now;

        public Cliente Cliente { get; set; }
    }
}