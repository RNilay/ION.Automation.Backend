using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Users.User
{
    public class UserLoginDto
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
