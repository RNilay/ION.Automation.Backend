using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Interfaces.Users.User
{
    public interface IUserMfaRepository
    {

        Task SetMfaSecretAsync(int userId, string secret);
        Task SetMfaSecretAsync(int? userId, string userEmail, string secret);
        Task EnableMfaAsync(int userId, bool isEnabled);
        Task EnableMfaAsync(int? userId, string userEmail, bool isEnabled);
        Task<string> GetMfaSecretAsync(int userId);
        Task<(int UserId, string MfaSecret)> GetMfaSecretAsync(int? userId, string userEmail);
        Task<bool> ResetQrCodeAsync(int userId);

    }
}
