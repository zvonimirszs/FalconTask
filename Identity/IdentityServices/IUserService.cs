using System.IdentityModel.Tokens.Jwt;
using Identity.Models.Authenticate;

namespace Identity.IdentityServices;
public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    AuthenticateResponse ValidateToken(string token);
    User CreateUser(User user);
    User UpdateUser(User user);
}