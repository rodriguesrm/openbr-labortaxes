using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Infra.MongoDb
{

    /// <summary>
    /// Data seeder interface
    /// </summary>
    public interface IDataSeeder
    {

        /// <summary>
        /// Seed data in document collection
        /// </summary>
        public Task Seed();

    }

}
