using Models;
using Microsoft.EntityFrameworkCore;

namespace PI_1anoDS.Data
{
    public class AppDnContext :DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }
        public DbSet<Alternativa> Alternativas { get; set; }
        public DbSet<Tentativa> Tentativas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(
                @"server=localhost;
                database = navegarseguro;
                user = root;
                password = Senac2026;");
            
        }
    }

}