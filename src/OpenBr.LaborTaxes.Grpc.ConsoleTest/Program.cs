using OpenBr.LaborTaxes.Grpc.Client;
using System;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Grpc.ConsoleTest
{
    class Program
    {
        static Task Main(string[] args)
        {
            RunTest().Wait();
            return Task.CompletedTask;
        }

        private static async Task RunTest()
        {

            IGrpcLaborTaxesProvider provider = new GrpcLaborTaxesProvider(null, "https://localhost:5001");

            //INSS
            var inssReply = await provider.CalculateInss(new Client.Contracts.CalculateInssArgs()
            {
                InssType = Contract.Enums.InssType.Worker,
                Revenue = 5000,
                ReferenceDate = null
            });

            Console.WriteLine($"Success: {inssReply.Success}");
            Console.WriteLine($"Reply.Amount: {inssReply.Data.Amount:N2}");
            Console.WriteLine($"Reply.IsLimit: {inssReply.Data.IsLimit}");

            //IRPF
            var irpfReply = await provider.CalculateIrpf(new Client.Contracts.CalculateIrpfArgs()
            {
                InssValue = 551.29M,
                DependentsNumber = 4,
                Revenue = 9000,
                ReferenceDate = null
            });
            Console.WriteLine($"Success: {irpfReply.Success}");
            Console.WriteLine($"Reply.CalculationBasis: {irpfReply.Data.CalculationBasis:N2}");
            Console.WriteLine($"Reply.Amount: {irpfReply.Data.Amount:N2}");

            //NETREVENUE
            var netRevenueReply = await provider.CalculateNetRevenue(new Client.Contracts.CalculateNetRevenueArgs()
            {
                InssType = Contract.Enums.InssType.Worker,
                DependentsNumber = 4,
                Revenue = 9000,
                ReferenceDate = null
            });
            Console.WriteLine($"Success: {netRevenueReply.Success}");
            Console.WriteLine($"Reply.Inss.Amount: {netRevenueReply.Data.Inss.Amount:N2}");
            Console.WriteLine($"Reply.Irpf.CalculationBasis: {netRevenueReply.Data.Irpf.CalculationBasis:N2}");
            Console.WriteLine($"Reply.Irpf.Amount: {netRevenueReply.Data.Irpf.Amount:N2}");

            Console.ReadKey();

        }
    }
}
