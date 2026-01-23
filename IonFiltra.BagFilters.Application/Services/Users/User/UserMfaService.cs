using IonFiltra.BagFilters.Application.Interfaces.Users.User;
using IonFiltra.BagFilters.Core.Interfaces.Users.User;
using OtpNet;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Services.Users.User
{
    public class UserMfaService : IUserMfaService
    {
        private readonly IUserMfaRepository _userMfaRepository;

        public UserMfaService(IUserMfaRepository userMfaRepository)
        {
            _userMfaRepository = userMfaRepository;
        }


        /// <summary>
        /// EnableMfaAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="secret">The secret parameter.</param>
        /// <returns>Returns success status or appropriate error message.</returns>

        public async Task EnableMfaAsync(int userId, string secret)
        {
            await _userMfaRepository.SetMfaSecretAsync(userId, secret);
            await _userMfaRepository.EnableMfaAsync(userId, true);
        }

        /// <summary>
        /// VerifyMfaCodeAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="totpCode">The totpCode parameter.</param>
        /// <returns>Returns the UserMfa details for the specified project.</returns>

        public async Task<bool> VerifyMfaCodeAsync(int userId, string totpCode)
        {
            var secret = await _userMfaRepository.GetMfaSecretAsync(userId);
            if (string.IsNullOrEmpty(secret)) return false;

            var key = Base32Encoding.ToBytes(secret);
            var totp = new Totp(key);

            return totp.VerifyTotp(totpCode, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }

        /// <summary>
        /// GenerateQrCodeAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <param name="userEmail">The userEmail parameter.</param>
        /// <returns>Returns the UserMfa details for the specified project.</returns>
        public async Task<(int ResolvedUserId, string QrCodeBase64)> GenerateQrCodeAsync(int? userId, string userEmail)
        {
            // Fetch existing secret with possible userId resolution
            var (resolvedUserId, existingSecret) = await _userMfaRepository.GetMfaSecretAsync(userId, userEmail);

            if (resolvedUserId == 0)
            {
                throw new Exception("User not found for MFA setup.");
            }

            if (string.IsNullOrEmpty(existingSecret))
            {
                // Generate new secret
                var secret = KeyGeneration.GenerateRandomKey(20);
                existingSecret = Base32Encoding.ToString(secret);

                // Ensure the secret is stored BEFORE generating QR Code
                await _userMfaRepository.SetMfaSecretAsync(resolvedUserId, userEmail, existingSecret);
                await _userMfaRepository.EnableMfaAsync(resolvedUserId, userEmail, true);
            }

            // Generate QR Code using the correct (existing or new) secret
            var issuer = "IONFILTRA";
            var otpAuthUri = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(userEmail)}?secret={existingSecret}&issuer={Uri.EscapeDataString(issuer)}";

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpAuthUri, QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                var qrBitmap = qrCode.GetGraphic(20);
                var qrCodeBase64 = Convert.ToBase64String(qrBitmap);

                // Return both resolved user ID and the QR code
                return (resolvedUserId, qrCodeBase64);
            }
        }

        /// <summary>
        /// ResetQrCodeAsync method for UserMfa.
        /// </summary>
        /// <param name="userId">The userId parameter.</param>
        /// <returns>Returns the UserMfa details for the specified project.</returns>
        public async Task<bool> ResetQrCodeAsync(int userId)
        {
            // Call repository method to update MfaEnabled to 0
            var isUpdated = await _userMfaRepository.ResetQrCodeAsync(userId);

            return isUpdated;
        }

    }
}
