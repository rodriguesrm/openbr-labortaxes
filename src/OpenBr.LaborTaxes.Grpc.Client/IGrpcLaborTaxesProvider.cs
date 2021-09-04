using OpenBr.LaborTaxes.Contract.Model;
using OpenBr.LaborTaxes.Grpc.Client.Contracts;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Grpc.Client
{

    /// <summary>
    /// gRpc Labor Taxes provider interface/contract
    /// </summary>
    public interface IGrpcLaborTaxesProvider
    {

        /// <summary>
        /// Call calculate-inss service method
        /// </summary>
        /// <param name="args">Arguments object instance</param>
        Task<CalculateBaseResult<CalculateInssResult>> CalculateInss(CalculateInssArgs args);

        /// <summary>
        /// Call calculate-irpf service method
        /// </summary>
        /// <param name="args">Arguments object instance</param>
        Task<CalculateBaseResult<CalculateIrpfResult>> CalculateIrpf(CalculateIrpfArgs args);

        /// <summary>
        /// Call calculate-irpf service method
        /// </summary>
        /// <param name="args">Arguments object instance</param>
        Task<CalculateBaseResult<CalculateNetRevenueResult>> CalculateNetRevenue(CalculateNetRevenueArgs args);

    }
}