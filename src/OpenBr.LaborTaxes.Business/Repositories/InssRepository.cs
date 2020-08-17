using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenBr.LaborTaxes.Business.Documents;
using OpenBr.LaborTaxes.Business.Enums;
using OpenBr.LaborTaxes.Business.Helpers;
using OpenBr.LaborTaxes.Business.Infra;
using OpenBr.LaborTaxes.Business.Infra.MongoDb;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Repositories
{
    public class InssRepository : RepositoryBase<InssTax>, IInssRepository, IDocumentCollectionCreator, IDataSeeder
    {

        #region Constructors

        ///<inheritdoc/>
        public InssRepository(IMongoDatabase mongoDb) : base(mongoDb, "inss")
        {
        }

        #endregion

        #region Overrides

        
        ///<inheritdoc/>
        protected override UpdateDefinition<InssTax> MapToUpdate(InssTax document)
        {
            UpdateDefinition<InssTax> update = Builders<InssTax>.Update
                .Set(nameof(InssTax.Type).ToCamelCase(), document.Type)
                .Set(nameof(InssTax.DateStart).ToCamelCase(), document.DateStart)
                .Set(nameof(InssTax.DateEnd).ToCamelCase(), document.DateEnd)
                .Set(nameof(InssTax.Limit).ToCamelCase(), document.Limit)
                .Set(nameof(InssTax.Inactive).ToCamelCase(), document.Inactive)
                .Set(nameof(InssTax.Range).ToCamelCase(), document.Range)
            ;
            return update;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task CreateDocumentCollectionAsync()
        {
            await CreateCollectionAsync(false, null, null);
            await CreateIndexAsync(c => c.Type, nameof(InssTax.Type).ToCamelCase());
            await CreateIndexAsync(c => c.DateStart, nameof(InssTax.DateStart).ToCamelCase());
            await CreateIndexAsync(c => c.DateEnd, nameof(InssTax.DateEnd).ToCamelCase());
            await CreateIndexAsync(c => c.Inactive, nameof(InssTax.Inactive).ToCamelCase());
        }

        ///<inheritdoc/>
        public async Task<InssTax> GetActive(InssType type, DateTime? date, CancellationToken cancellationToken = default)
        {
            FilterDefinitionBuilder<InssTax> builder = Builders<InssTax>.Filter;
            FilterDefinition<InssTax> filter = builder.Eq(nameof(InssTax.Type).ToCamelCase(), type);
            if (date.HasValue)
            {
                filter = filter
                    & builder.Lte(nameof(InssTax.DateStart).ToCamelCase(), date.Value) 
                    & (
                        builder.Gte(nameof(InssTax.DateEnd).ToCamelCase(), date.Value) 
                        | builder.Eq(nameof(InssTax.DateEnd).ToCamelCase(), BsonNull.Value)
                    );
            }
            else
            {
                filter &= builder.Eq(nameof(InssTax.Inactive).ToCamelCase(), false);
            }

            InssTax result = await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return result;
        }

        #endregion

        #region Seed

        ///<inheritdoc/>
        public async Task Seed()
        {

            if (await Collection.CountDocumentsAsync(FilterDefinition<InssTax>.Empty) > 0)
                return;

            InssTax[] docs = GetSeedData();

            await Collection.InsertManyAsync(docs);
        }

        /// <summary>
        /// Get seed data from fire resource
        /// </summary>
        private InssTax[] GetSeedData()
        {
            string content;
            using (var stream = new StreamReader(AppResources.GetSeedData("inss.json")))
            {
                content = stream.ReadToEnd();
            }

            InssTax[] docs = JsonSerializer.Deserialize<InssTax[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return docs;
        }

        #endregion

    }

}
