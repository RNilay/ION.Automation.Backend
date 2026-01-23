using IonFiltra.BagFilters.Core.Entities.Users.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.Users.User
{
    public class UserAccount
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PasswordSalt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

       
        // MFA-related fields
        public string? MfaSecret { get; set; } // TOTP secret key
        public bool MfaEnabled { get; set; } = false; // Indicates if MFA is enabled

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Property: A user can have multiple roles (through UserRole)
        //[JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; }

  
    }
}
