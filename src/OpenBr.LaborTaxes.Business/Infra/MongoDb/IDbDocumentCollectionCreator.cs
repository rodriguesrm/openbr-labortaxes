using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Infra.MongoDb
{

    /// <summary>
    /// Interface for creates collections on the MongoDb database
    /// </summary>
    public interface IDbDocumentCollectionCreator
    {

        /// <summary>
        /// Create collections
        /// </summary>
        Task Create();

    }

}
