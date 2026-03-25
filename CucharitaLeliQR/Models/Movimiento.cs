using System;

namespace CucharitaLeliQR.Models
{
    public class Movimiento
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int Puntos { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public Cliente Cliente { get; set; }
    }
}