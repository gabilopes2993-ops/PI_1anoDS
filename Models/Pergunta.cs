
public class Pergunta
{
    public int IdPergunta { get; set; }
    public string Enunciado { get; set; }
    public string AlternativaA { get; set; }
    public string AlternativaB { get; set; }
    public string AlternativaC { get; set; }
    public string AlternativaD { get; set; }
    public string RespostaCorreta { get; set; }
    public string Explicacao { get; set; }
    public int Pontos { get; set; }

    public Pergunta()
    {
    }

    public Pergunta(int idPergunta, string enunciado,
        string alternativaA, string alternativaB,
        string alternativaC, string alternativaD,
        string respostaCorreta, string explicacao, int pontos)
    {
        IdPergunta = idPergunta;
        Enunciado = enunciado;
        AlternativaA = alternativaA;
        AlternativaB = alternativaB;
        AlternativaC = alternativaC;
        AlternativaD = alternativaD;
        RespostaCorreta = respostaCorreta;
        Explicacao = explicacao;
        Pontos = pontos;
    }
}
