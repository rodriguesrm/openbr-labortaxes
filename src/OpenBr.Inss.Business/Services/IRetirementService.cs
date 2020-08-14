using OpenBr.Inss.Business.Enums;
using OpenBr.Inss.Business.Model;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Services
{

    /// <summary>
    /// Retirement service interface
    /// </summary>
    public interface IRetirementService
    {

        /// <summary>
        /// Calculates the rate and the amount of the retirement contribution (INSS)
        /// </summary>
        /// <param name="type">Retirement type</param>
        /// <param name="revenue">Revenue value</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        /// <returns></returns>
        Task<CalculateRetirementResult> CalculateRetirement(RetirementType type, decimal revenue, CancellationToken cancellationToken = default);

    }
}
