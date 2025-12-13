using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentVerse.WebAPI.Common;
using TalentVerse.WebAPI.Data.Entities;
using TalentVerse.WebAPI.DTO.Account;
using TalentVerse.WebAPI.Interfaces;

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
}
