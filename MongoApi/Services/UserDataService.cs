using Microsoft.Extensions.Options;
using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IMongoCollection<UserData> _client;

        public UserDataService(IOptions<DatabaseSettings> settings, MongoDbHelper<UserData> dbHelper)
        {
            _client = dbHelper.GetCollection(settings.Value.CollectionName);
        }
        public async Task<UserData?> CreateUser(UserData userData)
        {
            await _client.InsertOneAsync(userData);
            return userData;
        }

        public async Task<bool?> DeleteUser(string id, string password)
        {
            var user = _client.Find(x => x.Id == id).FirstOrDefault();
            if (user != null && user.Password == password)
            {
                await _client.DeleteOneAsync(x => x.Id == id);
                return true;
            }
            return false;

        }

        public async Task<List<UserData>> GetUser()
        {
            var user = await _client.Find(userdata => true).ToListAsync();
            return user;
        }

        public async Task<UserData?> GetUserByEmail(string email, string password)
        {
            var user = await _client.Find(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null) return null;
            return user;
        }

        public async Task<UserData?> GetUserById(string id, string password)
        {
            var user = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null) return null;
            return user;
        }

        public async Task<UserData?> GetUserByUserName(string username, string password)
        {
            var user = await _client.Find(x => x.UserName == username).FirstOrDefaultAsync();
            if (user == null) return null;
            return user;
        }

        public async Task<UserData?> UpdateUser(string id, UserData userData)
        {
            await _client.ReplaceOneAsync(x => x.Id == id, userData);
            var user = _client.Find(x => x.Id == id).FirstOrDefault();
            if (user == null) return null;
            return user;
        }

        public async Task<UserData?> UpdateUserPassword(string id, string password)
        {
            var user = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                // User not found with the given ID
                return null;
            }

            var updateDefinition = Builders<UserData>.Update.Set(x => x.Password, password);
            await _client.UpdateOneAsync(x => x.Id == id, updateDefinition);

            user = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserData?> DeactivateUser(string id, string status)
        {
            var user = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                // User not found with the given ID
                return null;
            }

            var updateDefinition = Builders<UserData>.Update.Set(x => x.Status, status);
            await _client.UpdateOneAsync(x => x.Id == id, updateDefinition);

            user = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
            return user;
        }

        

    }
}
