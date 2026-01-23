using IonFiltra.BagFilters.Application.DTOs.Users.User;
using IonFiltra.BagFilters.Application.Interfaces.Users.User;
using IonFiltra.BagFilters.Core.Entities.Users.User;
using IonFiltra.BagFilters.Core.Interfaces.Users.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Services.Users.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IOtpRepository otpRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Adds a new User record.
        /// </summary>
        /// <param name="userDto">The UserDto containing details to add.</param>
        /// <returns>The ID of the newly added record.</returns>
        public async Task<(int? userId, string errorMessage)> CreateUserAsync(UserDto userDto)
        {
            var (PasswordHash, Salt) = HashPassword(userDto.Password);

            // Get the hashed password and salt from HashPassword method
            var (hashedPassword, salt) = HashPassword(userDto.Password);

            var user = new UserAccount
            {
                Email = userDto.Email,
                PasswordHash = PasswordHash,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PasswordSalt = Salt,
               
            };

            return await _userRepository.AddUserAsync(user, userDto.Roles);
        }

        public async Task<(int? userId, string errorMessage)> CreateUserAsync(UserDto userDto, String salt)
        {
            // Get the hashed password and salt from HashPassword method
            var user = new UserAccount
            {
                Email = userDto.Email,
                PasswordHash = userDto.Password,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PasswordSalt = salt,
                
        
            };

            return await _userRepository.AddUserAsync(user, userDto.Roles);
        }

        /// <summary>
        /// UpdateUserAsync method for User.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="dto">The dto parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            // Update fields only if values are provided
            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                user.LastName = dto.LastName;

           

            await _userRepository.UpdateUserAsync(user, dto.Roles);
            return true;
        }


        /// <summary>
        /// GetAllUsersAsync method for User.
        /// </summary>
        /// <returns>Returns the User details for the specified project.</returns>

        public async Task<IEnumerable<UserAccount>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        /// <summary>
        /// GetUserByIdAsync method for User.
        /// </summary>
        /// <param name="id">The id parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>
        public async Task<UserAccount> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        /// <summary>
        /// AssignRolesToUserAsync method for User.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="roleIds">The roleIds parameter.</param>
        /// <returns>Returns success status or appropriate error message.</returns>
        public async Task AssignRolesToUserAsync(int userId, List<int> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                await _userRepository.AddUserRoleAsync(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });
            }
        }
        /// <summary>
        /// LoginAsync method for User.
        /// </summary>
        /// <param name="loginDto">The loginDto parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>

        public async Task<UserLoginResponseDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(loginDto.UsernameOrEmail);

            if (user == null || user.IsDeleted || !user.IsActive)
            {
                return new UserLoginResponseDto { IsSuccessful = false, Message = "Invalid username or email." };
            }

            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return new UserLoginResponseDto { IsSuccessful = false, Message = "Invalid password." };
            }

            return new UserLoginResponseDto { IsSuccessful = true, UserId = user.UserId, FirstName = user.FirstName };
        }

        /// <summary>
        /// GenerateAndSendOtpAsync method for User.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>

        public async Task<bool> GenerateAndSendOtpAsync(int userId)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            var otpRecord = new UserOtp
            {
                UserId = userId,
                Otp = otp,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            await _otpRepository.SaveOtpAsync(otpRecord);

            // Send OTP logic (e.g., via email)
            return true;
        }

        /// <summary>
        /// VerifyOtpAsync method for User.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="otp">The otp parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>

        public async Task<bool> VerifyOtpAsync(int userId, string otp)
        {
            var otpRecord = await _otpRepository.GetLatestOtpForUserAsync(userId);

            if (otpRecord == null || otpRecord.IsUsed || otpRecord.ExpiresAt < DateTime.UtcNow || otpRecord.Otp != otp)
            {
                return false;
            }

            otpRecord.IsUsed = true;
            await _otpRepository.UpdateOtpAsync(otpRecord);

            return true;
        }

        private (string hashPassword, string salt) HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return (hashPassword, salt);
        }

        //private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);


        private bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        public string GenerateJwtToken(UserAccount user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            // step-1 assignment-1
            var claims = new List<Claim>
    {
        new Claim("UserId", user.UserId.ToString()),
         // Add UserType claim
    };
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// GetUserByEmailAsync method for User.
        /// </summary>
        /// <param name="email">The email parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>

        public async Task<UserLoginResponseDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(email);

            if (user == null || user.IsDeleted)
            {
                return new UserLoginResponseDto { IsSuccessful = false, Message = "Invalid username or email." };
            }
            return new UserLoginResponseDto { IsSuccessful = true, UserId = user.UserId, FirstName = user.FirstName, UserAccountObj = user };

        }

        /// <summary>
        /// SearchUsersAsync method for User.
        /// </summary>
        /// <param name="searchQuery">The searchQuery parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>

        public async Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return await _userRepository.GetAllUsersAsync();

            return await _userRepository.SearchUsersAsync(searchQuery);
        }

        //get users by role name
        /// <summary>
        /// GetUsersByRolesAsync method for User.
        /// </summary>
        /// <returns>Returns the User details for the specified project.</returns>
        public async Task<IEnumerable<UserAccount>> GetUsersByRolesAsync(params string[] roleNames)
        {
            return await _userRepository.GetUsersByRolesAsync(roleNames);
        }


        /// <summary>
        /// DeleteUserById method for User.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns success status or appropriate error message.</returns>
        public async Task DeleteUserById(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found or already deleted.");
            }
            await _userRepository.DeleteAsync(user);
        }

        /// <summary>
        ///DeactivateUserById method for User.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns success status or appropriate error message.</returns>

        public async Task DeactivateUserById(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found or already deactivated.");
            }
            await _userRepository.DeactivateAsync(user);
        }
        /// <summary>
        /// ResetPasswordAsync method for User.
        /// </summary>
        /// <param name="email">The email parameter.</param>
        /// <param name="password">The password parameter.</param>
        /// <param name="salt">The salt parameter.</param>
        /// <param name="isEncrypted">The isEncrypted parameter.</param>
        /// <returns>Returns the User details for the specified project.</returns>
        public async Task<bool> ResetPasswordAsync(string email, string password, string salt, bool isEncrypted)
        {
            return await _userRepository.ResetPasswordAsync(email, password, salt, isEncrypted);
        }

    }

}
