using IonFiltra.BagFilters.Core.Entities.Users.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Interfaces.Users.User
{
    public interface IOtpRepository
    {
        Task SaveOtpAsync(UserOtp otp);                    // Save a new OTP
        Task<UserOtp> GetLatestOtpForUserAsync(int userId); // Get the latest OTP for a user
        Task UpdateOtpAsync(UserOtp otp);                  // Update an existing OTP record
    }
}
