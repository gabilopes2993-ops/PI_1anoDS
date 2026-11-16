using PI_1anoDS.Data;
using Microsoft.EntityFrameworkCore;
public class Ranking
{
    public void MostrarRanking()
    {
        using (var context = new AppDbContext())
        {
            var ranking = context.Resultados
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
                Console.WriteLine(
                    $"{posicao}º - {resultado.Usuario.Nome} | " +
                    $"{resultado.Pontuacao} pontos"
                );

                posicao++;
            }

            Console.WriteLine("===========================");
        }
    }
}