
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PI_1anoDS.Models
{
    public class Ranking
    {
        public void MostrarRanking()
        {
            using (var context = new AppDbContext())
            {
                var ranking = context.Ranking
                    .Include(r => r.Usuario)
                    .OrderByDescending(r => r.Pontuacao)
                    .ToList();

                Console.WriteLine("\n===== RANKING DO QUIZ =====");

                if (ranking.Count == 0)
                {
                    Console.WriteLine("Nenhum resultado encontrado.");
                    return;
                }

                int posicao = 1;

                foreach (var resultado in ranking)
                {
                    string nome = resultado.Usuario != null
                        ? resultado.Usuario.Nome
                        : "Usuário não encontrado";

                    Console.WriteLine(
                        $"{posicao}º - {nome} | " +
                        $"{resultado.Pontuacao} pontos"
                    );

                    posicao++;
                }

                Console.WriteLine("===========================");
            }
        }
    }
}
