using OpenBr.LaborTaxes.Contract.Enums;
using OpenBr.LaborTaxes.Contract.Model;
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
        /// <param name="revenue">Revenue value</param>
        /// <param name="dependentsNumber">Number of dependents</param>
        /// <param name="date">Reference date to calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        Task<CalculateIrpfResult> CalculateIrpf(decimal revenue, decimal inssValue, byte dependentsNumber, DateTime? date, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculate net revenue by deducting inss and irpf
        /// </summary>
        /// <param name="type">Inss type</param>
        /// <param name="revenue">Revenue value</param>
        /// <param name="dependentsNumber">Number of dependents</param>
        /// <param name="date">Reference date to calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        Task<CalculateNetRevenueResult> CalculateNetRevenue(InssType type, decimal revenue, byte dependentsNumber, DateTime? date, CancellationToken cancellationToken = default);

    }
}
