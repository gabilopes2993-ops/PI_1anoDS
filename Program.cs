
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PI_1anoDS.Data;
using PI_1anoDS.Models;
using PI_1AnoDS.Models;

namespace PI_1AnoDS
{
    internal class Program
    {
        private const string SenhaAdmin = "admin123";

        static void Main(string[] args)
        {
            int opcao = -1;

            do
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("        NAVEGASEGURO");
                Console.WriteLine("================================");
                Console.WriteLine("1 - Jogador");
                Console.WriteLine("2 - Administrador");
                Console.WriteLine("0 - Sair");
                Console.WriteLine("================================");

                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("\nDigite uma opção válida!");
                    Pausar();
                    opcao = -1;
                    continue;
                }

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            MenuJogador();
                            break;

                        case 2:
                            LoginAdministrador();
                            break;

                        case 0:
                            Console.WriteLine("Obrigado por utilizar o NavegarSeguro");
                            break;

                        default:
                            Console.WriteLine("Opção inválida!");
                            Pausar();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nOcorreu um erro:");
                    Console.WriteLine(ex.Message);
                    Pausar();
                }

            } while (opcao != 0);
        }

        
        static void Pausar()
        {
            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        
        static int LerInteiro(string mensagem, int minimo = 0)
        {
            while (true)
            {
                Console.Write(mensagem);

                if (int.TryParse(Console.ReadLine(), out int valor)
                    && valor >= minimo)
                {
                    return valor;
                }

                Console.WriteLine("Valor inválido! Tente novamente.");
            }
        }

        
        static void MenuJogador()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("          ÁREA DO JOGADOR");
            Console.WriteLine("================================");
            Console.Write("Digite seu e-mail: ");

            string email = (Console.ReadLine() ?? "").Trim().ToLower();

            Usuario? usuario = context.Usuarios
                .FirstOrDefault(u => u.Email.ToLower() == email);

            if (usuario == null)
            {
                Console.WriteLine("\nUsuário não encontrado.");
                Console.Write("Deseja se cadastrar? (S/N): ");

                string resposta = (Console.ReadLine() ?? "")
                    .Trim().ToUpper();

                if (resposta != "S")
                {
                    Console.WriteLine("Voltando ao menu principal");
                    Pausar();
                    return;
                }

                Usuario novoUsuario = new Usuario();
                novoUsuario.CadastrarUsuario(context);

                email = novoUsuario.Email;

                usuario = context.Usuarios
                    .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

                if (usuario == null)
                {
                    Console.WriteLine("Não foi possível localizar o cadastro.");
                    Pausar();
                    return;
                }
            }

            int opcao = -1;

            do
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("         MENU DO JOGADOR");
                Console.WriteLine("================================");
                Console.WriteLine($"Jogador: {usuario.Nome}");
                Console.WriteLine("1 - Jogar quiz");
                Console.WriteLine("2 - Consultar ranking");
                Console.WriteLine("0 - Voltar");
                Console.WriteLine("================================");

                Console.Write("Escolha: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida!");
                    Pausar();
                    opcao = -1;
                    continue;
                }

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            Quiz quiz = new Quiz();
                            quiz.IniciarQuiz(context, usuario);
                            Pausar();
                            break;

                        case 2:
                            MostrarRanking(context);
                            Pausar();
                            break;

                        case 0:
                            Console.WriteLine("Voltando ao menu principal...");
                            break;

                        default:
                            Console.WriteLine("Opção inválida!");
                            Pausar();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nErro: " + ex.Message);
                    Pausar();
                }

            } while (opcao != 0);
        }

        
        static void LoginAdministrador()
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("      LOGIN ADMINISTRADOR");
            Console.WriteLine("================================");
            Console.Write("Digite a senha: ");

            string senha = Console.ReadLine() ?? "";

            if (senha != SenhaAdmin)
            {
                Console.WriteLine("\nSenha incorreta!");
                Pausar();
                return;
            }

            Console.WriteLine("\nAcesso autorizado!");
            Pausar();

            MenuAdministrador();
        }

    
        static void MenuAdministrador()
        {
            int opcao = -1;

            do
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("      MENU ADMINISTRADOR");
                Console.WriteLine("================================");
                Console.WriteLine("1 - Adicionar pergunta");
                Console.WriteLine("2 - Listar perguntas");
                Console.WriteLine("3 - Atualizar pergunta");
                Console.WriteLine("4 - Excluir pergunta");
                Console.WriteLine("0 - Voltar");
                Console.WriteLine("================================");

                Console.Write("Escolha: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida!");
                    Pausar();
                    opcao = -1;
                    continue;
                }

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            AdicionarPergunta();
                            Pausar();
                            break;

                        case 2:
                            ListarPerguntas();
                            Pausar();
                            break;

                        case 3:
                            AtualizarPergunta();
                            Pausar();
                            break;

                        case 4:
                            ExcluirPergunta();
                            Pausar();
                            break;

                        case 0:
                            Console.WriteLine("Voltando ao menu principal...");
                            break;

                        default:
                            Console.WriteLine("Opção inválida!");
                            Pausar();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nErro: " + ex.Message);
                    Pausar();
                }

            } while (opcao != 0);
        }

        
        static void AdicionarPergunta()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== ADICIONAR PERGUNTA =====");

            Pergunta pergunta = new Pergunta();

            do
            {
                Console.Write("Enunciado: ");
                pergunta.Enunciado = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(pergunta.Enunciado))
                    Console.WriteLine("O enunciado não pode ficar vazio.");

            } while (string.IsNullOrWhiteSpace(pergunta.Enunciado));

            do
            {
                Console.Write("Alternativa A: ");
                pergunta.AlternativaA = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(pergunta.AlternativaA));

            do
            {
                Console.Write("Alternativa B: ");
                pergunta.AlternativaB = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(pergunta.AlternativaB));

            do
            {
                Console.Write("Alternativa C: ");
                pergunta.AlternativaC = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(pergunta.AlternativaC));

            do
            {
                Console.Write("Alternativa D: ");
                pergunta.AlternativaD = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(pergunta.AlternativaD));

            pergunta.RespostaCorreta = LerResposta();

            Console.Write("Explicação: ");
            pergunta.Explicacao = Console.ReadLine() ?? "";

            pergunta.Pontos = LerInteiro("Pontuação: ", 1);

            context.Perguntas.Add(pergunta);
            context.SaveChanges();

            Console.WriteLine("\nPergunta cadastrada com sucesso!");
        }

        
        static string LerResposta()
        {
            while (true)
            {
                Console.Write("Resposta correta (A/B/C/D): ");

                string resposta = (Console.ReadLine() ?? "")
                    .Trim().ToUpper();

                if (resposta == "A" || resposta == "B" ||
                    resposta == "C" || resposta == "D")
                {
                    return resposta;
                }

                Console.WriteLine("Digite somente A, B, C ou D.");
            }
        }

        
        static void ListarPerguntas()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== PERGUNTAS CADASTRADAS =====");

            var perguntas = context.Perguntas
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .ToList();

            if (perguntas.Count == 0)
            {
                Console.WriteLine("Nenhuma pergunta cadastrada.");
                return;
            }

            foreach (var p in perguntas)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"ID: {p.Id}");
                Console.WriteLine($"Enunciado: {p.Enunciado}");
                Console.WriteLine($"A) {p.AlternativaA}");
                Console.WriteLine($"B) {p.AlternativaB}");
                Console.WriteLine($"C) {p.AlternativaC}");
                Console.WriteLine($"D) {p.AlternativaD}");
                Console.WriteLine($"Resposta: {p.RespostaCorreta}");
                Console.WriteLine($"Explicação: {p.Explicacao}");
                Console.WriteLine($"Pontos: {p.Pontos}");
            }
        }

        
        static void AtualizarPergunta()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== ATUALIZAR PERGUNTA =====");

            int id = LerInteiro("Digite o ID da pergunta: ", 1);

            Pergunta? pergunta = context.Perguntas.Find(id);

            if (pergunta == null)
            {
                Console.WriteLine("Pergunta não encontrada.");
                return;
            }

            Console.Write("Novo enunciado: ");
            string enunciado = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(enunciado))
            {
                Console.WriteLine("Enunciado inválido. Atualização cancelada.");
                return;
            }

            Console.Write("Nova alternativa A: ");
            string a = Console.ReadLine() ?? "";

            Console.Write("Nova alternativa B: ");
            string b = Console.ReadLine() ?? "";

            Console.Write("Nova alternativa C: ");
            string c = Console.ReadLine() ?? "";

            Console.Write("Nova alternativa D: ");
            string d = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(a) ||
                string.IsNullOrWhiteSpace(b) ||
                string.IsNullOrWhiteSpace(c) ||
                string.IsNullOrWhiteSpace(d))
            {
                Console.WriteLine("As alternativas não podem ficar vazias.");
                return;
            }

            string resposta = LerResposta();

            Console.Write("Nova explicação: ");
            string explicacao = Console.ReadLine() ?? "";

            int pontos = LerInteiro("Nova pontuação: ", 1);

            pergunta.Enunciado = enunciado.Trim();
            pergunta.AlternativaA = a.Trim();
            pergunta.AlternativaB = b.Trim();
            pergunta.AlternativaC = c.Trim();
            pergunta.AlternativaD = d.Trim();
            pergunta.RespostaCorreta = resposta;
            pergunta.Explicacao = explicacao;
            pergunta.Pontos = pontos;

            context.SaveChanges();

            Console.WriteLine("\nPergunta atualizada com sucesso!");
        }

        
        static void ExcluirPergunta()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== EXCLUIR PERGUNTA =====");

            int id = LerInteiro("Digite o ID da pergunta: ", 1);

            Pergunta? pergunta = context.Perguntas.Find(id);

            if (pergunta == null)
            {
                Console.WriteLine("Pergunta não encontrada.");
                return;
            }

            Console.WriteLine($"\n{pergunta.Enunciado}");
            Console.Write("Confirma a exclusão? (S/N): ");

            string resposta = (Console.ReadLine() ?? "")
                .Trim().ToUpper();

            if (resposta != "S")
            {
                Console.WriteLine("Exclusão cancelada.");
                return;
            }

            context.Perguntas.Remove(pergunta);
            context.SaveChanges();

            Console.WriteLine("Pergunta excluída com sucesso!");
        }

        
        static void MostrarRanking(AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("===== RANKING NAVEGASEGURO =====");

            var ranking = context.Ranking
                .Include(r => r.Usuario)
                .AsNoTracking()
                .OrderByDescending(r => r.Pontuacao)
                .ToList();

            if (ranking.Count == 0)
            {
                Console.WriteLine("Nenhum resultado encontrado.");
                return;
            }

            int posicao = 1;

            foreach (var resultado in ranking)
            {
                string nome = resultado.Usuario?.Nome
                    ?? "Usuário não encontrado";

                Console.WriteLine(
                    $"{posicao}º - {nome} | {resultado.Pontuacao} pontos"
                );

                posicao++;
            }
        }
    }
}
