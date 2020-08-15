using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Repositories
{

    /// <summary>
    /// Repository interface
    /// </summary>
    public interface IRepository<TDocument> : IDisposable
        where TDocument : IEntity
    {

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <param name="id">Record Id</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task<TDocument> GetById(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// List the documents in the collection
        /// </summary>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task<IEnumerable<TDocument>> List(CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a document
        /// </summary>
        /// <param name="document">Document instance</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task Add(TDocument document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing document
        /// </summary>
        /// <param name="document">Document instance</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task Update(TDocument document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete an existing document
        /// </summary>
        /// <param name="document">Document instance</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task Remove(TDocument document, CancellationToken cancellationToken = default);

    }

}
