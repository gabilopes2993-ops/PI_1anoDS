using System;
using System.Linq;
using PI_1anoDS.Data;

 public void IniciarQuiz()
{
    public int pontuacao = 0;


    foreach (var pergunta in perguntas)
    {
        Console.WriteLine(pergunta.Enunciado);
        Console.WriteLine("A) " + pergunta.AlternativaA);
        Console.WriteLine("B) " + pergunta.AlternativaB);
        Console.WriteLine("C) " + pergunta.AlternativaC);
        Console.WriteLine("D) " + pergunta.AlternativaD);

        Console.Write("Sua resposta: ");
        string resposta = Console.ReadLine();

        if (resposta.ToUpper() == pergunta.RespostaCorreta)
        {
            Console.WriteLine("Resposta correta!");
            pontuacao += pergunta.Pontos;
        }
        else
        {
            Console.WriteLine("Resposta incorreta!");
        }
    }

     Console.WriteLine("Pontuação final: " + pontuacao);
}