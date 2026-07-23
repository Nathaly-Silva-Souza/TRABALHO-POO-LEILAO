using System;
using System.Collections.Generic;

namespace SimuladorLeilao
{
    public interface IPilha
    {
        void Push(string elemento);
        string Pop();
        string Peek();
        bool IsEmpty();
    }

    public class PilhaNavegacao : IPilha
    {
        private readonly List<string> _pilha;

        public PilhaNavegacao()
        {
            _pilha = new List<string> { "raiz" };
        }

        public void Push(string elemento)
        {
            _pilha.Add(elemento);
        }

        public string Pop()
        {
            if (_pilha.Count > 1) // Impede desempilhar a raiz
            {
                string item = _pilha[_pilha.Count - 1];
                _pilha.RemoveAt(_pilha.Count - 1);
                return item;
            }
            return "raiz";
        }

        public string Peek()
        {
            return IsEmpty() ? "raiz" : _pilha[_pilha.Count - 1];
        }

        public bool IsEmpty()
        {
            return _pilha.Count == 0;
        }
    }
}