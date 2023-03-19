using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Authenticate;
public class AuthenticateResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public string JwtToken { get; set; }  
    public AuthenticateResponse()
    {

    }
    public AuthenticateResponse(User user, string jwtToken)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        UserName = user.Username;
        Role = user.Role;
        JwtToken = jwtToken;
    }
}