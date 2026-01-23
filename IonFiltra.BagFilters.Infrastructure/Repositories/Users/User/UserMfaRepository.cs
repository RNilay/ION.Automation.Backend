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
    public class UserMfaRepository : IUserMfaRepository
    {
        private readonly TransactionHelper _transactionHelper;

        public UserMfaRepository(TransactionHelper transactionHelper)
        {
            _transactionHelper = transactionHelper;
        }

        /// <summary>
        /// SetMfaSecretAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="secret">The secret parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task SetMfaSecretAsync(int userId, string secret)
        {
            await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    user.MfaSecret = secret;
                    await dbContext.SaveChangesAsync();
                }
            });
        }

        /// <summary>
        /// SetMfaSecretAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="userEmail">The userEmail parameter.</param>
        /// <param name="secret">The secret parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task SetMfaSecretAsync(int? userId, string userEmail, string secret)
        {
            await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                var user = userId.HasValue
                    ? await dbContext.Users.FindAsync(userId.Value)
                    : await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user != null)
                {
                    user.MfaSecret = secret;
                    await dbContext.SaveChangesAsync();
                }
            });
        }

        /// <summary>
        /// EnableMfaAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="isEnabled">The isEnabled parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task EnableMfaAsync(int userId, bool isEnabled)
        {
            await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    user.MfaEnabled = isEnabled;
                    await dbContext.SaveChangesAsync();
                }
            });
        }

        /// <summary>
        /// EnableMfaAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="userEmail">The userEmail parameter.</param>
        /// <param name="isEnabled">The isEnabled parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task EnableMfaAsync(int? userId, string userEmail, bool isEnabled)
        {
            await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                var user = userId.HasValue
                    ? await dbContext.Users.FindAsync(userId.Value)
                    : await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user != null)
                {
                    user.MfaEnabled = isEnabled;
                    await dbContext.SaveChangesAsync();
                }
            });
        }


        /// <summary>
        /// GetMfaSecretAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task<string> GetMfaSecretAsync(int userId)
        {
            return await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                var user = await dbContext.Users.FindAsync(userId);
                return user?.MfaSecret;
            });
        }

        public async Task<(int UserId, string MfaSecret)> GetMfaSecretAsync(int? userId, string userEmail)
        {
            return await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                var user = userId.HasValue && userId > 0
                    ? await dbContext.Users.FindAsync(userId)
                    : await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                return user != null ? (user.UserId, user.MfaSecret) : (0, null);
            });
        }

        /// <summary>
        /// ResetQrCodeAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task<bool> ResetQrCodeAsync(int userId)
        {
            return await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null)
                    return false;

                user.MfaEnabled = false; // or 0 if it's an int

                user.MfaSecret = null;

                await dbContext.SaveChangesAsync();
                return true;
            });
        }

    }
}
