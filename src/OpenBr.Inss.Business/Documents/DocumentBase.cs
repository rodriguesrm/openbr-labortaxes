using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OpenBr.Inss.Business.Repositories;

namespace OpenBr.Inss.Business.Documents
{

    /// <summary>
    /// Modelo abstrato de entidade de documento
    /// </summary>
    public abstract class DocumentBase : IEntity
    {

        /// <inheritdoc/>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
