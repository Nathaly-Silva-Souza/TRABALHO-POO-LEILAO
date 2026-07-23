using System;
using System.Collections.Generic;

namespace SimuladorLeilao.Models
{
    // Classe do Item do Leilão
    public class ItemLeilao
    {
        public int Id { get; set; }

        public int IdItem 
        { 
            get => Id; 
            set => Id = value; 
        }

        public string Nome { get; set; }
        public double ValorInicial { get; set; }
        public double ValorAtual { get; set; }
        public double ValorMaximo { get; set; }
        public string Status { get; set; }
        public int TurnosRestantes { get; set; }
        public List<Lance> Lances { get; set; }

        public ItemLeilao(int id, string nome, double valorInicial)
        {
            Id = id;
            Nome = nome;
            ValorInicial = valorInicial;
            ValorAtual = valorInicial;
            ValorMaximo = valorInicial * 2; // Define um valor máximo automático para arremate
            Status = "Ativo";
            TurnosRestantes = 5;
            Lances = new List<Lance>();
        }

        // Método que aceita o objeto 'Lance' (usado pelo LanceController)
        public bool AdicionarLance(Lance lance)
        {
            if (lance == null) return false;

            if (lance.Valor > ValorAtual)
            {
                ValorAtual = lance.Valor;
                Lances.Add(lance);
                return true;
            }
            return false;
        }

        // Sobrecargas para evitar erros de compilação
        public bool AdicionarLance(double valor, string comprador = "")
        {
            return AdicionarLance(new Lance(comprador, valor));
        }

        public void DecrementarTurno()
        {
            if (TurnosRestantes > 0)
            {
                TurnosRestantes--;
                if (TurnosRestantes == 0)
                {
                    Status = "Finalizado";
                }
            }
        }

        // Faz os itens aparecerem legíveis quando usar o comando 'ls'
        public override string ToString()
        {
            return $"[ID: {Id}] {Nome} | Valor Atual: R$ {ValorAtual:F2} | Status: {Status} | Turnos Restantes: {TurnosRestantes}";
        }
    }

    // Classe da Sala do Leilão
    public class SalaLeilao
    {
        public string Nome { get; set; }
        public List<ItemLeilao> Itens { get; set; }

        public SalaLeilao(string nome)
        {
            Nome = nome;
            Itens = new List<ItemLeilao>();
        }
    }
}