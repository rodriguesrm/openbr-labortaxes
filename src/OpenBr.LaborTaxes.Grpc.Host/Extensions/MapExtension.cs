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
            =>  new()
                {
                    Success = true,
                    Rate = (double)result.Rate,
                    Amount = (double)result.Amount,
                    IsLimit = result.IsLimit
                };

    }

}
