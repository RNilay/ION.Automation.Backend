
using IonFiltra.BagFilters.Application.DTOs.Users.User;
using IonFiltra.BagFilters.Application.Interfaces.Users.User;
using Microsoft.AspNetCore.Mvc;
using IonFiltra.BagFilters.Application.DTOs.Users.Request;
using IonFiltra.BagFilters.Core.Entities.Users.User;

namespace IonFiltra.BagFilters.Api.Controllers.Users.User
{
    /// <summary>
    /// Controller class for MfaController.
    /// </summary>
    [Route("api/mfa")]
    [ApiController]
    public class MfaController : ControllerBase
    {

        private readonly IUserMfaService _userMfaService;
        private readonly ILogger<MfaController> _logger;

        public MfaController(IUserMfaService userMfaService, ILogger<MfaController> logger)
        {
            _userMfaService = userMfaService;
            _logger = logger;
        }
        /// <summary>
        /// EnableMfa method.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("enable")]
        public async Task<IActionResult> EnableMfa([FromBody] MfaSetupRequest request)
        {
            _logger.LogInformation("EnableMfa started with Request {request}", new object[] { request });
            var (resolvedUserId, qrCodeBase64) = await _userMfaService.GenerateQrCodeAsync(
                request.UserId > 0 ? request.UserId : (int?)null,
                request.UserEmail
            );

            return Ok(new { ResolvedUserId = resolvedUserId, QrCodeBase64 = qrCodeBase64 });
        }

        /// <summary>
        /// VerifyMfa method.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyMfa([FromBody] MfaVerificationRequest request)
        {
            _logger.LogInformation("VerifyMfa started with Request {request}", new object[] { request });
            var isValid = await _userMfaService.VerifyMfaCodeAsync(request.UserId, request.TotpCode);
            if (!isValid)
            {
                return Unauthorized("Invalid TOTP code.");
            }

            return Ok("Verified");
        }
        /// <summary>
        /// Resets the MFA QR code for the user.
        /// </summary>
        /// <param name="request">The ID of the user for whom to reset the QR code.</param>
        [HttpPost("resetQR")]
        public async Task<IActionResult> ResetQr([FromBody] ResetQrRequest request)
        {
            _logger.LogInformation("ResetQr started for UserId {userId}", new object[] { request });

            var result = await _userMfaService.ResetQrCodeAsync(request.UserId);
            if (!result)
            {
                return BadRequest("Failed to reset QR code.");
            }

            return Ok("QR code reset successfully.");
        }
    }
    public class ResetQrRequest
    {
        public int UserId { get; set; }
    }
}