using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OauthLoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<OauthLoginController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public OauthLoginController(IConfiguration configuration, ILogger<OauthLoginController> logger,
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _configuration = configuration;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    private string GenerateJwtToken(List<Claim> claims)
    {
        var jwtKey = _configuration.GetValue<string>("Jwt:Key")!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        var googleToken = request.Token;
        try
        {
            var payload = await ValidateGoogleToken(googleToken);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, payload.Subject),
                new (ClaimTypes.Email, payload.Email),
                new (ClaimTypes.Name, payload.Name),
            };
            
            if (user == null)
            {
                var isFirstUser = !await _userManager.Users.AnyAsync();
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.Email,
                };
                
                await _userManager.CreateAsync(user);
                if (isFirstUser)
                {
                    await _userManager.AddToRoleAsync(user, Consts.AdminRole);
                    await _userManager.AddToRoleAsync(user, Consts.UserRole);
                    claims.Add(new (ClaimTypes.Role, Consts.AdminRole));
                }
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userRoles.ToList()
                    .ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));
            }
            
            var jwtToken = GenerateJwtToken(claims);
            return Ok(new
            {
                token = jwtToken,
                name = payload.Name,
                roles = claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value).ToArray()
            });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Failed login");
            return Unauthorized();
        }
    }

    private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string googleToken)
    {
        var googleClientId = _configuration["GoogleClientId"]!;

        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { googleClientId }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);
        return payload;
    }
}
