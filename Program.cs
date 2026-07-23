using SimuladorLeilao.Controllers;
using SimuladorLeilao.Utils;

namespace SimuladorLeilao
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Inicia os dados e contexto central
            ContextoSistema contexto = new ContextoSistema();

            // 2. O Controlador Principal assume o comando
            MainController app = new MainController(contexto);
            
            // 3. Executa a lógica
            app.Run();
        }
    }
}