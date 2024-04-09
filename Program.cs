using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

// Cria os modelos de hóspedes e cadastra na lista de hóspedes
List<Pessoa> hospedes = new List<Pessoa>();

Reserva reserva = new Reserva();

bool exibirMenu = true;

// Realiza o loop do menu
while (exibirMenu)
{
    Console.Clear();
    Console.WriteLine("Bem-vindo ao sistema de hospedagem");
    Console.WriteLine("\nDigite a sua opção:");
    Console.WriteLine("1 - Cadastrar suíte");
    Console.WriteLine("2 - Cadastrar hospede");
    Console.WriteLine("3 - Check-out suíte");
    Console.WriteLine("4 - Listar hóspedes");
    Console.WriteLine("5 - Listar suítes");
    Console.WriteLine("6 - Encerrar\n");

    switch (Console.ReadLine())
    {
        case "1":
            reserva.CadastrarSuite();
            break;

        case "2":
            reserva.CadastrarHospedes();
            break;

        case "3":
            reserva.Checkout();
            break;

        case "4":
            reserva.ListarHospedes();
            break;

        case "5":
            reserva.ListarSuites();
            break;

        case "6":
            Console.WriteLine("\nObrigado por utilizar o sistema de hospedagem");
            exibirMenu = false;
            break;

        default:
            Console.WriteLine("\nOpção inválida");
            break;
    }

    Console.WriteLine("\nPressione enter para continuar");
    Console.ReadLine();
}
