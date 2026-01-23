using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Users.Request
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Challenge { get; set; }

        public string salt { get; set; }

        public Boolean IsEncrypted { get; set; }

        public Boolean IsNJSAuth { get; set; } = false;
    }
}
