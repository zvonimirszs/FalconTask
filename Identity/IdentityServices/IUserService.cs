using System.IdentityModel.Tokens.Jwt;
using Identity.Models.Authenticate;

namespace Identity.IdentityServices;
public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    AuthenticateResponse ValidateToken(string token);
    //IEnumerable<User> GetAll();
    //User GetById(int id);
}