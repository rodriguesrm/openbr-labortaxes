using OpenBr.LaborTaxes.Business.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Repositories
{

    /// <summary>
    /// Irpf tax repository interface
    /// </summary>
    public interface IIrpfRepository : IRepository<IrpfTax>
    {

        /// <summary>
        /// Get active Irpf
        /// </summary>
        /// <param name="date">Reference date to calculate</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task<IrpfTax> GetActive(DateTime? date, CancellationToken cancellationToken = default);

    }
}
