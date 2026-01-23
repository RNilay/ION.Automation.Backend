using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Interfaces.Users.User
{
    public interface IUserMfaService
    {

        Task EnableMfaAsync(int userId, string secret);
        Task<bool> VerifyMfaCodeAsync(int userId, string totpCode);
        //Task<string> GenerateQrCodeAsync(int userId, string userEmail);
        Task<(int ResolvedUserId, string QrCodeBase64)> GenerateQrCodeAsync(int? userId, string userEmail);
        Task<bool> ResetQrCodeAsync(int userId);
    }
}
