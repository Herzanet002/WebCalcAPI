using WebCalcAPI.Models.Users;

namespace WebCalcAPI.Contracts.Services
{
    public interface IAuthenticateService
    {
        string GenerateJwtToken(UserModel model);

        UserModel? Authenticate(UserLogin? userLogin);
    }
}