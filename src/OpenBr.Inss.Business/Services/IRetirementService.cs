using OpenBr.Inss.Business.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Services
{

    /// <summary>
    /// Retirement service interface
    /// </summary>
    public interface IRetirementService
    {

        Task<string> CalculateRetirement(RetirementType type, decimal revenue, CancellationToken cancellationToken = default);

    }
}
