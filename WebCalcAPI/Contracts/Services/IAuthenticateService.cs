using WebCalcAPI.Models.Users;

namespace WebCalcAPI.Contracts.Services
{
    public interface IAuthenticateService
    {
        public string GenerateJwtToken(UserModel model);

        public UserModel? Authenticate(UserLogin? userLogin);
    }
}