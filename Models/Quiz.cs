

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

            var perguntas = context.Perguntas.ToList();

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
                        .ToUpper();

                    if (resposta == "A" || resposta == "B" ||
                        resposta == "C" || resposta == "D")
                        break;

                    Console.WriteLine("Alternativa inválida!");
                }

                if (resposta == pergunta.RespostaCorreta.Trim().ToUpper())
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

            
            Ranking novoRanking = new Ranking();

            novoRanking.UsuarioId = usuario.Id;
            novoRanking.Pontuacao = pontuacao;

            context.Ranking.Add(novoRanking);
            context.SaveChanges();

            Console.WriteLine("\nPontuação salva no ranking!");
        }
    }
}
