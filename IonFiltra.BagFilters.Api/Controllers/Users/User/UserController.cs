using IonFiltra.BagFilters.Application.DTOs.Users.Request;
using IonFiltra.BagFilters.Application.DTOs.Users.User;
using IonFiltra.BagFilters.Application.Interfaces.Users.User;
using IonFiltra.BagFilters.Core.Entities.Users.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace IonFiltra.BagFilters.Api.Controllers.Users.User
{
    /// <summary>
    /// Controller class for UserController.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IUserMfaService _userMfaService;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IUserMfaService userMfaService, ILogger<UserController> logger)
        {
            _userService = userService;
            _userMfaService = userMfaService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches User data by  ID.
        /// </summary>
        /// <param name="id">The ID to search for.</param>
        /// <returns>Returns User details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount>> GetUserById(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", new object[] { id });
            if (id <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", new object[] { id });
                return BadRequest("Invalid user ID.");
            }
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)

            {
                _logger.LogWarning("User with ID {UserId} not found", new object[] { id });
                return NotFound($"User with ID {id} not found.");
            }
            _logger.LogInformation("User fetched successfully: {User}", user);

            return Ok(user);
        }
        /// <summary>
        /// Adds a new User record.
        /// </summary>
        /// <param name="request">The NJSCreateUserRequest containing data to add.</param>
        /// <returns>Returns the newly created record ID.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("CreateUser started with Request {request}", new object[] { request });
            var userDto = request.UserDto;
            string passwordSalt = request.salt;

            (int? userId, string errorMessage) result;
            if (request.IsEncrypted == true && !string.IsNullOrEmpty(passwordSalt))
            {
                result = await _userService.CreateUserAsync(userDto, passwordSalt);//create user with salt
            }
            else
            {
                result = await _userService.CreateUserAsync(userDto);
            }

            if (result.userId.HasValue)
            {
                // Associate roles with the user
                if (userDto.Roles != null && userDto.Roles.Any())
                {
                    await _userService.AssignRolesToUserAsync(result.userId.Value, userDto.Roles);
                }
                return StatusCode(201, new { message = "User created successfully.", userId = result.userId });
            }
            else
            {
                return StatusCode(400, new { error = result.errorMessage ?? "Failed to create user." });
            }
        }

        /// <summary>
        /// Updates an existing User record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns>Returns success status or appropriate error message.</returns>
        [HttpPut("update/{id}")]

        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto user)
        {
            _logger.LogInformation("UpdateUser started with Id {id} and User {user}", new object[] { id, user });
            if (user == null)
            {
                return BadRequest("Invalid user data or ID.");
            }

            bool result = await _userService.UpdateUserAsync(id, user);

            if (!result)
            {
                // Simulate internal error for missing user
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = $"Failed to update user with ID {id}. User might not exist or an unexpected error occurred."
                });
            }

            return Ok(new
            {
                Status = 200,
                Message = "User updated successfully."
            });
        }


        /// <summary>
        /// Generating OTP Request method.
        /// </summary>
        /// <param name="loginDto"></param>
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp(UserLoginDto loginDto)
        {
            _logger.LogInformation("RequestOtp started with Logindto {loginDto}", new object[] { loginDto });
            var result = await _userService.LoginAsync(loginDto);

            if (!result.IsSuccessful)
            {
                return Unauthorized(result);
            }

            var otpSent = await _userService.GenerateAndSendOtpAsync(result.UserId.Value);
            if (!otpSent)
            {
                return StatusCode(500, "Failed to send OTP. Please try again.");
            }

            return Ok(new { Message = "OTP sent to your email." });
        }
        /// <summary>
        /// VerifyOtp method.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otp"></param>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(int userId, string otp)
        {
            _logger.LogInformation("VerifyOtp started with Userid {userId} and Otp {otp}", new object[] { userId, otp });
            var isOtpValid = await _userService.VerifyOtpAsync(userId, otp);

            if (!isOtpValid)
            {
                return Unauthorized(new { Message = "Invalid or expired OTP." });
            }

            var user = await _userService.GetUserByIdAsync(userId);
            var token = _userService.GenerateJwtToken(user);

            return Ok(new
            {
                Message = "Login successful.",
                Token = token
            });
        }
        /// <summary>
        /// GetSalt method.
        /// </summary>
        /// <param name="Email"></param>
        [HttpPost("challenge")]
        public async Task<IActionResult> GetSalt([FromBody] string Email)
        {
            _logger.LogInformation("GetSalt started with Email {Email}", new object[] { Email });
            if (string.IsNullOrEmpty(Email))
            {
                return BadRequest("Email is required.");
            }

            var userLoginResponseDTO = await _userService.GetUserByEmailAsync(Email);
            var user = userLoginResponseDTO?.UserAccountObj;

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user.PasswordSalt);
        }
        /// <summary>
        /// Login method.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            _logger.LogInformation("Login started with Request {request}", new object[] { request });
            var userLoginResponseDTO = await _userService.GetUserByEmailAsync(request.Email);
            var user = userLoginResponseDTO?.UserAccountObj;

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            


            if (user.IsActive == false)
            {
                return Unauthorized("Your account is currently inactive. Please contact the administrator to regain access.");
            }


            if (!request.IsNJSAuth)
            {

                var storedSalt = user.PasswordSalt; // Retrieve stored salt
                var hashedPasswordFromClient = request.Password; // HMAC SHA256 Hash received from client

                // Compute the hash using HMAC SHA256 and stored salt
                using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(user.PasswordHash));
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Challenge)));

                if (computedHash != hashedPasswordFromClient)
                {
                    return Unauthorized("Invalid credentials.");
                }
            }

            // Generate JWT Token
            var token = _userService.GenerateJwtToken(user);

            // Send MFA status to frontend
            return Ok(new
            {
                RequiresMfa = user.MfaEnabled,
                Token = token, // Always send token
                userId = user.UserId,
                userEmail = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserRoles = user.UserRoles,
               
            });
        }

        /// <summary>
        /// VerifyMfa method.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("verify-mfa")]
        public async Task<IActionResult> VerifyMfa([FromBody] MfaVerificationRequest request)
        {
            _logger.LogInformation("VerifyMfa started with Request {request}", new object[] { request });
            var isValid = await _userMfaService.VerifyMfaCodeAsync(request.UserId, request.TotpCode);
            if (!isValid)
            {
                return Unauthorized("Invalid TOTP code.");
            }

            var user = await _userService.GetUserByIdAsync(request.UserId);
            var token = _userService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Fetches ALl User data .
        /// </summary>      
        /// <returns>Returns the User details.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        /// <summary>
        /// Fetches User data by Email.
        /// </summary>
        /// <param name="Email">The Email addrerss to search for.</param>
        /// <returns>Returns the User details for the specified project.</returns>
        [HttpGet("user-by-email")]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUserByEmail([FromBody] string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return BadRequest("Email is required.");
            }

            var userLoginResponseDTO = await _userService.GetUserByEmailAsync(Email);
            var user = userLoginResponseDTO?.UserAccountObj;

            if (user == null)

            {
                return NotFound("User not found.");
            }

            return Ok(user.PasswordSalt);
        }
        /// <summary>
        /// Search data.
        /// </summary>
        /// <param name="query"></param>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserAccount>>> SearchUsers([FromQuery] string query)
        {
            var users = await _userService.SearchUsersAsync(query);
            return Ok(users);
        }
        /// <summary>
        /// Fetches Users data by Role .
        /// </summary>
        /// <param name="roles">Theroles to search for.</param>
        /// <returns>Returns user details.</returns>
        [HttpGet("users-by-role")]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUsersByRole([FromQuery] string[] roles)
        {
            if (roles == null || roles.Length == 0)
            {
                return BadRequest("Please provide at least one role.");
            }

            var users = await _userService.GetUsersByRolesAsync(roles);

            if (!users.Any())
            {
                return NotFound("No users found with the specified roles.");
            }

            return Ok(users);
        }
        /// <summary>
        /// Delete an existing User record By ID.
        /// </summary>
        /// <param name="id">The Delete User by ID</param>
        /// <returns>Returns success status or appropriate error message.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUsersById(int id)
        {
            await _userService.DeleteUserById(id);
            return Ok($"User with ID {id} deleted successfully.");
        }
        /// <summary>
        /// Deactivate an existing User record By ID.
        /// </summary>
        /// <param name="id">The deactivate User by ID</param>
        /// <returns>Returns success status or appropriate error message.</returns>
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUserById(int id)
        {

            await _userService.DeactivateUserById(id);
            return Ok($"User with ID {id} Deactivated successfully.");
        }
        /// <summary>
        /// Reset-password an existing Password .
        /// </summary>
        /// <param name="request">The Reset-password </param>
        /// <returns>Returns success status or appropriate error message.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Email and password are required." });
            }

            try
            {


                var result = await _userService.ResetPasswordAsync(request.Email, request.Password, request.salt, true);


                if (result)
                {
                    return Ok(new { message = "Password reset successfully." });
                }
                else
                {
                    return StatusCode(500, new { error = "Password reset failed. The user may not exist or an internal error occurred." });
                }
            }
            catch (Exception ex)
            {
                // Optional: log the exception
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


    }
}
