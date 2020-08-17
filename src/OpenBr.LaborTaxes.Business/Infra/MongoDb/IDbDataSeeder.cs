using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Infra.MongoDb
{

    /// <summary>
    /// Collection data seeder interface
    /// </summary>
    public interface IDbDataSeeder
    {

        /// <summary>
        /// Seed data in document collection
        /// </summary>
        public Task Seed();

    }

}
