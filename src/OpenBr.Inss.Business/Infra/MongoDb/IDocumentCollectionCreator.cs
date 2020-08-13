using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Infra.MongoDb
{

    /// <summary>
    /// Interface for creates the collection on the MongoDb base
    /// </summary>
    public interface IDocumentCollectionCreator
    {

        /// <summary>
        /// Creates collection
        /// </summary>
        Task CreateCollectionAsync();

    }

}
