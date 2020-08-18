using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Infra.MongoDb
{

    /// <summary>
    /// Interface for creates the collection on the MongoDb base
    /// </summary>
    public interface IDocumentCollectionCreator
    {

        /// <summary>
        /// Create document collection
        /// </summary>
        Task CreateDocumentCollectionAsync();

    }

}
