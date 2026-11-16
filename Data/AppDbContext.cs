
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pergunta>().HasData(
                new Pergunta
                {
                    IdPergunta = 1,
                    Enunciado = "O que é phishing?",
                    AlternativaA = "Um antivírus",
                    AlternativaB = "Um golpe para roubar informações",
                    AlternativaC = "Um sistema operacional",
                    AlternativaD = "Um navegador",
                    RespostaCorreta = "B",
                    Pontos = 10
                },

                new Pergunta
                {
                    IdPergunta = 2,
                    Enunciado = "Por que devemos utilizar senhas fortes?",
                    AlternativaA = "Para facilitar o acesso",
                    AlternativaB = "Para compartilhar contas",
                    AlternativaC = "Para dificultar invasões",
                    AlternativaD = "Para evitar atualizações",
                    RespostaCorreta = "C",
                    Pontos = 10
                },

                new Pergunta
                {
                    IdPergunta = 3,
                    Enunciado = "O que é autenticação de dois fatores (2FA)?",
                    AlternativaA = "Uma senha com dois números",
                    AlternativaB = "Uma camada extra de segurança que exige duas formas de verificação",
                    AlternativaC = "Um programa antivírus",
                    AlternativaD = "Um tipo de firewall",
                    RespostaCorreta = "B",
                    Pontos = 10
                },

                new Pergunta
                {
                    IdPergunta = 4,
                    Enunciado = "O que devemos fazer ao receber um e-mail suspeito com um link?",
                    AlternativaA = "Clicar para verificar o conteúdo",
                    AlternativaB = "Encaminhar para todos os contatos",
                    AlternativaC = "Não clicar e verificar a autenticidade do remetente",
                    AlternativaD = "Informar nossos dados pessoais",
                    RespostaCorreta = "C",
                    Pontos = 10
                },

                new Pergunta
                {
                    IdPergunta = 5,
                    Enunciado = "Qual é a principal função de um antivírus?",
                    AlternativaA = "Criar senhas automaticamente",
                    AlternativaB = "Proteger o computador contra softwares maliciosos",
                    AlternativaC = "Aumentar a velocidade da internet",
                    AlternativaD = "Compartilhar arquivos",
                    RespostaCorreta = "B",
                    Pontos = 10
                }
            );
        }
        public DbSet<Ranking> Ranking { get; set; }
    }

    public class Resultado
    {
    }
}
