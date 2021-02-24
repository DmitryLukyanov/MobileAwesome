using MongoDB.Driver;

namespace MobileAwesomeApp.Infrastructure.Mongo
{
    public class MongoClientFactory
    {
        private readonly ISettingsProvider _settingsProvider;

        public MongoClientFactory(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public IMongoClient CreateMongoClient()
        {
            var apiKey = _settingsProvider.GetByKey("ApiKey");
            var apiKeyAuthArguments = _settingsProvider.GetByKey("MongoRealmConnectionStringAuthArguments");
            var mongoClientSettings = MongoClientSettings.FromConnectionString($"mongodb://_:{apiKey}@realm.mongodb.com:27020/?{apiKeyAuthArguments}");
            return new MongoClient(mongoClientSettings);
        }
    }
}
