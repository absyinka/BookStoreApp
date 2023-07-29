using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookStoreApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger;
    private readonly IMapper mapper;
    private readonly UserManager<ApiUser> userManager;
    private readonly IConfiguration configuration;

    public AuthController(
        ILogger<AuthController> logger,
        IMapper mapper,
        UserManager<ApiUser> userManager,
        IConfiguration configuration)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.userManager = userManager;
        this.configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
        try
        {
            var user = mapper.Map<ApiUser>(request);
            user.UserName = request.Email;

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded == false)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }

                logger.LogWarning($"{Messages.GeneralErrorMessage} {nameof(Register)}");
                return BadRequest(ModelState);
            }

            await userManager.AddToRoleAsync(user, "User");

            return Accepted();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{Messages.GeneralErrorMessage} {nameof(Register)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginUserDto request)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (user is null || isPasswordValid == false)
            {
                logger.LogWarning($"{Messages.InvalidCredential} {nameof(Login)}");
                return Unauthorized($"{Messages.InvalidCredential}");
            }

            string tokenString = await GenerateToken(user);

            var response = new AuthResponse
            {
                UserId = user.Id,
                Email = request.Email,
                Token = tokenString,
            };

            return Accepted(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{Messages.GeneralErrorMessage} {nameof(Login)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    private async Task<string> GenerateToken(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
        var userClaims = await userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(CustomClaimTypes.Uid, user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(int.Parse(configuration["JwtSettings:Duration"])),
            signingCredentials: credentials
         );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
