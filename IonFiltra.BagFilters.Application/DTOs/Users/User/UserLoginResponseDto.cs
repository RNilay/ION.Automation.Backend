using IonFiltra.BagFilters.Core.Entities.Users.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Users.User
{
    public class UserLoginResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; }

        public UserAccount UserAccountObj { get; set; }
    }
}
