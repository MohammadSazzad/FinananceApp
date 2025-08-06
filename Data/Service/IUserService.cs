using FinanceApp.Models;
using FinanceApp.Models.DTO;

namespace FinanceApp.Data.Service
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserById(int id);
        Task RegisterUser(User user, string password);
        Task<LoginResponse> LoginUser(UserLoginDTO loginDTO);
        Task UpdateUser(User user);
        Task ChangePassword(int userId, string currentPassword, string newPassword);
        Task DeleteUser(int id);
    }
}