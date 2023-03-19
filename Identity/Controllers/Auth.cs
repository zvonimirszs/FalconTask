using Identity.IdentityServices;
using Identity.Models.Authenticate;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userservice;
    public AuthController(IUserService userservice)
    {
        _userservice = userservice;
    }
    [Route("api/identifikacija/testGet")]
    [HttpGet]
    public ActionResult Get()
    {
        Console.WriteLine("--> Getting Get Auth...");

    
        return Ok("--> Getting Get Auth...");
    }

    [Route("api/identifikacija/autorizacija")]
    [HttpPost]
    public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        AuthenticateResponse authenticateResponse = _userservice.Authenticate(model) ;
        if(authenticateResponse == null)
        {
            return Unauthorized(model);
        }
        else
        {
            return(authenticateResponse);
        }
    }
    [Route("api/identifikacija/token")]
    [HttpPost]
    public ActionResult<AuthenticateResponse> ValidateToken()
    {
        string authHeader = Request.Headers["Authorization"];  
        if(authHeader != null && authHeader.StartsWith("Bearer"))
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(token != null)
            {
                AuthenticateResponse authenticateResponse = _userservice.ValidateToken(token) ;
                if(authenticateResponse == null)
                {
                    return Unauthorized(authenticateResponse);
                }
                else
                {
                    return(authenticateResponse);
                }
            }
            else
            {
                return Unauthorized("NO TOKEN Bearer.");
            }
        }
        else
        {
            return Unauthorized("NO TOKEN.");
        }
    }
}