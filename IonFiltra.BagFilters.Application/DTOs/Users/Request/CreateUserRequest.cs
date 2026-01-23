using IonFiltra.BagFilters.Application.DTOs.Users.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Users.Request
{
    public class CreateUserRequest
    {
        public UserDto UserDto { get; set; }
        public string salt { get; set; }

        public Boolean IsEncrypted { get; set; }
    }

    public class ResetPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string salt { get; set; }
        public Boolean IsEncrypted { get; set; }
    }
}
