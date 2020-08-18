using MongoDB.Bson;
using MongoDB.Driver;
using OpenBr.LaborTaxes.Business.Documents;
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

    public class IrpfRepository : RepositoryBase<IrpfTax>, IIrpfRepository, IDocumentCollectionCreator, IDataSeeder
    {

        #region Constructors

        ///<inheritdoc/>
        public IrpfRepository(IMongoDatabase mongoDb) : base(mongoDb, "irpf")
        {
        }

        #endregion

        #region Overrides


        ///<inheritdoc/>
        protected override UpdateDefinition<IrpfTax> MapToUpdate(IrpfTax document)
        {
            UpdateDefinition<IrpfTax> update = Builders<IrpfTax>.Update
                .Set(nameof(IrpfTax.DateStart).ToCamelCase(), document.DateStart)
                .Set(nameof(IrpfTax.DateEnd).ToCamelCase(), document.DateEnd)
                .Set(nameof(IrpfTax.DeductionAmount).ToCamelCase(), document.DeductionAmount)
                .Set(nameof(IrpfTax.Inactive).ToCamelCase(), document.Inactive)
                .Set(nameof(IrpfTax.Range).ToCamelCase(), document.Range)
            ;
            return update;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task CreateDocumentCollectionAsync()
        {
            await CreateCollectionAsync(false, null, null);
            await CreateIndexAsync(c => c.DateStart, nameof(IrpfTax.DateStart).ToCamelCase());
            await CreateIndexAsync(c => c.DateEnd, nameof(IrpfTax.DateEnd).ToCamelCase());
            await CreateIndexAsync(c => c.Inactive, nameof(IrpfTax.Inactive).ToCamelCase());
        }

        ///<inheritdoc/>
        public async Task<IrpfTax> GetActive(DateTime? date, CancellationToken cancellationToken = default)
        {
            FilterDefinitionBuilder<IrpfTax> builder = Builders<IrpfTax>.Filter;
            FilterDefinition<IrpfTax> filter;
            if (date.HasValue)
            {
                filter = 
                    builder.Lte(nameof(IrpfTax.DateStart).ToCamelCase(), date.Value)
                    & (
                        builder.Gte(nameof(IrpfTax.DateEnd).ToCamelCase(), date.Value)
                        | builder.Eq(nameof(IrpfTax.DateEnd).ToCamelCase(), BsonNull.Value)
                    );
            }
            else
            {
                filter = builder.Eq(nameof(IrpfTax.Inactive).ToCamelCase(), false);
            }

            IrpfTax result = await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return result;
        }

        #endregion

        #region Seed

        ///<inheritdoc/>
        public async Task Seed()
        {

            if (await Collection.CountDocumentsAsync(FilterDefinition<IrpfTax>.Empty) > 0)
                return;

            IrpfTax[] docs = GetSeedData();

            await Collection.InsertManyAsync(docs);
        }

        /// <summary>
        /// Get seed data from fire resource
        /// </summary>
        private IrpfTax[] GetSeedData()
        {
            string content;
            using (var stream = new StreamReader(AppResources.GetSeedData("irpf.json")))
            {
                content = stream.ReadToEnd();
            }

            IrpfTax[] docs = JsonSerializer.Deserialize<IrpfTax[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return docs;
        }

        #endregion

    }
}