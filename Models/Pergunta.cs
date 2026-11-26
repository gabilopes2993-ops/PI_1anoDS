
using System.ComponentModel.DataAnnotations;

namespace PI_1anoDS.Models
{
    
    public class Pergunta
    {
        
        public int Id { get; set; }

        public string Enunciado { get; set; } = string.Empty;
        public string AlternativaA { get; set; } = string.Empty;
        public string AlternativaB { get; set; } = string.Empty;
        public string AlternativaC { get; set; } = string.Empty;
        public string AlternativaD { get; set; } = string.Empty;
        public string RespostaCorreta { get; set; } = string.Empty;
        public string Explicacao { get; set; } = string.Empty;
        public int Pontos { get; set; }

        public Pergunta()
        {
        }
    }
}
