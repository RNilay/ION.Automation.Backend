using IonFiltra.BagFilters.Core.Entities.Users.User;
using IonFiltra.BagFilters.Core.Entities.Users.UserRoles;
using IonFiltra.BagFilters.Core.Interfaces.Users.User;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Users.User
{
    public class UserRepository : IUserRepository
    {
        private readonly TransactionHelper _transactionHelper;

        public UserRepository(TransactionHelper transactionHelper)
        {
            _transactionHelper = transactionHelper;
        }

        /// <summary>
        /// GetAllUsersAsync method for User.
        /// </summary>
        /// <returns>Returns result.</returns>


        public async Task<IEnumerable<UserAccount>> GetAllUsersAsync()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {

                var query = dbContext.Users
                    .AsNoTracking()
                    .Where(u => !u.IsDeleted)
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role) // Include ApplicationRole
                    .AsQueryable();

                return await query
                           .Select(u => new UserAccount
                           {
                               UserId = u.UserId,
                               FirstName = u.FirstName,
                               LastName = u.LastName,
                               Email = u.Email,
                          
                               IsActive = u.IsActive,
                               UserRoles = u.UserRoles.Select(ur => new UserRole
                               {
                                   RoleId = ur.RoleId,
                                   Role = new ApplicationRoles
                                   {
                                       RoleName = ur.Role.RoleName
                                   }
                               }).ToList()

                           })
                           .ToListAsync();
            });
        }



        /// <summary>
        /// GetUserByUsernameOrEmailAsync method for User.
        /// </summary>
        /// <param name="usernameOrEmail">The usernameOrEmail parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task<UserAccount?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var user = await dbContext.Users
                    .AsNoTracking()
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => (u.Email == usernameOrEmail) && !u.IsDeleted);

                if (user == null)
                {
                    return null; // ✅ Return null if user is not found
                }

                return new UserAccount
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MfaEnabled = user.MfaEnabled,
                    MfaSecret = user.MfaSecret,
                    PasswordSalt = user.PasswordSalt,
                    PasswordHash = user.PasswordHash,
                    
                    IsActive = user.IsActive,
                   
                    UserRoles = user.UserRoles.Select(ur => new UserRole
                    {
                        RoleId = ur.RoleId,
                        Role = new ApplicationRoles
                        {
                            RoleName = ur.Role.RoleName
                        }
                    }).ToList()
                };
            });
        }


        /// <summary>
        /// Adds a new User record.
        /// </summary>
        /// <param name="user">The userId parameter.</param>
        /// <returns>The ID of the newly added record.</returns>
        public async Task<int> AddUserAsync(UserAccount user)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var addedUser = await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                return addedUser.Entity.UserId;
            });
        }
        /// <summary>
        /// Adds a new User record.
        /// </summary>
        /// <returns>The ID of the newly added record.</returns>
        public async Task AddUserRoleAsync(UserRole userRole)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                await dbContext.UserRoles.AddAsync(userRole);
            });
        }

        /// <summary>
        /// GetUserByIdAsync method for User.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task<UserAccount?> GetUserByIdAsync(int userId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);
            });

        }

        /// <summary>
        /// Updates an existing User record.
        /// </summary>
        /// <param name="roleIds">The userId parameter.</param>
        /// <returns>Returns success status or appropriate error message.</returns>


        public async Task UpdateUserAsync(UserAccount user, List<int> roleIds)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Get existing user from DB
                var existingUser = await dbContext.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.UserId == user.UserId);

                if (existingUser == null)
                    throw new Exception("User not found");

                // Update only necessary fields
                existingUser.Email = user.Email ?? existingUser.Email;
                existingUser.FirstName = user.FirstName ?? existingUser.FirstName;
                existingUser.LastName = user.LastName ?? existingUser.LastName;
               
                existingUser.UpdatedAt = DateTime.UtcNow;

                // Handle Roles
                if (user.UserRoles != null && user.UserRoles.Any())
                {
                    // Remove existing roles
                    dbContext.UserRoles.RemoveRange(existingUser.UserRoles);

                    // Add new roles
                    var newUserRoles = roleIds.Select(roleId => new UserRole
                    {
                        UserId = existingUser.UserId,
                        RoleId = roleId
                    }).ToList();

                    await dbContext.UserRoles.AddRangeAsync(newUserRoles);
                }

                await dbContext.SaveChangesAsync();
            });
        }


        /// <summary>
        /// Deletes an existing User record.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns success status or appropriate error message.</returns>
        public async Task DeleteUserAsync(int userId)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    user.IsDeleted = true;
                    await dbContext.SaveChangesAsync();
                }
            });
        }

        /// <summary>
        /// SearchUsersAsync method for User.
        /// </summary>
        /// <param name="searchQuery">The searchQuery parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchQuery)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.Users
                    .Where(u => !u.IsDeleted &&
                        (u.FirstName.Contains(searchQuery) ||
                         u.LastName.Contains(searchQuery) ||
                         u.Email.Contains(searchQuery)))
                    .ToListAsync();
            });
        }


        public async Task<(int? userId, string errorMessage)> AddUserAsync(UserAccount user, List<int> roleIds)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingUser = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return (null, "Email already exists.");
                }
                // user.UserType = "GuestUser";
                var addedUser = await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                if (addedUser.Entity.UserId > 0)
                {
                    var userRoles = roleIds.Select(roleId => new UserRole
                    {
                        UserId = addedUser.Entity.UserId,
                        RoleId = roleId
                    }).ToList();

                    await dbContext.UserRoles.AddRangeAsync(userRoles);
                    await dbContext.SaveChangesAsync();
                }

                return (addedUser.Entity.UserId, null);
            });
        }


        /// <summary>
        /// GetUsersByRolesAsync method for User.
        /// </summary>
        /// <returns>Returns result.</returns>
        public async Task<IEnumerable<UserAccount>> GetUsersByRolesAsync(params string[] roleNames)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Perform the query inside the transaction
                var users = await (from user in dbContext.Users
                                   join userRole in dbContext.UserRoles on user.UserId equals userRole.UserId
                                   join role in dbContext.Roles on userRole.RoleId equals role.RoleId
                                   where roleNames.Contains(role.RoleName) // Filter users by provided role names
                                   && !user.IsDeleted // Exclude deleted users
                                   select new UserAccount
                                   {
                                       UserId = user.UserId,
                                       FirstName = user.FirstName,
                                       LastName = user.LastName,
                                       Email = user.Email,
                                   }).ToListAsync();

                // Return the list of users
                return users;
            });



        }
        /// <summary>
        /// Deletes an existing User record.
        /// </summary>
        /// <returns>Returns success status or appropriate error message.</returns>
        public async Task DeleteAsync(UserAccount user)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                user.IsDeleted = true;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            });
        }
        /// <summary>
        /// DeactivateAsync method for User.
        /// </summary>
        /// <param name="user">The user parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task DeactivateAsync(UserAccount user)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                user.IsActive = false;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            });
        }

        /// <summary>
        /// ResetPasswordAsync method for User.
        /// </summary>
        /// <param name="email">The email parameter.</param>
        /// <param name="password">The password parameter.</param>
        /// <param name="salt">The salt parameter.</param>
        /// <param name="isEncrypted">The isEncrypted parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task<bool> ResetPasswordAsync(string email, string password, string salt, bool isEncrypted)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingUser = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

                if (existingUser == null)
                {
                    return false; // user not found
                }

                existingUser.PasswordHash = password;
                existingUser.PasswordSalt = salt;

                dbContext.Users.Update(existingUser);
                await dbContext.SaveChangesAsync();
                return true; // success
            });
        }



    }
}
