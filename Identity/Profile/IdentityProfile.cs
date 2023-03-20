using AutoMapper;
using Identity.Models.Authenticate;

namespace Identity.Profiles;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        //Source --> Target
        CreateMap<AuthenticateRequest, GrpcUserModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
        CreateMap<GrpcUserModel, AuthenticateRequest>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

        CreateMap<AuthenticateResponse, GrpcIdentityModel>()
            .ForMember(dest => dest.IdentityId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Jwttoken, opt => opt.MapFrom(src => src.JwtToken));

        CreateMap<User, GrpcIdentityModel>()
             .ForMember(dest => dest.IdentityId, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.FirstName))
             .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
             .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        CreateMap<GrpcIdentityModel, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdentityId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Firstname))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Lastname))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
    }

}