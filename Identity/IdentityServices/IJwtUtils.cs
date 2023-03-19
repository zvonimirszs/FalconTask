using System.IdentityModel.Tokens.Jwt;
using Identity.Models.Authenticate;

namespace Identity.IdentityServices;
public interface IJwtUtils
{
    public string GenerateJwtToken(User user);
    public JwtSecurityToken ValidateJwtToken(string token);
}