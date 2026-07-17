using System;
using System.Collections.Generic;

namespace SimuladorLeilao
{
    // Classe Mãe (Base)
    public abstract class ItemLeilao
    {
        public int IdItem { get; set; }
        public string Nome { get; set; }
        public double ValorMinimo { get; set; }
        public double ValorMaximo { get; set; }
        public double ValorAtual { get; set; }
        public int TurnosRestantes { get; set; }
        public string Status { get; set; } // "Ativo", "Vendido", "Retirado"
        public string CompradorVencedor { get; set; }

        protected ItemLeilao(int idItem, string nome, double valorMinimo, double valorMaximo, int turnos)
        {
            IdItem = idItem;
            Nome = nome;
            ValorMinimo = valorMinimo;
            ValorMaximo = valorMaximo;
            ValorAtual = valorMinimo;
            TurnosRestantes = turnos;
            Status = "Ativo";
            CompradorVencedor = "Nenhum";
        }

        public void DecrementarTurno()
        {
            if (Status == "Ativo")
            {
                TurnosRestantes--;
                if (TurnosRestantes <= 0)
                {
                    Status = ValorAtual > ValorMinimo ? "Vendido" : "Retirado";
                }
            }
        }

        public override string ToString()
        {
            return $"[{IdItem}] {Nome} - Status: {Status} - Preço Atual: R$ {ValorAtual:F2} (Turnos restantes: {TurnosRestantes})";
        }
    }

    // Subclasse: Carro Clássico (Herança)
    public class CarroClassico : ItemLeilao
    {
        public int Quilometragem { get; set; }
        public int AnoFabricacao { get; set; }

        public CarroClassico(int idItem, string nome, double valorMinimo, double valorMaximo, int turnos, int quilometragem, int anoFabricacao)
            : base(idItem, nome, valorMinimo, valorMaximo, turnos)
        {
            Quilometragem = quilometragem;
            AnoFabricacao = anoFabricacao;
        }

        public override string ToString()
        {
            return base.ToString() + $" | [Carro] Km: {Quilometragem} | Ano: {AnoFabricacao}";
        }
    }

    // Subclasse: Relíquia Histórica (Herança)
    public class ReliquiaHistorica : ItemLeilao
    {
        public int AnoOrigem { get; set; }
        public string EstadoConservacao { get; set; }

        public ReliquiaHistorica(int idItem, string nome, double valorMinimo, double valorMaximo, int turnos, int anoOrigem, string estadoConservacao)
            : base(idItem, nome, valorMinimo, valorMaximo, turnos)
        {
            AnoOrigem = anoOrigem;
            EstadoConservacao = estadoConservacao;
        }

        public override string ToString()
        {
            return base.ToString() + $" | [Relíquia] Origem: {AnoOrigem} | Conservação: {EstadoConservacao}";
        }
    }

    // Subclasse: Obra de Arte (Herança)
    public class ObraDeArte : ItemLeilao
    {
        public string Artista { get; set; }
        public string Tecnica { get; set; }

        public ObraDeArte(int idItem, string nome, double valorMinimo, double valorMaximo, int turnos, string artista, string tecnica)
            : base(idItem, nome, valorMinimo, valorMaximo, turnos)
        {
            Artista = artista;
            Tecnica = tecnica;
        }

        public override string ToString()
        {
            return base.ToString() + $" | [Obra de Arte] Artista: {Artista} | Técnica: {Tecnica}";
        }
    }

    // Sala de Leilão
    public class SalaLeilao
    {
        public string NomeSala { get; set; }
        public List<ItemLeilao> Itens { get; set; }

        public SalaLeilao(string nomeSala)
        {
            NomeSala = nomeSala;
            Itens = new List<ItemLeilao>();
        }

        public void AdicionarItem(ItemLeilao item)
        {
            Itens.Add(item);
        }

        public bool RemoverItem(int idItem)
        {
            var item = Itens.Find(i => i.IdItem == idItem);
            if (item != null)
            {
                Itens.Remove(item);
                return true;
            }
            return false;
        }
    }
}