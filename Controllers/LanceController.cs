using System;
using SimuladorLeilao.Utils;
using SimuladorLeilao.Views;
using SimuladorLeilao.Models;

namespace SimuladorLeilao.Controllers
{
    public class LanceController : IController
    {
        private readonly ContextoSistema _contexto;
        private readonly ViewLance _viewLance;
        private readonly ViewMenu _viewMenu; // Para mensagens de erro/sucesso

        public LanceController(ContextoSistema contexto, ViewMenu viewMenu)
        {
            _contexto = contexto;
            _viewLance = new ViewLance();
            _viewMenu = viewMenu;
        }

        public void Run()
        {
            string salaAtual = _contexto.Pilha.Peek();
            if (salaAtual == "raiz")
            {
                _viewMenu.MostrarMensagem("Você precisa estar em uma sala para dar um lance!", ConsoleColor.Red);
                return;
            }

            try
            {
                var dados = _viewLance.ObterDadosLance(); // View imprime e espera
                var item = _contexto.Salas[salaAtual].Itens.Find(i => i.IdItem == dados.idItem);

                if (item == null || item.Status != "Ativo")
                {
                    _viewMenu.MostrarMensagem("Item não encontrado ou leilão encerrado.", ConsoleColor.Red);
                    return;
                }

                if (dados.valorLance > _contexto.SaldoComprador)
                {
                    _viewMenu.MostrarMensagem("Saldo insuficiente!", ConsoleColor.Red);
                    return;
                }

                if (dados.valorLance <= item.ValorAtual)
                {
                    _viewMenu.MostrarMensagem($"Lance inválido. Deve ser maior que R$ {item.ValorAtual:F2}", ConsoleColor.Red);
                    return;
                }

                // Cria o modelo de Lance e envia para a lógica do Leilão
                Lance novoLance = new Lance(dados.nomePessoa, dados.valorLance);
                item.AdicionarLance(novoLance);
                
                // Regra de Arremate imediato
                if (dados.valorLance >= item.ValorMaximo)
                {
                    item.Status = "Vendido";
                    _contexto.SaldoComprador -= dados.valorLance;
                    _viewMenu.MostrarMensagem($"🔥 PARABÉNS {dados.nomePessoa.ToUpper()}! Arrematou pelo valor máximo!", ConsoleColor.Green);
                }
                else
                {
                    _viewMenu.MostrarMensagem("Lance aceito com sucesso!", ConsoleColor.Green);
                }
            }
            catch (Exception)
            {
                _viewMenu.MostrarMensagem("Erro: Dados de entrada inválidos.", ConsoleColor.Red);
            }
        }
    }
}