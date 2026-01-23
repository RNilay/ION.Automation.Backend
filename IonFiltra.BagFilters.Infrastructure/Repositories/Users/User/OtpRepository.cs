using IonFiltra.BagFilters.Core.Entities.Users.User;
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
    public class OtpRepository : IOtpRepository
    {
        private readonly TransactionHelper _transactionHelper;

        public OtpRepository(TransactionHelper transactionHelper)
        {
            _transactionHelper = transactionHelper;
        }

        /// <summary>
        /// SaveOtpAsync method for Otp.
        /// </summary>
        /// <param name="otp">The otp parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task SaveOtpAsync(UserOtp otp)
        {
            await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                await dbContext.UserOtps.AddAsync(otp);
                await dbContext.SaveChangesAsync();
            });
        }

        /// <summary>
        /// GetLatestOtpForUserAsync method for Otp.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns result.</returns>
        public async Task<UserOtp> GetLatestOtpForUserAsync(int userId)
        {
            return await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                return await dbContext.UserOtps
                    .Where(o => o.UserId == userId && !o.IsUsed) // Find unused OTPs
                    .OrderByDescending(o => o.CreatedAt) // Get the latest one
                    .FirstOrDefaultAsync();
            });
        }

        /// <summary>
        /// Updates an existing Otp record.
        /// </summary>
        /// <param name="otp">The userId parameter.</param>
        /// <returns>Returns success status or appropriate error message.</returns>

        public async Task UpdateOtpAsync(UserOtp otp)
        {
            await _transactionHelper.ExecuteAsync(async (dbContext) =>
            {
                dbContext.UserOtps.Update(otp);
                await dbContext.SaveChangesAsync();
            });
        }
    }
}
