
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Mail;
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
            int opcao = -1;

            do
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("         NAVEGASEGURO");
                Console.WriteLine("================================");
                Console.WriteLine("1 - Jogador");
                Console.WriteLine("2 - Administrador");
                Console.WriteLine("0 - Sair");
                Console.WriteLine("================================");

                opcao = LerInteiro("Escolha uma opção: ", 0, 2);

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
                            Console.WriteLine(
                                "Obrigado por utilizar o NavegaSeguro!");
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
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }

        static int LerInteiro(
            string mensagem,
            int minimo = 0,
            int maximo = int.MaxValue)
        {
            while (true)
            {
                Console.Write(mensagem);

                if (int.TryParse(Console.ReadLine(), out int valor)
                    && valor >= minimo
                    && valor <= maximo)
                {
                    return valor;
                }

                Console.WriteLine(
                    $"Valor inválido! Digite um número entre {minimo} e {maximo}.");
            }
        }

        static bool ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            nome = nome.Trim();

            if (nome.Length < 2 || nome.Length > 100)
                return false;

            return Regex.IsMatch(
                nome,
                @"^\p{L}+(?:[\s'-]+\p{L}+)*$"
            );
        }

        static bool ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                return false;

            return senha.Length >= 8
                && senha.Length <= 100
                && Regex.IsMatch(senha, @"[A-Z]")
                && Regex.IsMatch(senha, @"[a-z]")
                && Regex.IsMatch(senha, @"[0-9]")
                && Regex.IsMatch(senha, @"[^a-zA-Z0-9\s]")
                && !Regex.IsMatch(senha, @"\s");
        }

        static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            email = email.Trim();

            if (email.Length > 150)
                return false;

            try
            {
                MailAddress endereco = new MailAddress(email);

                if (endereco.Address != email)
                    return false;

                string dominio = endereco.Host;

                if (!dominio.Contains("."))
                    return false;

                if (dominio.StartsWith(".") ||
                    dominio.EndsWith("."))
                    return false;

                string[] partes = dominio.Split('.');

                if (partes.Length < 2)
                    return false;

                string extensao = partes.Last();

                if (extensao.Length < 2 ||
                    !Regex.IsMatch(extensao, @"^[a-zA-Z]+$"))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        static Usuario? CadastrarJogador(AppDbContext context)
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("       CADASTRO DE JOGADOR");
            Console.WriteLine("================================");

            string nome;

            do
            {
                Console.Write("\nDigite seu nome: ");
                nome = (Console.ReadLine() ?? "").Trim();

                if (!ValidarNome(nome))
                {
                    Console.WriteLine(
                        "Nome inválido! Utilize letras, espaços, hífens ou apóstrofos.");
                }

            } while (!ValidarNome(nome));

            string email;

            while (true)
            {
                Console.Write("\nDigite seu e-mail: ");

                email = (Console.ReadLine() ?? "")
                    .Trim()
                    .ToLowerInvariant();

                if (!ValidarEmail(email))
                {
                    Console.WriteLine("E-mail inválido!");
                    Console.WriteLine("Exemplo: usuario@gmail.com");
                    continue;
                }

                bool existe = context.Usuarios.Any(
                    u => u.Email.ToLower() == email);

                if (existe)
                {
                    Console.WriteLine(
                        "Este e-mail já está cadastrado!");
                    continue;
                }

                break;
            }

            string senha;

            do
            {
                Console.Write("\nCrie uma senha: ");
                senha = Console.ReadLine() ?? "";

                if (!ValidarSenha(senha))
                {
                    Console.WriteLine("\nSenha inválida!");
                    Console.WriteLine("A senha precisa ter:");
                    Console.WriteLine("- No mínimo 8 caracteres");
                    Console.WriteLine("- No máximo 100 caracteres");
                    Console.WriteLine("- Uma letra maiúscula");
                    Console.WriteLine("- Uma letra minúscula");
                    Console.WriteLine("- Pelo menos um número");
                    Console.WriteLine("- Pelo menos um caractere especial");
                    Console.WriteLine("- Nenhum espaço");
                }

            } while (!ValidarSenha(senha));

            string confirmacaoSenha;

            do
            {
                Console.Write("\nConfirme sua senha: ");
                confirmacaoSenha = Console.ReadLine() ?? "";

                if (senha != confirmacaoSenha)
                {
                    Console.WriteLine("As senhas não coincidem!");
                }

            } while (senha != confirmacaoSenha);

            Usuario usuario = new Usuario();

            usuario.Nome = nome;
            usuario.Email = email;
            usuario.Senha = senha;
            usuario.TipoUsuario = "Jogador";

            context.Usuarios.Add(usuario);
            context.SaveChanges();

            Console.WriteLine("\nCadastro realizado com sucesso!");

            return usuario;
        }

        static void MenuJogador()
        {
            using var context = new AppDbContext();

            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("          ÁREA DO JOGADOR");
            Console.WriteLine("================================");

            Console.Write("Digite seu e-mail: ");

            string email = (Console.ReadLine() ?? "")
                .Trim()
                .ToLowerInvariant();

            if (!ValidarEmail(email))
            {
                Console.WriteLine("E-mail inválido!");
                Pausar();
                return;
            }

            Usuario? usuario = context.Usuarios
                .FirstOrDefault(u => u.Email.ToLower() == email);

            if (usuario == null)
            {
                Console.WriteLine("\nUsuário não encontrado.");
                Console.Write("Deseja se cadastrar? (S/N): ");

                string resposta = (Console.ReadLine() ?? "")
                    .Trim()
                    .ToUpperInvariant();

                if (resposta != "S")
                {
                    Console.WriteLine("Voltando ao menu principal...");
                    Pausar();
                    return;
                }

                usuario = CadastrarJogador(context);

                if (usuario == null)
                    return;
            }
            else
            {
                Console.Write("\nDigite sua senha: ");

                string senha = Console.ReadLine() ?? "";

                if (usuario.Senha != senha)
                {
                    Console.WriteLine("\nSenha incorreta!");
                    Pausar();
                    return;
                }

                Console.WriteLine("\nLogin realizado com sucesso!");
                Pausar();
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

                opcao = LerInteiro("Escolha: ", 0, 2);

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
                            Console.WriteLine(
                                "Voltando ao menu principal...");
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
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== LOGIN ADMINISTRADOR =====");

            Console.Write("E-mail: ");
            string email = (Console.ReadLine() ?? "").Trim();

            Console.Write("Senha: ");
            string senha = Console.ReadLine() ?? "";

            var admin = context.Usuarios.FirstOrDefault(u =>
                u.Email == email &&
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

                opcao = LerInteiro("Escolha: ", 0, 4);

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
                            Console.WriteLine(
                                "Voltando ao menu principal...");
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

        static string LerTextoObrigatorio(
            string mensagem,
            int maximo = 1000)
        {
            while (true)
            {
                Console.Write(mensagem);

                string texto = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine(
                        "Este campo não pode ficar vazio!");
                    continue;
                }

                if (texto.Length > maximo)
                {
                    Console.WriteLine(
                        $"O texto não pode ultrapassar {maximo} caracteres!");
                    continue;
                }

                return texto;
            }
        }

        static string[] LerAlternativasDiferentes()
        {
            while (true)
            {
                string a = LerTextoObrigatorio("Alternativa A: ", 200);
                string b = LerTextoObrigatorio("Alternativa B: ", 200);
                string c = LerTextoObrigatorio("Alternativa C: ", 200);
                string d = LerTextoObrigatorio("Alternativa D: ", 200);

                string[] alternativas = { a, b, c, d };

                if (alternativas.Distinct(
                    StringComparer.OrdinalIgnoreCase).Count() == 4)
                {
                    return alternativas;
                }

                Console.WriteLine(
                    "\nErro! As quatro alternativas precisam ser diferentes.");

                Console.WriteLine("Digite novamente as alternativas.\n");
            }
        }

        static string LerResposta()
        {
            while (true)
            {
                Console.Write("Resposta correta (A/B/C/D): ");

                string resposta = (Console.ReadLine() ?? "")
                    .Trim()
                    .ToUpperInvariant();

                if (resposta == "A" || resposta == "B" ||
                    resposta == "C" || resposta == "D")
                {
                    return resposta;
                }

                Console.WriteLine("Digite somente A, B, C ou D.");
            }
        }

        // ADICIONAR PERGUNTA
        static void AdicionarPergunta()
        {
            using var context = new AppDbContext();

            Console.Clear();
            Console.WriteLine("===== ADICIONAR PERGUNTA =====");

            Pergunta pergunta = new Pergunta();

            pergunta.Enunciado = LerTextoObrigatorio(
                "Enunciado: ", 500);

            string[] alternativas = LerAlternativasDiferentes();

            pergunta.AlternativaA = alternativas[0];
            pergunta.AlternativaB = alternativas[1];
            pergunta.AlternativaC = alternativas[2];
            pergunta.AlternativaD = alternativas[3];

            pergunta.RespostaCorreta = LerResposta();

            pergunta.Explicacao = LerTextoObrigatorio(
                "Explicação: ", 1000);

            pergunta.Pontos = LerInteiro(
                "Pontuação: ", 1, 100);

            context.Perguntas.Add(pergunta);
            context.SaveChanges();

            Console.WriteLine("\nPergunta cadastrada com sucesso!");
        }

        // LISTAR PERGUNTAS
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

        // ATUALIZAR PERGUNTA
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

            string enunciado = LerTextoObrigatorio(
                "Novo enunciado: ", 500);

            string[] alternativas = LerAlternativasDiferentes();

            string resposta = LerResposta();

            string explicacao = LerTextoObrigatorio(
                "Nova explicação: ", 1000);

            int pontos = LerInteiro(
                "Nova pontuação: ", 1, 100);

            pergunta.Enunciado = enunciado;
            pergunta.AlternativaA = alternativas[0];
            pergunta.AlternativaB = alternativas[1];
            pergunta.AlternativaC = alternativas[2];
            pergunta.AlternativaD = alternativas[3];
            pergunta.RespostaCorreta = resposta;
            pergunta.Explicacao = explicacao;
            pergunta.Pontos = pontos;

            context.SaveChanges();

            Console.WriteLine("\nPergunta atualizada com sucesso!");
        }

        // EXCLUIR PERGUNTA
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
                .Trim()
                .ToUpperInvariant();

            if (resposta != "S")
            {
                Console.WriteLine("Exclusão cancelada.");
                return;
            }

            context.Perguntas.Remove(pergunta);
            context.SaveChanges();

            Console.WriteLine("Pergunta excluída com sucesso!");
        }

        // RANKING
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
