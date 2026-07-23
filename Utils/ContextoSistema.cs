using System.Collections.Generic;
using SimuladorLeilao.Models;

namespace SimuladorLeilao.Utils
{
    public class ContextoSistema
    {
        public Dictionary<string, SalaLeilao> Salas { get; set; }
        public IPilha Pilha { get; set; }
        public double SaldoComprador { get; set; }
        public int ProximoId { get; set; }

        public ContextoSistema()
        {
            Pilha = new PilhaNavegacao();
            SaldoComprador = 500000.00;
            ProximoId = 1;
            Salas = new Dictionary<string, SalaLeilao>
            {
                { "carros", new SalaLeilao("Carros Clássicos") },
                { "reliquias", new SalaLeilao("Relíquias Históricas") },
                { "artes", new SalaLeilao("Obras de Arte") }
            };
        }
    }
}