using System.ComponentModel.DataAnnotations;

namespace DataService.Models.Authenticate;
public class AuthenticateRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}