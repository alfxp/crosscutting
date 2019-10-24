using MongoDB.Driver;

namespace Iris.Crosscutting.Infrastructure.Contexts.MongoDb
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<T> GetCollection<T>(string collectionName);
        IMongoClient Client { get; }
    }
}