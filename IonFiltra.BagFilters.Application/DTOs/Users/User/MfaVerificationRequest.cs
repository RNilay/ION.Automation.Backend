using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Users.User
{
    public class MfaVerificationRequest
    {
        public int UserId { get; set; }
        public string TotpCode { get; set; }
    }
}
