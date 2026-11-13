using System.Web;
using Microsoft.EntityFrameworkCore;
namespace Models;

public class Usuario
{
    private int? id;
    private string? nome;
    private string? email;


    public int? Id
    {
        get { return id; }
        set { id = value; }
    }

    public string? Nome
    {
        get { return nome; }
        set { nome = value; }
    }

    public string? Email
    {
        get { return email; }
        set { email = value; }
    }
    

    public Usuario(int? id, string? nome, string? email)
    {
        this.id = id;
        this.nome = nome;
        this.email = email;
    }

    public void DefinirId(int? id)
    {
        if (novoId > 0)
        {
            this.id = id;
        }
        else
        {
            throw new ArgumentException("O ID deve ser maior que zero.");
        }
    }
}
