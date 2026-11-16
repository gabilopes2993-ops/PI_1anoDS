using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PI_1anoDS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Perguntas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Enunciado = table.Column<string>(type: "longtext", nullable: false),
                    AlternativaA = table.Column<string>(type: "longtext", nullable: false),
                    AlternativaB = table.Column<string>(type: "longtext", nullable: false),
                    AlternativaC = table.Column<string>(type: "longtext", nullable: false),
                    AlternativaD = table.Column<string>(type: "longtext", nullable: false),
                    RespostaCorreta = table.Column<string>(type: "longtext", nullable: false),
                    Explicacao = table.Column<string>(type: "longtext", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perguntas", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ranking",
                columns: table => new
                {
                    IdRanking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Pontuacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranking", x => x.IdRanking);
                    table.ForeignKey(
                        name: "FK_Ranking_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Perguntas",
                columns: new[] { "Id", "AlternativaA", "AlternativaB", "AlternativaC", "AlternativaD", "Enunciado", "Explicacao", "Pontos", "RespostaCorreta" },
                values: new object[,]
                {
                    { 1, "Um antivírus", "Um golpe para roubar informações", "Um sistema operacional", "Um navegador", "O que é phishing?", "", 10, "B" },
                    { 2, "Para facilitar o acesso", "Para compartilhar contas", "Para dificultar invasões", "Para evitar atualizações", "Por que devemos utilizar senhas fortes?", "", 10, "C" },
                    { 3, "Uma senha com dois números", "Uma camada extra de segurança que exige duas formas de verificação", "Um programa antivírus", "Um tipo de firewall", "O que é autenticação de dois fatores (2FA)?", "", 10, "B" },
                    { 4, "Clicar para verificar o conteúdo", "Encaminhar para todos os contatos", "Não clicar e verificar a autenticidade do remetente", "Informar nossos dados pessoais", "O que devemos fazer ao receber um e-mail suspeito com um link?", "", 10, "C" },
                    { 5, "Criar senhas automaticamente", "Proteger o computador contra softwares maliciosos", "Aumentar a velocidade da internet", "Compartilhar arquivos", "Qual é a principal função de um antivírus?", "", 10, "B" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ranking_UsuarioId",
                table: "Ranking",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perguntas");

            migrationBuilder.DropTable(
                name: "Ranking");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
