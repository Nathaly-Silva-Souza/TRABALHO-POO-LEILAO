using System;

namespace SimuladorLeilao.Views
{
    public class ViewLance
    {
        public (int idItem, string nomePessoa, double valorLance) ObterDadosLance()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=== REALIZAR LANCE ===");
            Console.ResetColor();

            Console.Write("ID do Item: ");
            int id;
            int.TryParse(Console.ReadLine(), out id);

            Console.Write("Seu Nome (Comprador): ");
            string nome = Console.ReadLine() ?? "Anônimo";

            Console.Write("Valor do Lance: R$ ");
            double valor;
            double.TryParse(Console.ReadLine(), out valor);

            return (id, nome, valor);
        }
    }
}