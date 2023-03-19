using DataService.Models.Authenticate;
using Microsoft.AspNetCore.Mvc;

namespace DataService.SyncDataServices.Grpc;
public interface IIdentityDataClient
{
    AuthenticateResponse ReturnAuthenticateResponse(AuthenticateRequest model);
    AuthenticateResponse ReturnValidateTokenResponse(string token);
}