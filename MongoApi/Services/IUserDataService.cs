using MongoApi.Models;

namespace MongoApi.Services
{
    public interface IUserDataService
    {
        Task<List<UserData>> GetUser();
        Task<UserData?> GetUserById(string id, string password);
        Task<UserData?> GetUserByUserName(string username, string password);
        Task<UserData?> GetUserByEmail(string email, string password);
        Task<UserData?> CreateUser(UserData userData);
        Task<bool?> DeleteUser(string id, string password);
        Task<UserData?> UpdateUser(string id, UserData userData);
        Task<UserData?> UpdateUserPassword(string id, string password);
        Task<UserData?> DeactivateUser(string id, string status);
    }
}
