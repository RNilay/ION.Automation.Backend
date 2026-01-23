using IonFiltra.BagFilters.Core.Entities.Users.UserRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.Users.User
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; } // Foreign key to User

        [Key, Column(Order = 1)]
        public int RoleId { get; set; } // Foreign key to Role
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public string? AssignedBy { get; set; } // Who assigned this role?

        //// Navigation Properties
        //[JsonIgnore]
        //public UserAccount User { get; set; } // Navigation to User
        //[JsonIgnore]
        public ApplicationRoles Role { get; set; } // Navigation to Role

    }
}
