using System;
using System.Collections.Generic;
using SimuladorLeilao.Models;

namespace SimuladorLeilao.Views
{
    public class ViewMenu
    {
        public string ExibirMenuPrincipal(string diretorioAtual, double saldo)
        {
            Console.Clear();
    
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black; 
    
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"   SIMULADOR DE LEILÃO EM MEMÓRIA | Saldo: R$ {saldo:F2}".PadRight(60));
            Console.WriteLine($"   Pasta Atual: /{diretorioAtual}".PadRight(60));
            Console.WriteLine(new string('=', 60));
    
    
            Console.ResetColor();
    
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n [Comandos Disponíveis]:");
            Console.WriteLine("  cd [categoria]    -> Entrar em uma sala");
            Console.WriteLine("  cd ..             -> Voltar");
            Console.WriteLine("  ls                -> Listar itens");
            Console.WriteLine("  cadastrar         -> Cadastrar item");
            Console.WriteLine("  lance             -> Dar um lance");
            Console.WriteLine("  sair              -> Encerrar\n");
            Console.ResetColor();

            Console.Write("▶ Digite o comando: ");
            return Console.ReadLine()?.Trim().ToLower() ?? "";
        }

        public void MostrarMensagem(string mensagem, ConsoleColor cor = ConsoleColor.Yellow)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine($"\n>> {mensagem}\n");
            Console.ResetColor();
            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        public void MostrarItens(List<ItemLeilao> itens)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n=== ITENS NA SALA ===");
            if (itens.Count == 0) Console.WriteLine("Nenhum item cadastrado.");
            
            foreach (var item in itens)
            {
                Console.WriteLine(item);
            }
            Console.ResetColor();
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}