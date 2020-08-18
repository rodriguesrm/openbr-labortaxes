using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OpenBr.LaborTaxes.Business.Repositories;

namespace OpenBr.LaborTaxes.Business.Documents
{

    /// <summary>
    /// Abstract document entity template
    /// </summary>
    public abstract class DocumentBase : IEntity
    {

        /// <inheritdoc/>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
