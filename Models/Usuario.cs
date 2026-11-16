
using System;
using System.Linq;
using PI_1anoDS.Data;


namespace PL_1AnoDS.Models
{
    public class Usuario
    {
        
        private int id;
        private string nome = "";
        private string email = "";

        
        public Usuario()
        {
        }

        public Usuario(int id, string nome, string email)
        {
            this.id = id;
            Nome = nome;
            Email = email;
        }

        
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Nome
        {
            get { return nome; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("O nome não pode ser vazio.");
                }
                else
                {
                    nome = value.Trim();
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) ||
                    !value.Contains("@") ||
                    !value.Contains("."))
                {
                    Console.WriteLine("E-mail inválido.");
                }
                else
                {
                    email = value.Trim().ToLower();
                }
            }
        }

        
        private bool ExisteEmail(AppDnContext context, string email)
        {
            return context.Usuarios.Any(
                u => u.Email == email
            );
        }

        
        public void CadastrarUsuario(AppDnContext context)
        {
            while (true)
            {
                Console.Write("Digite seu nome: ");
                string nome = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("Nome inválido. Tente novamente.");
                    continue;
                }

                Nome = nome;
                break;
            }

            while (true)
            {
                Console.Write("Digite seu e-mail: ");
                string email = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(email) ||
                    !email.Contains("@") ||
                    !email.Contains("."))
                {
                    Console.WriteLine("E-mail inválido. Tente novamente.");
                    continue;
                }

                Email = email;

                if (ExisteEmail(context, Email))
                {
                    Console.WriteLine("Este e-mail já está cadastrado.");
                    continue;
                }

                break;
            }

            
            context.Usuarios.Add(this);
            context.SaveChanges();

            Console.WriteLine("Usuário cadastrado com sucesso!");
        }

        
        public void LerUsuarios(AppDnContext context)
        {
            var lista = context.Usuarios.ToList();

            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                return;
            }

            Console.WriteLine("\nUsuários cadastrados:");

            foreach (var usuario in lista)
            {
                Console.WriteLine(
                    $"ID: {usuario.Id} | Nome: {usuario.Nome} | E-mail: {usuario.Email}"
                );
            }
        }

        
        public void BuscarUsuario(AppDnContext context)
        {
            Console.Write("Digite o nome do usuário: ");
            string nome = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            var usuario = context.Usuarios
                .FirstOrDefault(u => u.Nome.Contains(nome));

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            Console.WriteLine(
                $"ID: {usuario.Id} | Nome: {usuario.Nome} | E-mail: {usuario.Email}"
            );
        }

        
        public void ExcluirUsuario(AppDnContext context)
        {
            LerUsuarios(context);

            Console.Write("\nDigite o ID do usuário que deseja excluir: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            context.Usuarios.Remove(usuario);
            context.SaveChanges();

            Console.WriteLine("Usuário excluído com sucesso!");
        }

        
        public void AtualizarUsuario(AppDnContext context)
        {
            LerUsuarios(context);

            Console.Write("\nDigite o ID do usuário: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            while (true)
            {
                Console.WriteLine("O que deseja atualizar? ");
                Console.WriteLine("1 - Nome");
                Console.WriteLine("2 - E-mail");
                Console.WriteLine("3 - Sair");
                Console.Write("Escolha: ");

                string opcao = Console.ReadLine() ?? "";

                switch (opcao)
                {
                    case "1":
                        Console.Write("Digite o novo nome: ");
                        string novoNome = Console.ReadLine() ?? "";

                        if (string.IsNullOrWhiteSpace(novoNome))
                        {
                            Console.WriteLine("Nome inválido.");
                            continue;
                        }

                        usuario.Nome = novoNome;
                        context.SaveChanges();

                        Console.WriteLine("Nome atualizado com sucesso!");
                        return;

                    case "2":
                        Console.Write("Digite o novo e-mail: ");
                        string novoEmail = Console.ReadLine() ?? "";

                        if (string.IsNullOrWhiteSpace(novoEmail) ||
                            !novoEmail.Contains("@") ||
                            !novoEmail.Contains("."))
                        {
                            Console.WriteLine("E-mail inválido.");
                            continue;
                        }

                        novoEmail = novoEmail.Trim().ToLower();

                        if (ExisteEmail(context, novoEmail))
                        {
                            Console.WriteLine("Este e-mail já está cadastrado.");
                            continue;
                        }

                        usuario.Email = novoEmail;
                        context.SaveChanges();

                        Console.WriteLine("E-mail atualizado com sucesso!");
                        return;

                    case "3":
                        Console.WriteLine("Saindo...");
                        return;

                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
        }
    }
}
