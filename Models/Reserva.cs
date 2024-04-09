

namespace DesafioProjetoHospedagem.Models
{
    public class Reserva
    {
        public List<Pessoa> Hospedes = new List<Pessoa>();
        public List<Suite> Suite = new List<Suite>();
        private List<int> hospedesIndex = new List<int>(); // Índice temporário para apoio no check-in e check-out
        private int nmSuites = 0;
        public int DiasReservados { get; set; }

        public Reserva() { }

        public void CadastrarHospedes()
        {
            // Verifica a quantidade de suítes disponíveis
            int suitesDisponiveis = Suite?.Count(s => !s.Ocupada) ?? 0;

            // Verifica se há suítes cadastradas
            if (Suite?.Count == 0 || suitesDisponiveis == 0)
            {
                Console.WriteLine("Não há suítes cadastradas ou desocupadas no momento!");
                return;
            }

            Console.Write("Digite a quantidade de hóspedes a cadastrar: ");
            int qtdHospedes = LerIntMaiorQueZero("Quantidade inválida, digite novamente");

            int maxCapacidade = Suite.Where(s => !s.Ocupada).Max(s => s.Capacidade);

            // Verifica se a quantidade de hóspedes é maior que a capacidade das suítes
            if (qtdHospedes > maxCapacidade)
            {
                Console.WriteLine("Quantidade de hóspedes maior que a capacidade das suítes disponíveis\n" +
                    $"Tente distribuir em mais de um quarto. A maior suíte livre comporta até {maxCapacidade} pessoas.");
                return;
            }


            for (int i = 0; i < qtdHospedes; i++)
            {
                // Pega o maior ID de hospede cadastrador no momento
                int id = Hospedes?.Count > 0 ? Hospedes.Max(h => h.ID) : 0;

                Console.WriteLine($"\nDados do hóspede {i + 1}");
                Console.Write("Digite o primeiro nome do hóspede: ");
                string nome = Console.ReadLine();
                Console.Write("Digite o sobrenome do hóspede: ");
                string sobrenome = Console.ReadLine();

                Hospedes.Add(new Pessoa(id + 1, nome, sobrenome));

                // Adiciona o hospede ao índice temporário para o check-in
                hospedesIndex.Add(id + 1);
            }

            // Manda para o check-in os hóspedes cadastrados
            CheckIn(hospedesIndex, qtdHospedes);
        }

        private void CheckIn(List<int> hospedesIndex, int qtdHospedes)
        {
            Console.Clear();
            Console.WriteLine("\nRealizar check-in");

            Console.WriteLine($"Suites disponíveis: ");
            foreach (var suite in Suite)
            {
                if (!suite.Ocupada)
                {
                    Console.WriteLine($"\nNº da suíte: {suite.NumeroSuite}");
                    Console.WriteLine($"Tipo: {suite.TipoSuite} (Até {suite.Capacidade} pessoas)");
                    Console.WriteLine($"Valor da diária: {suite.ValorDiaria}\n");
                }
            }

            Console.Write("Digite o número da suíte: ");
            int numeroSuite = LerIntMaiorQueZero("Número inválido, digite novamente");

            Suite suiteSelecionada = Suite.FirstOrDefault(s => s.NumeroSuite == numeroSuite);

            // Loop para verificar se a suite existe, está livre e tem capacidade para os hóspedes
            while (suiteSelecionada == null || suiteSelecionada.Ocupada || suiteSelecionada.Capacidade < qtdHospedes)
            {
                Console.WriteLine("A suíte selecionada não está disponível ou não foi encontrada.\n" +
                    "Selecione uma das suítes listadas acima!: ");
                numeroSuite = LerIntMaiorQueZero("Número inválido, digite novamente: ");

                suiteSelecionada = Suite.FirstOrDefault(s => s.NumeroSuite == numeroSuite);
            }                

            // Itera sobre os hospedes e adiciona a suite escolhida
            foreach (var index in hospedesIndex)
            {
                var hospede = Hospedes.FirstOrDefault(h => h.ID == index);
                suiteSelecionada.Hospedes.Add(hospede);
            }

            hospedesIndex.Clear(); // Limpa o índice temporário para não influenciar no próximo cadastro

            suiteSelecionada.Ocupada = true;
            Console.WriteLine("Hóspedes registrados com sucesso");
        }

        public void Checkout()
        {
            // Verifica se há suítes ocupadas
            if (Suite?.Count(s => s.Ocupada) == 0)
            {
                Console.WriteLine("Não há suítes ocupadas no momento!");
                return;
            }

            Console.WriteLine("Digite o número da suíte para realizar o check-out: ");
            int numeroSuite = LerIntMaiorQueZero("Número inválido, tente novamente");

            Suite suiteSelecionada = Suite.FirstOrDefault(s => s.NumeroSuite == numeroSuite);

            // Verifica se a suite está ocupada
            if (suiteSelecionada == null || !suiteSelecionada.Ocupada)
            {
                Console.WriteLine("Suite não está ocupada ou não existe");
                return;
            }

            Console.Write("Digite a quantidade de dias reservados: ");
            int dias = LerIntMaiorQueZero("Quantidade de dias inválida, tente novamente");

            // Calcula o valor da diária
            decimal valorDiaria = CalcularValorDiaria(dias, numeroSuite - 1);
            string desconto = dias >= 10 ? " (com desconto de 10%)" : "";

            // Exibe o valor da diária
            Console.WriteLine($"Valor total da estadia: {valorDiaria}{desconto}");

            // Exibe os hóspedes da suite
            Console.WriteLine("Hóspedes da suite: ");
            foreach (var hospede in suiteSelecionada.Hospedes)
            {
                Console.WriteLine($"{hospede.Nome} {hospede.Sobrenome}");
                hospedesIndex.Add(hospede.ID);
            }

            // Remove os hóspedes da lista de hóspedes
            foreach (var id in hospedesIndex)
            {
                Hospedes.RemoveAll(h => h.ID == id);
            }

            // Realiza o check-out
            hospedesIndex.Clear();
            suiteSelecionada.Hospedes.Clear();
            suiteSelecionada.Ocupada = false;
            Console.WriteLine("Check-out realizado com sucesso");
        }

        public void CadastrarSuite()
        {
            int qtdSuites = 0;
            Console.Write("Cadastrar mais de 1 do mesmo tipo? (S/N): ");
            string opcao = Console.ReadLine();
            while (opcao.ToUpper() != "S" && opcao.ToUpper() != "N")
            {
                Console.WriteLine("Opção inválida, digite novamente");
                opcao = Console.ReadLine();
            }

            if (opcao.ToUpper() == "S")
            {
                Console.Write("Digite a quantidade de suítes a cadastrar: ");
                qtdSuites = LerIntMaiorQueZero("Quantidade inválida, digite novamente: ");
            }
            else
            {
                qtdSuites = 1;
            }

            Console.Write("\nDigite o tipo da suíte: ");
            string tipoSuite = Console.ReadLine().ToUpper();

            Console.Write("\nDigite a capacidade da suíte(apenas número): ");
            int capacidade = LerIntMaiorQueZero("\nCapacidade inválida, tente novamente: ");

            Console.Write("\nDigite o valor da diária: ");
            // Loop enquanto o valor da diária for menor ou igual a 0 ou não for um numero
            decimal valorDiaria = 0;
            while (!decimal.TryParse(Console.ReadLine(), out valorDiaria) || valorDiaria <= 0)
            {
                Console.Write("\nValor da diária inválido, digite novamente: ");
            }

            for (int i = 0; i < qtdSuites; i++)
            {
                nmSuites += 1;
                Suite suite = new Suite(nmSuites, tipoSuite, capacidade, valorDiaria);

                Suite.Add(suite);
            }
            string plural = qtdSuites > 1 ? "s" : "";

            Console.WriteLine($"{qtdSuites} suíte{plural} do tipo '{tipoSuite}' adicionada{plural} com sucesso!");
        }

        public void ListarHospedes()
        {
            // Selecionar se quer a lista de hóspedes por nome ou suíte
            Console.WriteLine("\nDigite a opção desejada:");
            Console.WriteLine("1 - Listar hóspedes por nome");
            Console.WriteLine("2 - Listar hóspedes por suíte");
            string opcao = Console.ReadLine();
            while (opcao != "1" && opcao != "2")
            {
                Console.WriteLine("\nOpção inválida, digite novamente");
                opcao = Console.ReadLine();
            }

            if (opcao == "1")
            {
                if (Hospedes?.Count == null || Hospedes?.Count == 0)
                {
                    Console.WriteLine("\nNão há hóspedes cadastrados");
                    return;
                }

                Console.WriteLine("\n\nListagem de hóspedes por nome: ");
                foreach (var hospede in Hospedes)
                {
                    Console.WriteLine($"{hospede.Nome} {hospede.Sobrenome}");
                }
            }
            else
            {
                if (Suite?.Count == null || Suite?.Count == 0)
                {
                    Console.WriteLine("\nNão há suítes cadastradas");
                    return;
                }

                // Verifica se há suítes ocupadas
                if (Suite?.Count(s => s.Ocupada) == 0)
                {
                    Console.WriteLine("\nNão há suítes ocupadas no momento!");
                    return;
                }

                Console.WriteLine("\n\nListagem de hóspedes por suíte: ");
                foreach (var suite in Suite)
                {
                    // Se a suíte estiver livre, pula para a próxima
                    if (!suite.Ocupada)
                    {
                        continue;
                    }

                    Console.WriteLine($"\nNº da suíte: {suite.NumeroSuite}");
                    Console.WriteLine($"Tipo: {suite.TipoSuite} Capacidade: {suite.Capacidade}");
                    Console.WriteLine($"Valor da diária: {suite.ValorDiaria}");
                    Console.WriteLine("Hóspedes: ");
                    foreach (var hospede in suite.Hospedes)
                    {
                        Console.WriteLine($"{hospede.Nome} {hospede.Sobrenome}");
                    }
                }
            }
        }

        private decimal CalcularValorDiaria(int dias, int suite)
        {
            // Calcula o valor total da reserva com base no valor da diária e a quantidade de dias
            // Considerando um desconto de 10% para período igual ou maior que 10 dias
            decimal valor = Suite[suite].ValorDiaria * dias;
            if (dias >= 10)
            {
                valor -= (valor * 0.1m);
            }

            return valor;
        }

        public void ListarSuites()
        {
            if (Suite?.Count == null || Suite?.Count == 0)
            {
                Console.WriteLine("\nNão há suítes cadastradas");
                return;
            }

            Console.WriteLine("\n\nListagem de suítes: ");
            foreach (var suite in Suite)
            {
                var ocupada = suite.Ocupada ? "ocupada" : "disponível";

                Console.WriteLine($"\nNº da suíte: {suite.NumeroSuite}");
                Console.WriteLine($"Tipo: {suite.TipoSuite} (Até {suite.Capacidade} pessoas)");
                Console.WriteLine($"Valor da diária: {suite.ValorDiaria}");
                Console.WriteLine($"-= Suíte {ocupada} =-");
            }
        }

        private int LerIntMaiorQueZero(string mensagem)
        {
            int numero = 0;
            while (!int.TryParse(Console.ReadLine(), out numero) || numero <= 0)
            {
                Console.WriteLine(mensagem);
            }
            return numero;
        }
    }
}
