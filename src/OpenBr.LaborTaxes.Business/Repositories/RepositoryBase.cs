using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Repositories
{

    /// <summary>
    /// Absctract repository base
    /// </summary>
    /// <typeparam name="TDocument">Document type</typeparam>
    public abstract class RepositoryBase<TDocument> : IRepository<TDocument>
        where TDocument : IEntity
    {

        #region Local object/variables

        protected string _collectionName;
        protected IMongoDatabase _db;

        /// <summary>
        /// Database collection
        /// </summary>
        protected IMongoCollection<TDocument> Collection => _db.GetCollection<TDocument>(_collectionName);

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new repository object instance
        /// </summary>
        /// <param name="mongoDb">MongoDb database object connection instance</param>
        /// <param name="collectionName">Collection name</param>
        public RepositoryBase(IMongoDatabase mongoDb, string collectionName)
        {
            _collectionName = collectionName;
            _db = mongoDb;
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Map the fields to be updated
        /// </summary>
        /// <param name="document">Document object instance</param>
        protected abstract UpdateDefinition<TDocument> MapToUpdate(TDocument document);

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public virtual async Task<TDocument> GetById(string id, CancellationToken cancellationToken = default)
        {
            FilterDefinition<TDocument> fd = Builders<TDocument>.Filter.Eq("_id", id);
            return await Collection.Find(fd).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TDocument>> List(CancellationToken cancellationToken = default)
            => await Collection.AsQueryable().ToListAsync(cancellationToken);

        /// <inheritdoc/>
        public virtual async Task Add(TDocument document, CancellationToken cancellationToken = default)
            => await Collection.InsertOneAsync(document, new InsertOneOptions(), cancellationToken);

        /// <inheritdoc/>
        public virtual async Task Update(TDocument document, CancellationToken cancellationToken = default)
        {
            FilterDefinition<TDocument> fd = Builders<TDocument>.Filter.Eq("_id", document.Id);
            UpdateDefinition<TDocument> update = MapToUpdate(document);
            await Collection.UpdateOneAsync(fd, update, null, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task Remove(TDocument document, CancellationToken cancellationToken = default)
        {
            FilterDefinition<TDocument> fd = Builders<TDocument>.Filter.Eq("_id", document.Id);
            await Collection.DeleteOneAsync(fd, cancellationToken);
        }

        #endregion

        #region ICollectionCreator

        /// <summary>
        /// Checks whether a collection exists
        /// </summary>
        protected async Task<bool> CollectionExtitsAsync()
        {

            IAsyncCursor<BsonDocument> collections = await _db.ListCollectionsAsync();
            bool result = false;

            if (collections != null)
                await collections.ForEachAsync(document => result = result || document["name"].AsString.Equals(_collectionName));

            return result;

        }

        /// <summary>
        /// Get a collection
        /// </summary>
        protected IMongoCollection<TDocument> MongCollection => _db.GetCollection<TDocument>(_collectionName);

        /// <summary>
        /// Create collection if not exist
        /// </summary>
        protected Task<IMongoCollection<TDocument>> CriarColecaoAsync()
            => CreateCollectionAsync(false, null, null);

        /// <summary>
        /// Create collection if not exist
        /// </summary>
        /// <param name="limited">Indicates whether the collection will be of the limited type ("capped")</param>
        /// <param name="maxSize">Maximum collection size</param>
        /// <param name="documentLimits">Maximum number of documents</param>
        protected async Task<IMongoCollection<TDocument>> CreateCollectionAsync(bool limited, long? maxSize, long? documentLimits)
        {

            if (!(await CollectionExtitsAsync()))
            {

                if (limited && maxSize > 0)
                {

                    CreateCollectionOptions options = new CreateCollectionOptions
                    {
                        Capped = limited,
                        MaxSize = maxSize
                    };

                    if (documentLimits > 0)
                        options.MaxDocuments = documentLimits;

                    await _db.CreateCollectionAsync(_collectionName, options);

                }
                else
                {
                    await _db.CreateCollectionAsync(_collectionName);
                }

            }

            return MongCollection;

        }


        /// <summary>
        /// Checks if index exists
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <param name="indexNames">List of index names to be checked</param>
        protected async Task<bool> IndexExistsAsync(IMongoCollection<TDocument> collectionName, params string[] indexNames)
        {
            using (IAsyncCursor<BsonDocument> cursor = await collectionName.Indexes.ListAsync())
            {
                IEnumerable<BsonDocument> indexes = await cursor.ToListAsync();
                foreach (BsonDocument ix in indexes)
                {
                    BsonDocument currentIndex = ix.GetValue("key", null).ToBsonDocument();
                    if (currentIndex == null) continue;
                    bool result = indexNames.Any(checkIndex => (currentIndex.GetValue(checkIndex, 0).ToBoolean()));
                    if (result) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Create index if not exists
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="indexName">Index name</param>
        protected Task CreateIndexAsync(Expression<Func<TDocument, object>> fieldName, string indexName)
            => CreateIndexAsync(fieldName, indexName, false, null);

        /// <summary>
        /// Create index if not exists
        /// </summary>
        /// <param name="fieldsName">Fields name</param>
        /// <param name="indexName">Index name</param>
        /// <param name="isExclusive">Indicates whether the index is unique/exclusive</param>
        protected Task CreateIndexAsync(Expression<Func<TDocument, object>> fieldsName, string indexName, bool isExclusive)
            => CreateIndexAsync(fieldsName, indexName, isExclusive, null);

        /// <summary>
        /// Create index if not exists
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="isExclusive">Indicates whether the index is unique/exclusive</param>
        /// <param name="lifetime">Document lifetime</param>
        /// <param name="fields">Fields list</param>
        protected async Task CreateIndexAsync(string indexName, bool isExclusive, int? lifetime, params Expression<Func<TDocument, object>>[] fields)
        {

            IMongoCollection<TDocument> collection = MongCollection;

            if (collection != null)
            {
                if (!await IndexExistsAsync(collection, indexName))
                {
                    IndexKeysDefinition<TDocument>[] indexKeys = new IndexKeysDefinition<TDocument>[fields.Length];

                    for (int i = 0; i < fields.Length; i++)
                    {
                        indexKeys[i] = Builders<TDocument>.IndexKeys.Ascending(fields[i]);
                    }

                    CreateIndexOptions parametrosIndice;

                    if (lifetime.HasValue)
                        parametrosIndice = new CreateIndexOptions { Name = indexName, Unique = isExclusive, ExpireAfter = TimeSpan.FromDays(lifetime.Value) };
                    else
                        parametrosIndice = new CreateIndexOptions { Name = indexName, Unique = isExclusive };

                    IndexKeysDefinition<TDocument> definicaoIndice = Builders<TDocument>.IndexKeys.Combine(indexKeys);
                    CreateIndexModel<TDocument> modeloIndice = new CreateIndexModel<TDocument>(definicaoIndice, parametrosIndice);
                    await collection.Indexes.CreateOneAsync(modeloIndice);

                }
            }
        }

        /// <summary>
        /// Create index if not exists
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="indexName">Index name</param>
        /// <param name="isExclusive">Indicates whether the index is unique/exclusive</param>
        /// <param name="lifetime">Document lifetime</param>
        protected async Task CreateIndexAsync(Expression<Func<TDocument, object>> fieldName, string indexName, bool isExclusive, int? lifetime)
        {
            IMongoCollection<TDocument> collection = MongCollection;

            if (collection != null)
            {
                if (!await IndexExistsAsync(collection, indexName))
                {

                    TimeSpan? tsDias = null;
                    if (lifetime.HasValue)
                        tsDias = TimeSpan.FromDays(lifetime.Value);

                    CreateIndexOptions parametrosIndice = new CreateIndexOptions { Name = indexName, Unique = isExclusive, ExpireAfter = tsDias };
                    CreateIndexModel<TDocument> modeloIndice = new CreateIndexModel<TDocument>(Builders<TDocument>.IndexKeys.Ascending(fieldName), parametrosIndice);
                    await collection.Indexes.CreateOneAsync(modeloIndice);

                }
            }
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing">Flag indicating execution of 'dispose'</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //object?.Dispose();
                }

                _db = null;
                _collectionName = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Destroy the repository object
        /// </summary>
        ~RepositoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
