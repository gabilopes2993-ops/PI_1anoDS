
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PI_1anoDS.Data;
using PI_1anoDS.Models;
using PI_1AnoDS.Models;

namespace PI_1anoDS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    Console.WriteLine("================================");
                    Console.WriteLine("       NAVEGASEGURO");
                    Console.WriteLine("================================");

                    
                    Console.WriteLine("\nTestando conexão com o banco...");

                    bool conectado = context.Database.CanConnect();

                    if (!conectado)
                    {
                        Console.WriteLine("Não foi possível conectar ao banco.");
                        return;
                    }

                    Console.WriteLine("Conexão realizada com sucesso!");

                    int opcao;

                    do
                    {
                        Console.WriteLine("\n========== MENU ==========");
                        Console.WriteLine("1 - Cadastrar usuário");
                        Console.WriteLine("2 - Listar usuários");
                        Console.WriteLine("3 - Buscar usuário");
                        Console.WriteLine("4 - Atualizar usuário");
                        Console.WriteLine("5 - Excluir usuário");
                        Console.WriteLine("6 - Mostrar ranking");
                        Console.WriteLine("0 - Sair");
                        Console.Write("\nEscolha: ");

                        if (!int.TryParse(Console.ReadLine(), out opcao))
                        {
                            Console.WriteLine("Digite uma opção válida.");
                            continue;
                        }

                        Usuario usuario = new Usuario();

                        switch (opcao)
                        {
                            case 1:
                                usuario.CadastrarUsuario(context);
                                break;

                            case 2:
                                usuario.LerUsuarios(context);
                                break;

                            case 3:
                                usuario.BuscarUsuario(context);
                                break;

                            case 4:
                                usuario.AtualizarUsuario(context);
                                break;

                            case 5:
                                usuario.ExcluirUsuario(context);
                                break;

                            case 6:
                                Ranking ranking = new Ranking();
                                ranking.MostrarRanking();
                                break;

                            case 0:
                                Console.WriteLine("Encerrando o NavegaSeguro...");
                                break;

                            default:
                                Console.WriteLine("Opção inválida.");
                                break;
                        }

                    } while (opcao != 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nOcorreu um erro:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nPressione qualquer tecla para sair.");
            Console.ReadKey();
        }
    }
}
