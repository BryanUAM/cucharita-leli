using System;
namespace CucharitaLeliQR.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int Puntos { get; set; } = 0;

        public string? CodigoQR { get; set; }

        public int PremiosCanjeados { get; set; } = 0;

        public DateTime? UltimoEscaneo { get; set; }
    }
}