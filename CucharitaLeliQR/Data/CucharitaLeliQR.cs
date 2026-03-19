using Microsoft.EntityFrameworkCore;
using CucharitaLeliQR.Models;

namespace CucharitaLeliQR.Data
{
    public class SodaContext : DbContext
    {
        public SodaContext(DbContextOptions<SodaContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Recompensa> Recompensas { get; set; }

        public DbSet<Movimiento> Movimientos { get; set; }

        public DbSet<Admin> Admins { get; set; }
    }
}