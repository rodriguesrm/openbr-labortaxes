using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OpenBr.Inss.Business.Infra.MongoDb
{

    /// <summary>
    /// Contains the conversion of the ObjectId <-> String between Application-MongoDb
    /// </summary>
    public class StringObjectIdConvention : ConventionBase, IPostProcessingConvention
    {

        ///<inheritdoc/>
        public void PostProcess(BsonClassMap classMap)
        {

            BsonMemberMap idMapeado = classMap.IdMemberMap;
            if (idMapeado != null && idMapeado.MemberName == "Id" && idMapeado.MemberType == typeof(string))
            {
                idMapeado.SetIdGenerator(new StringObjectIdGenerator());
            }

        }

    }
}
