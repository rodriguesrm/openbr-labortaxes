using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Infra.MongoDb
{

    /// <summary>
    /// MongoDb data seeder class object
    /// </summary>
    public class MongoDataSeeder : IDbDataSeeder
    {

        #region Local objects/variables

        private readonly IEnumerable<IDataSeeder> seeders;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new MongoDataSeeder object instance
        /// </summary>
        /// <param name="seeders">Seeder collection</param>
        public MongoDataSeeder(IEnumerable<IDataSeeder> seeders)
        {
            this.seeders = seeders;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task Seed()
        {
            await Task.WhenAll(seeders.Select(c => c.Seed()));
        }

        #endregion

    }
}
