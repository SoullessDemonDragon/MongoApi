using Microsoft.Extensions.Options;
using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.Services
{
    public class MongoDbHelper<T>
    {
        private readonly IMongoDatabase _database;

        public MongoDbHelper(IOptions<DatabaseSettings> settings, IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
