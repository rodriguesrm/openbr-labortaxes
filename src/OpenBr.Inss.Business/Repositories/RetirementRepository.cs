using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenBr.Inss.Business.Documents;
using OpenBr.Inss.Business.Enums;
using OpenBr.Inss.Business.Helpers;
using OpenBr.Inss.Business.Infra;
using OpenBr.Inss.Business.Infra.MongoDb;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Repositories
{
    public class RetirementRepository : RepositoryBase<Retirement>, IRetirementRepository, IDocumentCollectionCreator, IDataSeeder
    {

        #region Constructors

        ///<inheritdoc/>
        public RetirementRepository(IMongoDatabase mongoDb) : base(mongoDb, "retirement")
        {
        }

        #endregion

        #region Overrides

        
        ///<inheritdoc/>
        protected override UpdateDefinition<Retirement> MapToUpdate(Retirement document)
        {
            UpdateDefinition<Retirement> update = Builders<Retirement>.Update
                .Set(nameof(Retirement.Type).ToCamelCase(), document.Type)
                .Set(nameof(Retirement.DateStart).ToCamelCase(), document.DateStart)
                .Set(nameof(Retirement.DateEnd).ToCamelCase(), document.DateEnd)
                .Set(nameof(Retirement.Limit).ToCamelCase(), document.Limit)
                .Set(nameof(Retirement.Inactive).ToCamelCase(), document.Inactive)
                .Set(nameof(Retirement.Range).ToCamelCase(), document.Range)
            ;
            return update;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task CreateDocumentCollectionAsync()
        {
            await CreateCollectionAsync(false, null, null);
            await CreateIndexAsync(c => c.Type, nameof(Retirement.Type).ToCamelCase());
            await CreateIndexAsync(c => c.DateStart, nameof(Retirement.DateStart).ToCamelCase());
            await CreateIndexAsync(c => c.DateEnd, nameof(Retirement.DateEnd).ToCamelCase());
            await CreateIndexAsync(c => c.Inactive, nameof(Retirement.Inactive).ToCamelCase());
        }

        ///<inheritdoc/>
        public async Task<Retirement> GetActive(RetirementType type, DateTime? date, CancellationToken cancellationToken = default)
        {
            FilterDefinitionBuilder<Retirement> builder = Builders<Retirement>.Filter;
            FilterDefinition<Retirement> filter = builder.Eq(nameof(Retirement.Type).ToCamelCase(), type);
            if (date.HasValue)
            {
                filter = filter
                    & builder.Lte(nameof(Retirement.DateStart).ToCamelCase(), date.Value) 
                    & (
                        builder.Gte(nameof(Retirement.DateEnd).ToCamelCase(), date.Value) 
                        | builder.Eq(nameof(Retirement.DateEnd).ToCamelCase(), BsonNull.Value)
                    );
            }
            else
            {
                filter &= builder.Eq(nameof(Retirement.Inactive).ToCamelCase(), false);
            }

            Retirement result = await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return result;
        }

        #endregion

        #region Seed

        ///<inheritdoc/>
        public async Task Seed()
        {

            if (await Collection.CountDocumentsAsync(FilterDefinition<Retirement>.Empty) > 0)
                return;

            Retirement[] docs = GetSeedData();

            await Collection.InsertManyAsync(docs);
        }

        /// <summary>
        /// Get seed data from fire resource
        /// </summary>
        private Retirement[] GetSeedData()
        {
            string content;
            using (var stream = new StreamReader(AppResources.GetSeedData("retirement.json")))
            {
                content = stream.ReadToEnd();
            }

            Retirement[] docs = JsonSerializer.Deserialize<Retirement[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return docs;
        }

        #endregion

    }

}
