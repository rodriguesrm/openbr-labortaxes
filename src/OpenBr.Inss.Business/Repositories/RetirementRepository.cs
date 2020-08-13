using MongoDB.Driver;
using OpenBr.Inss.Business.Documents;
using OpenBr.Inss.Business.Helpers;
using OpenBr.Inss.Business.Infra.MongoDb;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Repositories
{
    public class RetirementRepository : RepositoryBase<Retirement>, IRetirementRepository, IDocumentCollectionCreator
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
        public async Task CreateCollectionAsync()
        {
            await CreateCollectionAsync();
            await CreateIndexAsync(c => c.Type, nameof(Retirement.Type).ToCamelCase());
        }

        #endregion

    }

}
