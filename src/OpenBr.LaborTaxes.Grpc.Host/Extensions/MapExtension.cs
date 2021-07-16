using OpenBr.LaborTaxes.Business.Model;
using OpenBr.LaborTaxes.Contract;

namespace OpenBr.LaborTaxes.Grpc.Host.Extensions
{

    /// <summary>
    /// Provides extesion methods to map request/arguments
    /// </summary>
    public static class MapExtension
    {

        /// <summary>
        /// Map CalculateInssResult to CalculateInssReply
        /// </summary>
        /// <param name="result">Object to map</param>
        public static CalculateInssReply MapResult(this CalculateInssResult result)
            => new()
            {
                Success = true,
                Data = new InssReply()
                {
                    Rate = (double)result.Rate,
                    Amount = (double)result.Amount,
                    IsLimit = result.IsLimit
                }
            };

        /// <summary>
        /// Map CalculateIrpfResult to CalculateIrpfReply
        /// </summary>
        /// <param name="result">Object to map</param>
        public static CalculateIrpfReply MapResult(this CalculateIrpfResult result)
            => new()
            {
                Success = true,
                Data = new IrpfReply()
                {
                    Rate = (double)result.Rate,
                    CalculationBasis = (double)result.CalculationBasis,
                    DependentsDeductionAmount = (double)result.DependentsDeductionAmount,
                    Amount = (double)result.Amount
                }
            };

        public static CalculateNetRevenueReply MapResult(this CalculateNetRevenueResult result)
            => new()
            {
                Success = true,
                Data = new NetRevenueReply()
                {
                    Inss = new InssReply()
                    {
                        Rate = (double)result.Inss.Rate,
                        Amount = (double)result.Inss.Amount,
                        IsLimit = result.Inss.IsLimit
                    },
                    Irpf = new IrpfReply()
                    {
                        CalculationBasis = (double)result.Irpf.CalculationBasis,
                        Rate = (double)result.Irpf.Rate,
                        Amount = (double)result.Irpf.Amount,
                        DependentsDeductionAmount = (double)result.Irpf.DependentsDeductionAmount
                    }
                }
            };

    }

}
