using System;
using System.Collections.Generic;

namespace SimuladorLeilao
{
    public class ControllerLeilao
    {
        private readonly ViewLeilao _view;               // Registro da View (MVC)
        private readonly IPilha _pilha;                 // Injeção de dependência da Pilha
        private readonly Dictionary<string, SalaLeilao> _salas; // Dados do Model
        private double _saldoComprador;
        private int _proximoId;

        public ControllerLeilao(ViewLeilao view, IPilha pilha)
        {
            _view = view;
            _pilha = pilha;
            _saldoComprador = 500000.00;
            _proximoId = 1;

            _salas = new Dictionary<string, SalaLeilao>
            {
                { "carros", new SalaLeilao("Carros Clássicos") },
                { "reliquias", new SalaLeilao("Relíquias Históricas") },
                { "artes", new SalaLeilao("Obras de Arte") }
            };

            InicializarDados();
        }

        private void InicializarDados()
        {
            _salas["carros"].AdicionarItem(new CarroClassico(_proximoId++, "Mustang 1969", 120000, 250000, 5, 45000, 1969));
            _salas["reliquias"].AdicionarItem(new ReliquiaHistorica(_proximoId++, "Moeda Império Romano", 5000, 15000, 7, 300, "Excelente"));
        }

        public void Run()
        {
            while (true)
            {
                string diretorioAtual = _pilha.Peek();
                string comando = _view.ExibirMenuPrincipal(diretorioAtual, _saldoComprador);

                if (comando == "sair")
                {
                    _view.MostrarMensagem("Encerrando o simulador de leilão. Até logo!");
                    break;
                }

                ProcessarComando(comando);
            }
        }

        private void ProcessarComando(string comando)
        {
            string[] partes = comando.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) return;

            string acao = partes[0];

            switch (acao)
            {
                case "cd":
                    if (partes.Length < 2)
                    {
                        _view.MostrarMensagem("Use: cd [nome_da_sala] ou cd ..");
                        return;
                    }
                    string destino = partes[1];
                    if (destino == "..")
                    {
                        _pilha.Pop();
                    }
                    else if (_salas.ContainsKey(destino))
                    {
                        _pilha.Push(destino);
                    }
                    else
                    {
                        _view.MostrarMensagem("Sala/Categoria inexistente!");
                    }
                    break;

                case "ls":
                    string salaAtual = _pilha.Peek();
                    if (salaAtual == "raiz")
                    {
                        _view.MostrarMensagem("Categorias disponíveis: carros | reliquias | artes");
                    }
                    else
                    {
                        _view.MostrarItens(_salas[salaAtual].Itens);
                    }
                    break;

                case "cadastrar":
                    CadastrarItem();
                    break;

                case "atualizar":
                    AtualizarItem();
                    break;

                case "remover":
                    RemoverItem();
                    break;

                case "lance":
                    DarLance(partes);
                    break;

                default:
                    _view.MostrarMensagem("Comando não reconhecido.");
                    break;
            }
        }

        private void CadastrarItem()
        {
            string salaAtual = _pilha.Peek();
            if (salaAtual == "raiz")
            {
                _view.MostrarMensagem("Você precisa entrar em uma sala específica para cadastrar um item!");
                return;
            }

            try
            {
                var dados = _view.ObterDadosCadastro(salaAtual);
                ItemLeilao novoItem = null;

                if (salaAtual == "carros")
                {
                    novoItem = new CarroClassico(_proximoId++, dados.nome, dados.valMin, dados.valMax, dados.turnos,
                        Convert.ToInt32(dados.especificos["km"]), Convert.ToInt32(dados.especificos["ano"]));
                }
                else if (salaAtual == "reliquias")
                {
                    novoItem = new ReliquiaHistorica(_proximoId++, dados.nome, dados.valMin, dados.valMax, dados.turnos,
                        Convert.ToInt32(dados.especificos["ano"]), dados.especificos["conservacao"]);
                }
                else if (salaAtual == "artes")
                {
                    novoItem = new ObraDeArte(_proximoId++, dados.nome, dados.valMin, dados.valMax, dados.turnos,
                        dados.especificos["artista"], dados.especificos["tecnica"]);
                }

                if (novoItem != null)
                {
                    _salas[salaAtual].AdicionarItem(novoItem);
                    _view.MostrarMensagem("Item cadastrado com sucesso!");
                    AtualizarTurnos();
                }
            }
            catch (Exception ex)
            {
                _view.MostrarMensagem($"Erro ao cadastrar item: {ex.Message}");
            }
        }

        private void AtualizarItem()
        {
            string salaAtual = _pilha.Peek();
            if (salaAtual == "raiz")
            {
                _view.MostrarMensagem("Navegue até a sala do item para atualizá-lo.");
                return;
            }

            try
            {
                Console.Write("ID do Item a atualizar: ");
                int idBusca = Convert.ToInt32(Console.ReadLine());
                var item = _salas[salaAtual].Itens.Find(i => i.IdItem == idBusca);

                if (item != null)
                {
                    Console.Write($"Novo valor mínimo (Atual: R$ {item.ValorMinimo:F2}): ");
                    item.ValorMinimo = Convert.ToDouble(Console.ReadLine());

                    Console.Write($"Novo valor máximo (Atual: R$ {item.ValorMaximo:F2}): ");
                    item.ValorMaximo = Convert.ToDouble(Console.ReadLine());

                    _view.MostrarMensagem("Item updated!");
                    AtualizarTurnos();
                }
                else
                {
                    _view.MostrarMensagem("Item não encontrado.");
                }
            }
            catch
            {
                _view.MostrarMensagem("Dados inválidos.");
            }
        }

        private void RemoverItem()
        {
            string salaAtual = _pilha.Peek();
            if (salaAtual == "raiz")
            {
                _view.MostrarMensagem("Navegue até a sala do item para removê-lo.");
                return;
            }

            try
            {
                Console.Write("ID do Item a ser removido: ");
                int idBusca = Convert.ToInt32(Console.ReadLine());
                if (_salas[salaAtual].RemoverItem(idBusca))
                {
                    _view.MostrarMensagem("Item removido com sucesso!");
                    AtualizarTurnos();
                }
                else
                {
                    _view.MostrarMensagem("Item não encontrado.");
                }
            }
            catch
            {
                _view.MostrarMensagem("ID inválido.");
            }
        }

        private void DarLance(string[] partes)
        {
            if (partes.Length < 3)
            {
                _view.MostrarMensagem("Use: lance [ID] [VALOR]");
                return;
            }

            string salaAtual = _pilha.Peek();
            if (salaAtual == "raiz")
            {
                _view.MostrarMensagem("Você precisa estar em uma sala de itens para dar um lance.");
                return;
            }

            try
            {
                int idItem = Convert.ToInt32(partes[1]);
                double valorLance = Convert.ToDouble(partes[2]);

                var item = _salas[salaAtual].Itens.Find(i => i.IdItem == idItem);

                if (item == null)
                {
                    _view.MostrarMensagem("Item não encontrado na sala atual.");
                    return;
                }

                if (item.Status != "Ativo")
                {
                    _view.MostrarMensagem("Este lote já está encerrado!");
                    return;
                }

                if (valorLance > _saldoComprador)
                {
                    _view.MostrarMensagem("Saldo insuficiente para dar esse lance.");
                    return;
                }

                if (valorLance <= item.ValorAtual)
                {
                    _view.MostrarMensagem($"O lance deve ser maior do que o preço atual (R$ {item.ValorAtual:F2}).");
                    return;
                }

                if (valorLance >= item.ValorMaximo)
                {
                    item.ValorAtual = valorLance;
                    item.Status = "Vendido";
                    item.CompradorVencedor = "Jogador";
                    _saldoComprador -= valorLance;
                    _view.MostrarMensagem($"🔥 PARABÉNS! Você atingiu o valor máximo de R$ {valorLance:F2} e arrematou o item '{item.Nome}' imediatamente!");
                }
                else
                {
                    item.ValorAtual = valorLance;
                    item.CompradorVencedor = "Jogador";
                    _view.MostrarMensagem($"Lance de R$ {valorLance:F2} aceito com sucesso!");
                }

                AtualizarTurnos();
            }
            catch
            {
                _view.MostrarMensagem("Erro: ID ou Valor do lance inválidos.");
            }
        }

        private void AtualizarTurnos()
        {
            foreach (var sala in _salas.Values)
            {
                foreach (var item in sala.Itens)
                {
                    if (item.Status == "Ativo")
                    {
                        item.DecrementarTurno();
                        if (item.Status == "Vendido" && item.CompradorVencedor == "Jogador")
                        {
                            _saldoComprador -= item.ValorAtual;
                            _view.MostrarMensagem($"🚨 O lote '{item.Nome}' encerrou por tempo! Você venceu o leilão por R$ {item.ValorAtual:F2}!");
                        }
                        else if (item.Status == "Retirado")
                        {
                            _view.MostrarMensagem($"⏳ O lote '{item.Nome}' expirou sem lances e foi retirado do leilão.");
                        }
                    }
                }
            }
        }
    }
}