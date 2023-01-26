using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;
using WebCalcAPI.Models.Users;

namespace WebCalcAPI.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly IOptionsMonitor<JwtOptions> _jwtOptionsMonitor;
    private readonly IOptions<List<UserModel>> _userOptionsMonitor;

    public AuthenticateService(IOptionsMonitor<JwtOptions> jwtOptionsMonitor,
        IOptions<List<UserModel>> userOptionsMonitor)
    {
        _jwtOptionsMonitor = jwtOptionsMonitor;
        _userOptionsMonitor = userOptionsMonitor;
    }

    public string GenerateJwtToken(UserModel user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptionsMonitor.CurrentValue.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(
            _jwtOptionsMonitor.CurrentValue.Issuer,
            _jwtOptionsMonitor.CurrentValue.Audience,
            claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public UserModel? Authenticate(UserLogin? userLogin)
    {
        var validUsers = _userOptionsMonitor.Value;
        if (userLogin is null) return null;
        return validUsers.FirstOrDefault(user =>
            user.UserName == userLogin.UserName && user.Password == userLogin.Password);
    }
}