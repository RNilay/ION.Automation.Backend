using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Users.User
{
    public class MfaSetupRequest
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
