using MongoApi.DTO;

namespace MongoApi.Services
{
    public interface IUserDataValidationService
    {
        List<string> ValidateGetUserByUserNameDto(GetUserByUserNameDto dto);
        List<string> ValidateGetUserByEmailDto(GetUserByEmailDto dto);
        List<string> ValidateGetUserByIdDto(GetUserByIdDto dto);
        bool IsUserNamePasswordValid(string userName, string password);
        bool IsEmailPasswordValid(string email, string password);
        bool IsIdPasswordValid(string id, string password);
        Task<bool> DoesUsernameExist(string username);
        Task<bool> DoesEmailExist(string email);
    }
}