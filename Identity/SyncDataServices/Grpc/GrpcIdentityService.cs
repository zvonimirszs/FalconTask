using System.Text.Json;
using AutoMapper;
using Grpc.Core;
using Identity.IdentityServices;
using Identity.Models.Authenticate;

namespace Identity.SyncDataServices.Grpc
{
    public class GrpcIdentityService : GrpcIdentity.GrpcIdentityBase
    {
        private IUserService _userService;
        private readonly IMapper _mapper;

        public GrpcIdentityService(IUserService userService, IMapper mapper)
        {
             _userService = userService;
            _mapper = mapper;
        }
        public override Task<IdentityResponse> Authenticate(GrpcUserModel request, ServerCallContext context)
        {
            Console.WriteLine($"--> Getting Authenticate {JsonSerializer.Serialize(request)}...");
            var authentication = _userService.Authenticate(_mapper.Map<AuthenticateRequest>(request)); ;
            if (authentication != null)
            {
                Console.WriteLine($"--> Reciving Authenticate {JsonSerializer.Serialize(authentication)}...");
                var response = new IdentityResponse();
                response.Identity = _mapper.Map<GrpcIdentityModel>(authentication);
                Console.WriteLine($"--> Sending Authenticate Response {JsonSerializer.Serialize(response)}...");
                return Task.FromResult(response);
            }
            else
            {
                var response = new IdentityResponse();
                response.Identity = null;
                return Task.FromResult(response);
            }

        }

        public override Task<IdentityResponse> ValidateToken(GetAllRequest request, ServerCallContext context)
        {
            Console.WriteLine($"--> Getting ValidateToken {JsonSerializer.Serialize(request)}...");
            var authentication = _userService.ValidateToken(request.Token); ;
            if (authentication != null)
            {
                Console.WriteLine($"--> Reciving ValidateToken {JsonSerializer.Serialize(authentication)}...");
                var response = new IdentityResponse();
                response.Identity = _mapper.Map<GrpcIdentityModel>(authentication);
                Console.WriteLine($"--> Sending ValidateToken Response {JsonSerializer.Serialize(response)}...");
                return Task.FromResult(response);
            }
            else
            {
                var response = new IdentityResponse();
                response.Identity = null;
                return Task.FromResult(response);
            }

        }
    }
}