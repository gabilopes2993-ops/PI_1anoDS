
using Microsoft.EntityFrameworkCore;
using PI_1anoDS.Models;
using PI_1AnoDS.Models;

namespace PI_1anoDS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }
        public DbSet<Ranking> Rankings { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(
                @"server=localhost;
                database=navegarseguro;
                user=root;
                password=Senac2026;");
        }

        public DbSet<Ranking> Ranking { get; set; }
    }

    public class Resultado
    {
    }
}
