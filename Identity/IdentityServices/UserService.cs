using AutoMapper;
using Identity.Data;
using Identity.Helpers;
using Identity.Models.Authenticate;
using Microsoft.Extensions.Options;
using BCrypt.Net;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Identity.IdentityServices;

public class UserService : IUserService
{
    private readonly IIdentityRepo _repository;
    private readonly IMapper _mapper;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;

     public UserService(
        IIdentityRepo repository, IMapper mapper, 
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings)
    {
        _repository= repository;
        _mapper = mapper;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _repository.GetUserByUserName(model.Username);
        Console.WriteLine($"--> Reciving Authenticate {JsonSerializer.Serialize(user)}...");
        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            //throw new AppException("Username or password is incorrect");     
            return null;       
        }
        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        //var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        //user.RefreshTokens.Add(refreshToken);

        // remove old refresh tokens from user
        //removeOldRefreshTokens(user);

        // save changes to db
        //_context.Update(user);
        //_context.SaveChanges();

        return new AuthenticateResponse(user, jwtToken);
    }
    public AuthenticateResponse ValidateToken(string token)
    {
        Console.WriteLine($"--> Reciving token {JsonSerializer.Serialize(token)}...");
        JwtSecurityToken jwtToken = _jwtUtils.ValidateJwtToken(token);
        Console.WriteLine($"--> Reciving JWTtoken {JsonSerializer.Serialize(jwtToken)}...");
        AuthenticateResponse response = new AuthenticateResponse();

        if(jwtToken != null)
        {
            
            response.Id = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            response.FirstName = jwtToken.Claims.First(x => x.Type == "FirstName").Value;
            response.LastName = jwtToken.Claims.First(x => x.Type == "LastName").Value;
            response.UserName = jwtToken.Claims.First(x => x.Type == "Username").Value;
            response.JwtToken = "";
            response.Role = jwtToken.Claims.First(x => x.Type == "Role").Value;
        }
        else
        {
            response = null;
        }

        return response;
    }

    //public IEnumerable<User> GetAll()
    //{
    //    throw new NotImplementedException();
    //}

    //public User GetById(int id)
    //{
    //    throw new NotImplementedException();
    //}
}