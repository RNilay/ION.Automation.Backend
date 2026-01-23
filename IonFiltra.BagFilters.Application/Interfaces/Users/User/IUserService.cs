using IonFiltra.BagFilters.Application.DTOs.Users.User;
using IonFiltra.BagFilters.Core.Entities.Users.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Interfaces.Users.User
{
    public interface IUserService
    {
        Task<(int? userId, string errorMessage)> CreateUserAsync(UserDto userDto);

        Task<(int? userId, string errorMessage)> CreateUserAsync(UserDto userDto, String salt);

        //Task UpdateUserAsync(UserAccount user); // Update user details

        Task<bool> UpdateUserAsync(int userId, UpdateUserDto dto);

        Task AssignRolesToUserAsync(int userId, List<int> roleIds);

        Task<IEnumerable<UserAccount>> GetAllUsersAsync();
        Task<UserAccount> GetUserByIdAsync(int id); // Fetch a user by their ID
        Task<UserLoginResponseDto> LoginAsync(UserLoginDto loginDto);
        Task<bool> GenerateAndSendOtpAsync(int userId);
        Task<bool> VerifyOtpAsync(int userId, string otp);
        string GenerateJwtToken(UserAccount user);
        Task<UserLoginResponseDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchQuery);//Search user
        Task<IEnumerable<UserAccount>> GetUsersByRolesAsync(params string[] roleNames);//get users by role name
        Task DeleteUserById(int id);
        Task DeactivateUserById(int id);
        Task<bool> ResetPasswordAsync(string email, string password, string salt, bool isEncrypted);

    }
}
