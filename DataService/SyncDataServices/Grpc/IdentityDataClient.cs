using System.Text.Json;
using AutoMapper;
using Grpc.Net.Client;
using Identity;
using DataService;
using DataService.Helpers;
using DataService.HelpersExceptionMiddleware.Exceptions;
using DataService.Models;
using DataService.Models.Authenticate;

namespace DataService.SyncDataServices.Grpc;
//gRPC Client
public class IdentityDataClient : IIdentityDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public IdentityDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }
    public AuthenticateResponse ReturnAuthenticateResponse(AuthenticateRequest model)
    {
        Console.WriteLine($"--> Povezivanje na GRPC Servis {_configuration["GrpcIdentity"]}. Metoda: ReturnAuthenticateResponse");

        var channel = GrpcChannel.ForAddress(_configuration["GrpcIdentity"]);
        var client = new GrpcIdentity.GrpcIdentityClient(channel);
        var request = new Identity.GrpcUserModel(_mapper.Map<GrpcUserModel>(model));
        //var request = new Identity.GetAllRequest();
        try
        {
            var reply = client.Authenticate(request);
            return _mapper.Map<AuthenticateResponse>(reply.Identity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> NIJE moguće pozvati ili povezati se na GRPC Server {ex.Message}");
            throw new ServiceException($"--> NIJE moguće pozvati ili povezati se na GRPC Server za IDENTIFIKACIJU.");
        }
    }

    public AuthenticateResponse ReturnValidateTokenResponse(string token)
    {
        Console.WriteLine($"--> Povezivanje na GRPC Servis {_configuration["GrpcIdentity"]}. Metoda: ReturnValidateTokenResponse");
        if(token == null)
        {
            return null;
        }
        var channel = GrpcChannel.ForAddress(_configuration["GrpcIdentity"]);
        var client = new GrpcIdentity.GrpcIdentityClient(channel);
        var request = new Identity.GetAllRequest();
        request.Token = token;

        try
        {
            var reply = client.ValidateToken(request);
            return _mapper.Map<AuthenticateResponse>(reply.Identity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> NIJE moguće pozvati ili povezati se na GRPC Server {ex.Message}");
            throw new ServiceException($"--> NIJE moguće pozvati ili povezati se na GRPC Server za IDENTIFIKACIJU.");
        }
    }
}