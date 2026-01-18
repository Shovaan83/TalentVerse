using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentVerse.WebAPI.Common;
using TalentVerse.WebAPI.Data.Entities;
using TalentVerse.WebAPI.DTO.Account;
using TalentVerse.WebAPI.Interfaces;
using TalentVerse.WebAPI.Services;

namespace TalentVerse.WebAPI.Controllers;

[Route("api/[controller]")] // URL = api/account
[ApiController]

public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<UserDto>>> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResponse<UserDto>.FailureResponse("Validation Failed"));
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email.ToLower()))
            {
                return BadRequest(ServiceResponse<UserDto>.FailureResponse(AppConstant.ErrorMessages.UserExists));
            }

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                Bio = registerDto.Bio
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!createdUser.Succeeded)
            {
                var errors = createdUser.Errors.Select(e => e.Description).ToList();
                return BadRequest(ServiceResponse<UserDto>.FailureResponse("User creation failed", errors));
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, AppConstant.Roles.Member);

            if (!roleResult.Succeeded)
            {
                return BadRequest(ServiceResponse<UserDto>.FailureResponse(AppConstant.ErrorMessages.RoleAssignmentFailed));
            }

            var userDto = new UserDto
            {
                Username = appUser.UserName,
                Email = appUser.Email,
                Bio = appUser.Bio,
                Token = await _tokenService.CreateToken(appUser)
            };

            return Ok(ServiceResponse<UserDto>.SuccessResponse(userDto, AppConstant.SuccessMessages.RegistrationSuccessful));
        }
            catch (Exception e)
            {
                _logger.LogError(e, "Registration Error");
                return StatusCode(500, ServiceResponse<UserDto>.FailureResponse(AppConstant.ErrorMessages.GenerricError));
            }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<UserDto>>> Login(LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResponse<UserDto>.FailureResponse("Validation Failed"));
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

            if (user == null)
            {
                return Unauthorized(ServiceResponse<UserDto>.FailureResponse(AppConstant.ErrorMessages.InvalidLogin));
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized(ServiceResponse<UserDto>.FailureResponse(AppConstant.ErrorMessages.InvalidLogin));
            }

            if (user.TwoFactorEnabled)
            {
                // Generate and send 2FA code
                var twoFactorService = HttpContext.RequestServices.GetRequiredService<ITwoFactorService>();
                var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
                
                var code = twoFactorService.GenerateCode();
                await twoFactorService.StoreCodeAsync(user.Id, code);

                var emailBody = $@"Hello {user.UserName},

Your Two-Factor Authentication code for login is: {code}

This code will expire in 10 minutes.

If you didn't request this code, please secure your account immediately.

Best regards,
TalentVerse Team";

                await emailService.SendEmailAsync(user.Email, "Your TalentVerse Login Code", emailBody);
                
                _logger.LogInformation($"2FA code sent to {user.Email} for login");
                
                return Ok(ServiceResponse<UserDto>.SuccessResponse(new UserDto
                {
                    Email = user.Email,
                    IsTwoFactorRequired = true
                }, $"2FA code sent to {user.Email}. Please check your email."));
            }

            var userDto = new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureURL,
                Token = await _tokenService.CreateToken(user)
            };

            return Ok(ServiceResponse<UserDto>.SuccessResponse(userDto, AppConstant.SuccessMessages.LoginSuccessful));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Login Error");
            return StatusCode(500, ServiceResponse<UserDto>.FailureResponse(AppConstant.ErrorMessages.GenerricError));
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<ServiceResponse<CurrentUserDto>>> GetCurrentUser()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(ServiceResponse<CurrentUserDto>.FailureResponse("User not found."));

            var dto = new CurrentUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureURL
            };

            return Ok(ServiceResponse<CurrentUserDto>.SuccessResponse(dto));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get current user error");
            return StatusCode(500, ServiceResponse<CurrentUserDto>.FailureResponse(AppConstant.ErrorMessages.GenerricError));
        }
    }

    [Authorize]
    [HttpPost("request-2fa-code")]
    public async Task<ActionResult<ServiceResponse<string>>> RequestTwoFactorCode([FromServices] ITwoFactorService twoFactorService, [FromServices] IEmailService emailService)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(ServiceResponse<string>.FailureResponse("User not found."));

            var code = twoFactorService.GenerateCode();
            await twoFactorService.StoreCodeAsync(user.Id, code);

            var emailBody = $@"Hello {user.UserName},

Your Two-Factor Authentication code is: {code}

This code will expire in 10 minutes.

If you didn't request this code, please ignore this email.

Best regards,
TalentVerse Team";

            await emailService.SendEmailAsync(user.Email, "Your TalentVerse 2FA Code", emailBody);

            _logger.LogInformation($"2FA code sent to {user.Email}");

            return Ok(ServiceResponse<string>.SuccessResponse("Code sent", $"A verification code has been sent to {user.Email}"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Request 2FA Code Error");
            return StatusCode(500, ServiceResponse<string>.FailureResponse(AppConstant.ErrorMessages.GenerricError));
        }
    }

    [Authorize]
    [HttpPost("enable-2fa")]
    public async Task<ActionResult<ServiceResponse<bool>>> EnableTwoFactor([FromBody] VerifyCodeDto verifyDto, [FromServices] ITwoFactorService twoFactorService)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(ServiceResponse<bool>.FailureResponse("User not found."));

            var code = verifyDto.Code.Trim();
            
            if (code.Length != 6 || !code.All(char.IsDigit))
            {
                _logger.LogWarning($"Invalid code format for user {user.Email}");
                return BadRequest(ServiceResponse<bool>.FailureResponse("Code must be exactly 6 digits."));
            }

            var isValid = await twoFactorService.ValidateCodeAsync(user.Id, code);

            if (!isValid)
            {
                _logger.LogWarning($"2FA verification failed for user {user.Email}");
                return BadRequest(ServiceResponse<bool>.FailureResponse("Invalid or expired code. Please request a new code."));
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation($"2FA enabled successfully for user {user.Email}");

            return Ok(ServiceResponse<bool>.SuccessResponse(true, "Two-Factor Authentication has been enabled successfully."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Enable 2FA Error");
            return StatusCode(500, ServiceResponse<bool>.FailureResponse(AppConstant.ErrorMessages.GenerricError));
        }
    }

    [HttpPost("login-2fa")]
    public async Task<ActionResult<ServiceResponse<UserDto>>> LoginWith2FA(VerifyTwoFactorDto verifyDto, [FromServices] ITwoFactorService twoFactorService)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == verifyDto.Email.ToLower());

        if (user == null || !user.TwoFactorEnabled)
            return Unauthorized(ServiceResponse<UserDto>.FailureResponse("Invalid request"));

        var code = verifyDto.Code.Trim();

        if (code.Length != 6 || !code.All(char.IsDigit))
            return BadRequest(ServiceResponse<UserDto>.FailureResponse("Code must be exactly 6 digits"));

        var isValid = await twoFactorService.ValidateCodeAsync(user.Id, code);

        if (!isValid)
            return Unauthorized(ServiceResponse<UserDto>.FailureResponse("Invalid or expired code"));

        var userDto = new UserDto
        {
            Username = user.UserName,
            Email = user.Email,
            Bio = user.Bio,
            ProfilePictureUrl = user.ProfilePictureURL,
            Token = await _tokenService.CreateToken(user)
        };

        return Ok(ServiceResponse<UserDto>.SuccessResponse(userDto, "Login Successful via 2FA"));
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto, [FromServices] ITwoFactorService twoFactorService, [FromServices] IEmailService emailService)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email.ToLower());

            if (user == null)
            {

                return Ok(ServiceResponse<string>.SuccessResponse("", "If the email exists, a password reset code has been sent."));
            }

            // Generate reset code
            var code = twoFactorService.GenerateCode();
            await twoFactorService.StoreCodeAsync(user.Id, code);

            var emailBody = $@"Hello {user.UserName},

You requested to reset your password for TalentVerse.

Your password reset code is: {code}

This code will expire in 10 minutes.

If you didn't request this, please ignore this email and secure your account.

Best regards,
TalentVerse Team";

            await emailService.SendEmailAsync(user.Email, "Password Reset Code - TalentVerse", emailBody);

            _logger.LogInformation($"Password reset code sent to {user.Email}");

            return Ok(ServiceResponse<string>.SuccessResponse("", "If the email exists, a password reset code has been sent."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Forgot Password Error");
            return StatusCode(500, ServiceResponse<string>.FailureResponse("An error occurred. Please try again later."));
        }
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ServiceResponse<string>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, [FromServices] ITwoFactorService twoFactorService)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResponse<string>.FailureResponse("Validation Failed"));
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == resetPasswordDto.Email.ToLower());

            if (user == null)
            {
                return BadRequest(ServiceResponse<string>.FailureResponse("Invalid request"));
            }

            var code = resetPasswordDto.Code.Trim();

            if (code.Length != 6 || !code.All(char.IsDigit))
            {
                return BadRequest(ServiceResponse<string>.FailureResponse("Code must be exactly 6 digits"));
            }

            // Validate the code
            var isValid = await twoFactorService.ValidateCodeAsync(user.Id, code);

            if (!isValid)
            {
                _logger.LogWarning($"Invalid password reset code for user {user.Email}");
                return BadRequest(ServiceResponse<string>.FailureResponse("Invalid or expired code"));
            }

            // Remove current password and set new one
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ServiceResponse<string>.FailureResponse("Password reset failed", errors));
            }

            _logger.LogInformation($"Password reset successful for user {user.Email}");

            return Ok(ServiceResponse<string>.SuccessResponse("", "Password has been reset successfully. You can now login with your new password."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Reset Password Error");
            return StatusCode(500, ServiceResponse<string>.FailureResponse("An error occurred. Please try again later."));
        }
    }
}
