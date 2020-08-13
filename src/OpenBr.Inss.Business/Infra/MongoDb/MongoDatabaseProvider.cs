using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using OpenBr.Inss.Business.Infra.Config;

namespace OpenBr.Inss.Business.Infra.MongoDb
{

    /// <summary>
    /// Provides a MongoDb database connection
    /// </summary>    
    public class MongoDatabaseProvider
    {

        #region Local objects/variables

        protected ApplicationConfig _appConfig;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new provider instance
        /// </summary>
        /// <param name="configuration">Coleção de configurações</param>
        public MongoDatabaseProvider(IOptions<ApplicationConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        #endregion

        #region Static methods

        static MongoDatabaseProvider()
        {
            ConventionPack cp = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new StringObjectIdConvention(),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("conventions", cp, t => true);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get database connection object
        /// </summary>
        public IMongoDatabase GetDatabase()
        {
            string nomeBd = MongoUrl.Create(_appConfig.ConnectionStrings.MongoDb).DatabaseName;
            MongoClient dbClient = new MongoClient(_appConfig.ConnectionStrings.MongoDb);
            return dbClient.GetDatabase(nomeBd);
        }

        #endregion

    }

}
