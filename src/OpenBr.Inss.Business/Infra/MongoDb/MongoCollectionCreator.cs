using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Infra.MongoDb
{

    /// <summary>
    /// Creator of MongoDb database objects (Collections, Indexes, etc.)
    /// </summary>
    public class MongoCollectionCreator : IDbDocumentCollectionCreator
    {

        #region Local objects/variables

        private readonly IEnumerable<IDocumentCollectionCreator> _criadores;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MongoCollectionCreator instance
        /// </summary>
        /// <param name="creators">List of creative repositories</param>
        public MongoCollectionCreator(IEnumerable<IDocumentCollectionCreator> creators)
        {
            _criadores = creators;
        }

        #endregion

        ///<inheritdoc/>
        public async Task Create()
        {
            await Task.WhenAll(_criadores.Select(c => c.CreateDocumentCollectionAsync()));
        }

    }

}
