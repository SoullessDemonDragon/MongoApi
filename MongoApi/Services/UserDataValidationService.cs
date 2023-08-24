using Microsoft.Extensions.Options;
using MongoApi.DTO;
using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.Services
{
    public class UserDataValidationService : IUserDataValidationService
    {
        private readonly IMongoCollection<UserData> _client;

        public UserDataValidationService(IOptions<DatabaseSettings> settings, MongoDbHelper<UserData> dbHelper)
        {
            _client = dbHelper.GetCollection(settings.Value.CollectionName);
        }

        public List<string> ValidateGetUserByIdDto(GetUserByIdDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.Id))
            {
                errors.Add("Id is required.");
            }

            if (string.IsNullOrEmpty(dto.Password))
            {
                errors.Add("Password is required.");
            }

            return errors;
        }
        public List<string> ValidateGetUserByEmailDto(GetUserByEmailDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.Email))
            {
                errors.Add("Email is required.");
            }

            if (string.IsNullOrEmpty(dto.Password))
            {
                errors.Add("Password is required.");
            }

            return errors;
        }

        public List<string> ValidateGetUserByUserNameDto(GetUserByUserNameDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.UserName))
            {
                errors.Add("Username is required.");
            }

            if (string.IsNullOrEmpty(dto.Password))
            {
                errors.Add("Password is required.");
            }

            return errors;
        }

        public bool IsUserNamePasswordValid(string userName, string password)
        {
            var user = _client.Find(x => x.UserName == userName).FirstOrDefault();
            if (user != null && user.Password == password) return true;
            return false;
        }

        public bool IsEmailPasswordValid(string email, string password)
        {
            var user = _client.Find(x => x.Email == email).FirstOrDefault();
            if (user != null && user.Password == password) return true;
            return false;
        }

        public bool IsIdPasswordValid(string id, string password)
        {
            var user = _client.Find(x => x.Id == id).FirstOrDefault();
            if (user != null && user.Password == password) return true;
            return false;
        }

        public async Task<bool> DoesUsernameExist(string username)
        {
            var filter = Builders<UserData>.Filter.Eq(x => x.UserName, username);
            var count = await _client.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<bool> DoesEmailExist(string email)
        {
            var filter = Builders<UserData>.Filter.Eq(x => x.Email, email);
            var count = await _client.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
