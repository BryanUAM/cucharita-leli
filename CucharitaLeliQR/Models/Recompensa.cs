using Microsoft.AspNetCore.Authorization;
using System;
using System.ComponentModel.DataAnnotations;

namespace CucharitaLeliQR.Models
{
    [Authorize]
    public class Recompensa
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public string CodigoQR { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activo";

        public DateTime FechaGenerado { get; set; } = DateTime.UtcNow;

        public Cliente Cliente { get; set; }
    }
}