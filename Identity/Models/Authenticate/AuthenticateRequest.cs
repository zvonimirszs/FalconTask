using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Authenticate;
public class AuthenticateRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}