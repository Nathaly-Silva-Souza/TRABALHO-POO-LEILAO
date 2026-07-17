using System;

namespace SimuladorLeilao
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instanciação dos objetos
            ViewLeilao view = new ViewLeilao();
            IPilha pilhaNavegacao = new PilhaNavegacao();

            // O Controlador centraliza o fluxo e liga o Model com a View
            ControllerLeilao controlador = new ControllerLeilao(view, pilhaNavegacao);

            // Inicia o loop principal do terminal
            controlador.Run();
        }
    }
}