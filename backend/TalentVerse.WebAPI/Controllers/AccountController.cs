using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                Bio = registerDto.Bio
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                return Ok (new UserDto
                {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Bio = appUser.Bio,
                    Token = await _tokenService.CreateToken(appUser)
                });
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null) return Unauthorized("Invalid email or password.");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid email or password.");

        return Ok(new UserDto
        {
            Username = user.UserName,
            Email = user.Email,
            Bio = user.Bio,
            ProfilePictureUrl = user.ProfilePictureURL,
            Token = await _tokenService.CreateToken(user)
        });
    }
}
