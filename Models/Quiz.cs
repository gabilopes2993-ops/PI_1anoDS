



using System;
using System.Linq;
using PI_1anoDS.Data;
using PI_1anoDS.Models;
using PI_1AnoDS.Models;

namespace PI_1AnoDS.Models
{
    public class Quiz
    {
        public void IniciarQuiz(AppDbContext context, Usuario usuario)
        {
            int pontuacao = 0;

            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("       QUIZ NAVEGASEGURO");
            Console.WriteLine("==============================");
            Console.WriteLine("Jogador: " + usuario.Nome);

            var perguntas = context.Perguntas
                .ToList()
                .OrderBy(p => Guid.NewGuid())
                .ToList();

            if (perguntas.Count == 0)
            {
                Console.WriteLine("Nenhuma pergunta cadastrada.");
                return;
            }

            foreach (var pergunta in perguntas)
            {
                Console.WriteLine("\n" + pergunta.Enunciado);
                Console.WriteLine("A) " + pergunta.AlternativaA);
                Console.WriteLine("B) " + pergunta.AlternativaB);
                Console.WriteLine("C) " + pergunta.AlternativaC);
                Console.WriteLine("D) " + pergunta.AlternativaD);

                string resposta;

                while (true)
                {
                    Console.Write("Sua resposta (A/B/C/D): ");

                    resposta = (Console.ReadLine() ?? "")
                        .Trim()
                        .ToUpperInvariant();

                    if (resposta == "A" || resposta == "B" ||
                        resposta == "C" || resposta == "D")
                    {
                        break;
                    }

                    Console.WriteLine("Alternativa inválida!");
                }

                string respostaCorreta = (pergunta.RespostaCorreta ?? "")
                    .Trim()
                    .ToUpperInvariant();

                if (respostaCorreta != "A" &&
                    respostaCorreta != "B" &&
                    respostaCorreta != "C" &&
                    respostaCorreta != "D")
                {
                    Console.WriteLine("Erro: resposta correta inválida no cadastro da pergunta.");
                    continue;
                }

                if (resposta == respostaCorreta)
                {
                    Console.WriteLine("Resposta correta!");
                    pontuacao += pergunta.Pontos;
                }
                else
                {
                    Console.WriteLine("Resposta incorreta!");
                    Console.WriteLine("Explicação: " + pergunta.Explicacao);
                }
            }

            Console.WriteLine("\n==============================");
            Console.WriteLine("       RESULTADO FINAL");
            Console.WriteLine("==============================");
            Console.WriteLine("Jogador: " + usuario.Nome);
            Console.WriteLine("Pontuação: " + pontuacao);

            Ranking? rankingExistente = context.Ranking
                .FirstOrDefault(r => r.UsuarioId == usuario.Id);

            if (rankingExistente == null)
            {
                Ranking novoRanking = new Ranking();

                novoRanking.UsuarioId = usuario.Id;
                novoRanking.Pontuacao = pontuacao;

                context.Ranking.Add(novoRanking);

                Console.WriteLine("\nPrimeira pontuação registrada no ranking!");
            }
            else if (pontuacao > rankingExistente.Pontuacao)
            {
                rankingExistente.Pontuacao = pontuacao;

                Console.WriteLine("\nNova melhor pontuação registrada!");
            }
            else
            {
                Console.WriteLine("\nSua melhor pontuação continua sendo: "
                    + rankingExistente.Pontuacao);
            }

            context.SaveChanges();

            Console.WriteLine("\nPontuação salva no ranking!");
        }
    }
}
