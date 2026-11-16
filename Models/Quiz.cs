
using System;
using System.Linq;
using PI_1anoDS.Data;

namespace PI_1anoDS.Models
{
    public class Quiz
    {
        public void IniciarQuiz()
        {
            int pontuacao = 0;

            using (var context = new AppDbContext())
            {
                var perguntas = context.Perguntas.ToList();

                foreach (var pergunta in perguntas)
                {
                    Console.WriteLine(pergunta.Enunciado);
                    Console.WriteLine("A) " + pergunta.AlternativaA);
                    Console.WriteLine("B) " + pergunta.AlternativaB);
                    Console.WriteLine("C) " + pergunta.AlternativaC);
                    Console.WriteLine("D) " + pergunta.AlternativaD);

                    Console.Write("Sua resposta: ");
                    string resposta = Console.ReadLine();

                    if (resposta?.Trim().ToUpper() == pergunta.RespostaCorreta.ToUpper())
                    {
                        Console.WriteLine("Resposta correta!");
                        pontuacao += pergunta.Pontos;
                    }
                    else
                    {
                        Console.WriteLine("Resposta incorreta!");
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine("Pontuação final: " + pontuacao);
        }
    }
}
