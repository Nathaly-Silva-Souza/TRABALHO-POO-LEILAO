using System;
using System.Collections.Generic;

namespace SimuladorLeilao
{
    public class ViewLeilao
    {
        public string ExibirMenuPrincipal(string diretorioAtual, double saldo)
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine($" SIMULADOR DE LEILÃO EM MEMÓRIA | Saldo: R$ {saldo:F2}");
            Console.WriteLine($" Pasta Atual: /{diretorioAtual}");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine(" [Comandos Disponíveis]:");
            Console.WriteLine("  cd [categoria]    -> Entrar em uma sala (Ex: cd carros, cd reliquias, cd artes)");
            Console.WriteLine("  cd ..             -> Voltar para a sala raiz");
            Console.WriteLine("  ls                -> Listar os itens da sala atual");
            Console.WriteLine("  cadastrar         -> Cadastrar novo item");
            Console.WriteLine("  atualizar         -> Atualizar preço base/máximo de um item");
            Console.WriteLine("  remover           -> Remover item do leilão");
            Console.WriteLine("  lance [id] [val]  -> Dar um lance em um item (Ex: lance 1 15000)");
            Console.WriteLine("  sair              -> Encerrar o programa");
            Console.WriteLine(new string('-', 50));
            Console.Write("Digite o seu comando: ");
            return Console.ReadLine()?.Trim().ToLower() ?? "";
        }

        public void MostrarMensagem(string mensagem)
        {
            Console.WriteLine($">> {mensagem}");
        }

        public void MostrarItens(List<ItemLeilao> itens)
        {
            if (itens.Count == 0)
            {
                Console.WriteLine("Nenhum item cadastrado nesta sala.");
                return;
            }
            foreach (var item in itens)
            {
                Console.WriteLine(item);
            }
        }

        public (string nome, double valMin, double valMax, int turnos, Dictionary<string, string> especificos) ObterDadosCadastro(string categoria)
        {
            Console.WriteLine($"\n--- Cadastro de Item em {categoria.ToUpper()} ---");
            Console.Write("Nome do item: ");
            string nome = Console.ReadLine();

            Console.Write("Valor Mínimo (Lance Inicial): ");
            double valMin = Convert.ToDouble(Console.ReadLine());

            Console.Write("Valor Máximo (Arremate Imediato): ");
            double valMax = Convert.ToDouble(Console.ReadLine());

            Console.Write("Duração do lote (em turnos/ações): ");
            int turnos = Convert.ToInt32(Console.ReadLine());

            var especificos = new Dictionary<string, string>();

            if (categoria == "carros")
            {
                Console.Write("Quilometragem: ");
                especificos["km"] = Console.ReadLine();
                Console.Write("Ano de Fabricação: ");
                especificos["ano"] = Console.ReadLine();
            }
            else if (categoria == "reliquias")
            {
                Console.Write("Ano de Origem: ");
                especificos["ano"] = Console.ReadLine();
                Console.Write("Estado de Conservação: ");
                especificos["conservacao"] = Console.ReadLine();
            }
            else if (categoria == "artes")
            {
                Console.Write("Nome do Artista: ");
                especificos["artista"] = Console.ReadLine();
                Console.Write("Técnica Utilizada: ");
                especificos["tecnica"] = Console.ReadLine();
            }

            return (nome, valMin, valMax, turnos, especificos);
        }
    }
}