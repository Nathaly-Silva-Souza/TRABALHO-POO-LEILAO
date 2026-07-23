using System;
using System.Linq;
using SimuladorLeilao.Utils;
using SimuladorLeilao.Views;
using SimuladorLeilao.Models;

namespace SimuladorLeilao.Controllers
{
    public class MainController : IController
    {
        private readonly ContextoSistema _contexto;
        private readonly ViewMenu _view;

        public MainController(ContextoSistema contexto)
        {
            _contexto = contexto;
            _view = new ViewMenu(); 
        }

        public void Run()
        {
            bool executando = true;
            while (executando)
            {
                string diretorio = _contexto.Pilha.Peek();
                string comando = _view.ExibirMenuPrincipal(diretorio, _contexto.SaldoComprador);

                string[] partes = comando.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 0) continue;

                switch (partes[0])
                {
                    case "cd":
                        if (partes.Length > 1 && partes[1] == "..") _contexto.Pilha.Pop();
                        else if (partes.Length > 1 && _contexto.Salas.ContainsKey(partes[1])) _contexto.Pilha.Push(partes[1]);
                        else _view.MostrarMensagem("Sala inexistente.", ConsoleColor.Red);
                        break;
                    
                    case "ls":
                        if (diretorio == "raiz") _view.MostrarMensagem("Categorias: carros | reliquias | artes");
                        else _view.MostrarItens(_contexto.Salas[diretorio].Itens);
                        break;
                    
                    case "cadastrar":
                        if (diretorio == "raiz")
                        {
                            _view.MostrarMensagem("Erro: Você precisa entrar em uma sala (ex: cd carros) para cadastrar um item!", ConsoleColor.Red);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"=== CADASTRAR NOVO ITEM NA SALA: {diretorio.ToUpper()} ===");
                            
                            Console.Write("Nome do item: ");
                            string nomeItem = Console.ReadLine() ?? "";
                            
                            Console.Write("Valor inicial (Ex: 5000): ");
                            double valorInicial;
                            double.TryParse(Console.ReadLine(), out valorInicial);

                            int novoId = _contexto.Salas[diretorio].Itens.Count + 1;

                            // Cria o objeto e adiciona na sala
                            var novoItem = new ItemLeilao(novoId, nomeItem, valorInicial);
                            _contexto.Salas[diretorio].Itens.Add(novoItem);

                            _view.MostrarMensagem($"Item '{nomeItem}' cadastrado com sucesso por R$ {valorInicial}!", ConsoleColor.Yellow);
                        }
                        break;

                    case "lance":
                        IController lanceController = new LanceController(_contexto, _view);
                        lanceController.Run(); 
                        break;
                    
                    case "sair":
                        executando = false;
                        _view.MostrarMensagem("Encerrando...", ConsoleColor.Cyan);
                        break;
                    
                    default:
                        _view.MostrarMensagem("Comando não reconhecido.", ConsoleColor.DarkYellow);
                        break;
                }
                AtualizarTurnosGlobais();
            }
        }

        private void AtualizarTurnosGlobais()
        {
            foreach (var sala in _contexto.Salas.Values)
            {
                foreach (var item in sala.Itens)
                {
                    if (item.Status == "Ativo") item.DecrementarTurno();
                }
            }
        }
    }
}