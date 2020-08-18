using OpenBr.LaborTaxes.Business.Enums;
using OpenBr.LaborTaxes.Business.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Services
{

    /// <summary>
    /// Inss service interface
    /// </summary>
    public interface ILaborTaxesService
    {

        /// <summary>
        /// Calculates the rate and the amount of the Inss contribution
        /// </summary>
        /// <param name="type">Inss type</param>
        /// <param name="revenue">Revenue value</param>
        /// <param name="date">Reference date to calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        Task<CalculateInssResult> CalculateInss(InssType type, decimal revenue, DateTime? date, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates the rate and the amount of the Iprf tax
        /// </summary>
        /// <param name="revenue"></param>
        /// <param name="revenue">Revenue value</param>
        /// <param name="qtyDependents">Number of dependents</param>
        /// <param name="date">Reference date to calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        Task<CalculateIrpfResult> CalculateIrpf(decimal revenue, decimal inssValue, decimal qtyDependents, DateTime? date, CancellationToken cancellationToken = default);

    }
}
