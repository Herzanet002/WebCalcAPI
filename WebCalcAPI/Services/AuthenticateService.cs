using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IConfiguration _config;
        public AuthenticateService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJwtToken(UserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtOptions:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["JwtOptions:Issuer"],
                audience: _config["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserModel? Authenticate(UserLogin? userLogin)
        {
            var validUsers = _config.GetSection("ValidUsers").Get<List<UserModel>>();
            if (userLogin is null) return null;
            return validUsers.FirstOrDefault(user =>
                user.UserName == userLogin.UserName && user.Password == userLogin.Password);
        }
    }
}
