using MongoDB.Driver;

namespace Iris.Crosscutting.Infrastructure.Contexts.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoClient Client { get; }

        public IMongoDatabase Database { get; }

        public MongoDbContext(MongoDbContextConfig config, MongoClient client)
        {
            Client = client;
            Database = GetMongoDatabase(config.Database);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }

        private IMongoDatabase GetMongoDatabase(string databaseName)
        {
            try
            {
                return Client.GetDatabase(databaseName);
            }
            catch
            {
                return GetMongoDatabase(databaseName);
            }
        }
    }
}