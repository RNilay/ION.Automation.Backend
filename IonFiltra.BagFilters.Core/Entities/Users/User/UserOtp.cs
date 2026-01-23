using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.Users.User
{
    public class UserOtp
    {
        [Key]
        public int OtpId { get; set; }                 // Primary Key
        public int UserId { get; set; }               // Foreign Key to User
        public string Otp { get; set; }               // The 6-digit OTP code
        public DateTime CreatedAt { get; set; }       // When the OTP was created
        public DateTime ExpiresAt { get; set; }       // When the OTP will expire
        public bool IsUsed { get; set; } = false;     // Indicates if the OTP has been used

        // Navigation Property
        public UserAccount User { get; set; }
    }
}
