using OpenBr.LaborTaxes.Business.Documents;
using OpenBr.LaborTaxes.Contract.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Repositories
{

    /// <summary>
    /// Inss contribution repository interface
    /// </summary>
    public interface IInssRepository : IRepository<InssTax>
    {

        /// <summary>
        /// Get active Inss 
        /// </summary>
        /// <param name="type">Inss type</param>
        /// <param name="date">Reference date to calculate</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task<InssTax> GetActive(InssType type, DateTime? date, CancellationToken cancellationToken = default);

    }

}