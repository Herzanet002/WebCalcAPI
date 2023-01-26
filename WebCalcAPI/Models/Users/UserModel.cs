namespace WebCalcAPI.Models.Users;

public class UserModel : UserLogin
{
    public string Role { get; set; } = string.Empty;
}