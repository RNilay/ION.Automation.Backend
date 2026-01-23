using IonFiltra.BagFilters.Core.Entities.Users.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Interfaces.Users.User
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserAccount>> GetAllUsersAsync(); // Retrieve all users
        Task<UserAccount?> GetUserByIdAsync(int userId); // Get a specific user by ID
        Task<UserAccount> GetUserByUsernameOrEmailAsync(string usernameOrEmail); // Get a user by username or email
        Task<int> AddUserAsync(UserAccount user); // Add a new user

        //Task<int> AddUserAsync(UserAccount user,List<int> roles); // Add a new user

        Task<(int? userId, string errorMessage)> AddUserAsync(UserAccount user, List<int> roles);

        Task AddUserRoleAsync(UserRole userRole);

        Task UpdateUserAsync(UserAccount user, List<int> roleIds); // Update user details
        Task DeleteUserAsync(int userId); // Soft delete a user

        Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchQuery);//search user

        //get users by role name
        Task<IEnumerable<UserAccount>> GetUsersByRolesAsync(params string[] roleNames);

        Task DeleteAsync(UserAccount user);

        Task DeactivateAsync(UserAccount user);
        Task<bool> ResetPasswordAsync(string email, string password, string salt, bool isEncrypted);

    }
}
