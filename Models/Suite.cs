
using System.Net.Security;

namespace DesafioProjetoHospedagem.Models
{
    public class Suite
    {
        private int suites = 0;
        public Suite() { }

        public Suite(int nmSuite, string tipoSuite, int capacidade, decimal valorDiaria, bool ocupada = false)
        {
            NumeroSuite = nmSuite;
            TipoSuite = tipoSuite;
            Capacidade = capacidade;
            ValorDiaria = valorDiaria;
            Ocupada = ocupada;
            Hospedes = new List<Pessoa>();
        }

        public int NumeroSuite { get; set; }
        public string TipoSuite { get; set; }
        public int Capacidade { get; set; }
        public decimal ValorDiaria { get; set; }
        public bool Ocupada { get; set; }
        public List<Pessoa>? Hospedes { get; set; }
    }
}
