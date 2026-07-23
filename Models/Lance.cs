using System;

namespace SimuladorLeilao.Models
{
    public class Lance
    {
        public string NomePessoa { get; set; }
        public double Valor { get; set; }
        public DateTime DataHora { get; set; }

        public Lance(string nomePessoa, double valor)
        {
            NomePessoa = nomePessoa;
            Valor = valor;
            DataHora = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{NomePessoa} deu um lance de R$ {Valor:F2} às {DataHora:HH:mm:ss}";
        }
    }
}