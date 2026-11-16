using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PI_1anoDS.Data;


namespace PL_1AnoDS.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public Usuario() { }

        public Usuario(int id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        
        private static bool ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("O nome não pode ficar vazio.");
                return false;
            }

            nome = nome.Trim();

            if (nome.Length < 2 || nome.Length > 100)
            {
                Console.WriteLine("O nome deve ter entre 2 e 100 caracteres.");
                return false;
            }

            if (nome.Any(char.IsDigit))
            {
                Console.WriteLine("O nome não pode conter números.");
                return false;
            }

            if (!nome.All(c =>
                char.IsLetter(c) ||
                char.IsWhiteSpace(c) ||
                c == '-' ||
                c == '\''))
            {
                Console.WriteLine("O nome contém caracteres inválidos.");
                return false;
            }

            return true;
        }

        
        private static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("O e-mail não pode ficar vazio.");
                return false;
            }

            email = email.Trim();

            if (email.Length > 150)
            {
                Console.WriteLine("O e-mail deve ter no máximo 150 caracteres.");
                return false;
            }

            try
            {
                var endereco = new MailAddress(email);

                if (endereco.Address != email ||
                    !email.Contains('.') ||
                    email.StartsWith("@") ||
                    email.EndsWith("@"))
                {
                    Console.WriteLine("Digite um e-mail válido.");
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Digite um e-mail válido.");
                return false;
            }

            return true;
        }

        
        private static bool EmailExiste(
            AppDbContext context,
            string email,
            int idIgnorado = 0)
        {
            string emailNormalizado = email.Trim().ToLower();

            return context.Usuarios.Any(u =>
                u.Id != idIgnorado &&
                u.Email.ToLower() == emailNormalizado);
        }

        
        public void CadastrarUsuario(AppDbContext context)
        {
            Console.WriteLine("\n===== CADASTRO DE USUÁRIO =====");

            string nome;

            do
            {
                Console.Write("Digite seu nome: ");
                nome = Console.ReadLine() ?? "";

            } while (!ValidarNome(nome));

            string email;

            while (true)
            {
                Console.Write("Digite seu e-mail: ");
                email = (Console.ReadLine() ?? "").Trim().ToLower();

                if (!ValidarEmail(email))
                    continue;

                if (EmailExiste(context, email))
                {
                    Console.WriteLine("Este e-mail já está cadastrado.");
                    continue;
                }

                break;
            }

            Nome = nome.Trim();
            Email = email;

            context.Usuarios.Add(this);
            context.SaveChanges();

            Console.WriteLine("\nUsuário cadastrado com sucesso!");
        }

        
        public void LerUsuarios(AppDbContext context)
        {
            Console.WriteLine("\n===== USUÁRIOS CADASTRADOS =====");

            var usuarios = context.Usuarios
                .AsNoTracking()
                .OrderBy(u => u.Nome)
                .ToList();

            if (usuarios.Count == 0)
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                return;
            }

            foreach (var usuario in usuarios)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"ID: {usuario.Id}");
                Console.WriteLine($"Nome: {usuario.Nome}");
                Console.WriteLine($"E-mail: {usuario.Email}");
            }
        }

        
        public Usuario? BuscarUsuario(AppDbContext context)
        {
            Console.WriteLine("\n===== BUSCAR USUÁRIO =====");
            Console.Write("Digite o e-mail: ");

            string email = (Console.ReadLine() ?? "").Trim().ToLower();

            if (!ValidarEmail(email))
                return null;

            Usuario? usuario = context.Usuarios
                .AsNoTracking()
                .FirstOrDefault(u => u.Email.ToLower() == email);

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return null;
            }

            Console.WriteLine("\nUsuário encontrado!");
            Console.WriteLine($"ID: {usuario.Id}");
            Console.WriteLine($"Nome: {usuario.Nome}");
            Console.WriteLine($"E-mail: {usuario.Email}");

            return usuario;
        }

        
        public void ExcluirUsuario(AppDbContext context)
        {
            Console.WriteLine("\n===== EXCLUIR USUÁRIO =====");
            Console.Write("Digite o ID do usuário: ");

            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            Usuario? usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            Console.WriteLine($"Nome: {usuario.Nome}");
            Console.WriteLine($"E-mail: {usuario.Email}");

            Console.Write("Confirma a exclusão? (S/N): ");
            string confirmacao = (Console.ReadLine() ?? "").Trim().ToUpper();

            if (confirmacao != "S")
            {
                Console.WriteLine("Exclusão cancelada.");
                return;
            }

            context.Usuarios.Remove(usuario);
            context.SaveChanges();

            Console.WriteLine("Usuário excluído com sucesso!");
        }

        
        public void AtualizarUsuario(AppDbContext context)
        {
            Console.WriteLine("\n===== ATUALIZAR USUÁRIO =====");
            Console.Write("Digite o ID do usuário: ");

            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            Usuario? usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            Console.WriteLine($"Nome atual: {usuario.Nome}");
            Console.WriteLine($"E-mail atual: {usuario.Email}");

            // ATUALIZAR NOME
            while (true)
            {
                Console.Write("\nNovo nome: ");
                string novoNome = Console.ReadLine() ?? "";

                if (ValidarNome(novoNome))
                {
                    usuario.Nome = novoNome.Trim();
                    break;
                }
            }

            
            while (true)
            {
                Console.Write("Novo e-mail: ");
                string novoEmail = (Console.ReadLine() ?? "")
                    .Trim()
                    .ToLower();

                if (!ValidarEmail(novoEmail))
                    continue;

                if (EmailExiste(context, novoEmail, usuario.Id))
                {
                    Console.WriteLine("Este e-mail já pertence a outro usuário.");
                    continue;
                }

                usuario.Email = novoEmail;
                break;
            }

            context.SaveChanges();

            Console.WriteLine("\nUsuário atualizado com sucesso!");
        }
    }
}
