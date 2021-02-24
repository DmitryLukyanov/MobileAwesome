using MongoDB.Driver;

namespace MobileAwesomeApp.Infrastructure.Mongo
{
    public class MongoClientWrapper // TODO: interface?
    {
        private readonly IMongoClient _client;
        public MongoClientWrapper(IMongoClient client)
        {
            _client = client;
        }

        public IMongoDatabase GetDatabase(DatabaseNamespace databaseNamespace)
        {
            return _client.GetDatabase(databaseNamespace.DatabaseName);
        }

        public IMongoCollection<TCollection> GetCollection<TCollection>(CollectionNamespace collectionNamespace)
        {
            return GetDatabase(collectionNamespace.DatabaseNamespace).GetCollection<TCollection>(collectionNamespace.CollectionName);
        }
    }
}
