namespace PI_1anoDS.Models
{
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
    }
}