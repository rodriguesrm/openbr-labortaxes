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
        /// Calculates the rate and the amount of the inss contribution (INSS)
        /// </summary>
        /// <param name="type">Inss type</param>
        /// <param name="revenue">Revenue value</param>
        /// <param name="date">Reference date to calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        Task<CalculateInssResult> CalculateInss(InssType type, decimal revenue, DateTime? date, CancellationToken cancellationToken = default);

    }
}
