
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
        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("===== NAVEGASEGURO =====");
                Console.WriteLine("1 - Jogador");
                Console.WriteLine("2 - Administrador");
                Console.WriteLine("0 - Sair");

                opcao = LerInteiro("Escolha: ");

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
                            Console.WriteLine("Até logo!");
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            Pausar();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                    Pausar();
                }

            } while (opcao != 0);
        }

        static void Pausar()
        {
            Console.WriteLine("\nPressione ENTER para continuar");
            Console.ReadLine();
        }

        static int LerInteiro(string mensagem, int minimo = 0)
        {
            while (true)
            {
                Console.Write(mensagem);

                if (int.TryParse(Console.ReadLine(), out int numero)
                    && numero >= minimo)
                    return numero;

                Console.WriteLine("Digite um número válido!");
            }
        }

        static string LerTexto(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);
                string texto = (Console.ReadLine() ?? "").Trim();

                if (texto != "")
                    return texto;

                Console.WriteLine("Este campo é obrigatório!");
            }
        }

        
        static void MenuJogador()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== LOGIN DO JOGADOR =====");

            string email = LerTexto("E-mail: ").ToLower();

            Usuario? usuario = context.Usuarios
                .FirstOrDefault(u => u.Email.ToLower() == email);

            if (usuario == null)
            {
                Console.WriteLine("\nUsuário não encontrado.");
                Console.Write("Deseja se cadastrar? (S/N): ");

                string resposta = (Console.ReadLine() ?? "")
                    .Trim().ToUpper();

                if (resposta != "S")
                    return;

                usuario = CadastrarJogador(context, email);
            }
            else
            {
                if (usuario.TipoUsuario != "Jogador")
                {
                    Console.WriteLine("Esta conta não é de jogador.");
                    Pausar();
                    return;
                }

                Console.Write("Senha: ");
                string senha = Console.ReadLine() ?? "";

                if (usuario.Senha != senha)
                {
                    Console.WriteLine("\nSenha incorreta!");
                    Pausar();
                    return;
                }
            }

            Console.WriteLine("\nLogin realizado com sucesso!");
            Pausar();

            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("===== MENU DO JOGADOR =====");
                Console.WriteLine($"Jogador: {usuario.Nome}");
                Console.WriteLine("1 - Jogar quiz");
                Console.WriteLine("2 - Consultar ranking");
                Console.WriteLine("0 - Voltar");

                opcao = LerInteiro("Escolha: ");

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
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            Pausar();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                    Pausar();
                }

            } while (opcao != 0);
        }

        
        static Usuario CadastrarJogador(AppDbContext context, string email)
        {
            Console.Clear();
            Console.WriteLine("===== CADASTRO DO JOGADOR =====");

            string nome = LerTexto("Nome: ");
            string senha = LerTexto("Crie uma senha: ");

            Usuario usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                Senha = senha,
                TipoUsuario = "Jogador"
            };

            context.Usuarios.Add(usuario);
            context.SaveChanges();

            Console.WriteLine("\nCadastro realizado com sucesso!");

            return usuario;
        }

        
        static void LoginAdministrador()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== LOGIN ADMINISTRADOR =====");

            string email = LerTexto("E-mail: ").ToLower();

            Console.Write("Senha: ");
            string senha = Console.ReadLine() ?? "";

            Usuario? admin = context.Usuarios
                .FirstOrDefault(u =>
                    u.Email.ToLower() == email &&
                    u.Senha == senha &&
                    u.TipoUsuario == "Admin");

            if (admin == null)
            {
                Console.WriteLine("\nE-mail ou senha incorretos!");
                Pausar();
                return;
            }

            Console.WriteLine("\nAcesso autorizado!");
            Pausar();

            MenuAdministrador();
        }

        
        static void MenuAdministrador()
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("===== MENU ADMINISTRADOR =====");
                Console.WriteLine("1 - Adicionar pergunta");
                Console.WriteLine("2 - Listar perguntas");
                Console.WriteLine("3 - Atualizar pergunta");
                Console.WriteLine("4 - Excluir pergunta");
                Console.WriteLine("0 - Voltar");

                opcao = LerInteiro("Escolha: ");

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
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            Pausar();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                    Pausar();
                }

            } while (opcao != 0);
        }

        
        static void AdicionarPergunta()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== ADICIONAR PERGUNTA =====");

            Pergunta pergunta = new Pergunta
            {
                Enunciado = LerTexto("Enunciado: "),
                AlternativaA = LerTexto("Alternativa A: "),
                AlternativaB = LerTexto("Alternativa B: "),
                AlternativaC = LerTexto("Alternativa C: "),
                AlternativaD = LerTexto("Alternativa D: "),
                RespostaCorreta = LerResposta()
            };

            Console.Write("Explicação: ");
            pergunta.Explicacao = Console.ReadLine() ?? "";

            pergunta.Pontos = LerInteiro("Pontuação: ", 1);

            context.Perguntas.Add(pergunta);
            context.SaveChanges();

            Console.WriteLine("\nPergunta cadastrada!");
        }

        
        static string LerResposta()
        {
            while (true)
            {
                string resposta = LerTexto(
                    "Resposta correta (A/B/C/D): "
                ).ToUpper();

                if (resposta == "A" || resposta == "B" ||
                    resposta == "C" || resposta == "D")
                    return resposta;

                Console.WriteLine("Digite A, B, C ou D!");
            }
        }

        
        static void ListarPerguntas()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== PERGUNTAS =====");

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
                Console.WriteLine("\n------------------------");
                Console.WriteLine($"ID: {p.Id}");
                Console.WriteLine($"Pergunta: {p.Enunciado}");
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

            int id = LerInteiro("ID da pergunta: ", 1);
            Pergunta? pergunta = context.Perguntas.Find(id);

            if (pergunta == null)
            {
                Console.WriteLine("Pergunta não encontrada!");
                return;
            }

            pergunta.Enunciado = LerTexto("Novo enunciado: ");
            pergunta.AlternativaA = LerTexto("Nova alternativa A: ");
            pergunta.AlternativaB = LerTexto("Nova alternativa B: ");
            pergunta.AlternativaC = LerTexto("Nova alternativa C: ");
            pergunta.AlternativaD = LerTexto("Nova alternativa D: ");
            pergunta.RespostaCorreta = LerResposta();

            Console.Write("Nova explicação: ");
            pergunta.Explicacao = Console.ReadLine() ?? "";

            pergunta.Pontos = LerInteiro("Nova pontuação: ", 1);

            context.SaveChanges();

            Console.WriteLine("\nPergunta atualizada!");
        }

        
        static void ExcluirPergunta()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== EXCLUIR PERGUNTA =====");

            int id = LerInteiro("ID da pergunta: ", 1);
            Pergunta? pergunta = context.Perguntas.Find(id);

            if (pergunta == null)
            {
                Console.WriteLine("Pergunta não encontrada!");
                return;
            }

            Console.WriteLine($"\n{pergunta.Enunciado}");
            Console.Write("Deseja excluir? (S/N): ");

            string resposta = (Console.ReadLine() ?? "")
                .Trim().ToUpper();

            if (resposta != "S")
            {
                Console.WriteLine("Exclusão cancelada.");
                return;
            }

            context.Perguntas.Remove(pergunta);
            context.SaveChanges();

            Console.WriteLine("Pergunta excluída!");
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
                    ?? "Usuário desconhecido";

                Console.WriteLine(
                    $"{posicao}º - {nome} | {resultado.Pontuacao} pontos"
                );

                posicao++;
            }
        }
    }
}
