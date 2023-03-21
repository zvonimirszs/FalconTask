
using AutoMapper;
using Identity;
using DataService.Dtos;
using DataService.Models;
using DataService.Models.Authenticate;

namespace DataService.Profiles
{
    public class DatasProfile : Profile
    {
        public DatasProfile()
        {
            // Source -> Target
            CreateMap<Zanr, ZanrReadDto>()
                    .ForMember(dest => dest.Naziv, opt => opt.MapFrom(src => src.Naziv));
            CreateMap<Zanr, GrpcZanrModel>()
                .ForMember(dest => dest.ZanrId, opt => opt.MapFrom(src =>src.Id))
                .ForMember(dest => dest.Opis, opt => opt.MapFrom(src =>src.Opis))
                .ForMember(dest => dest.Naziv, opt => opt.MapFrom(src =>src.Naziv));

            CreateMap<Direktor, DirektorReadDto>()
                    .ForMember(dest => dest.Ime, opt => opt.MapFrom(src => src.Ime))
                    .ForMember(dest => dest.Adresa, opt => opt.MapFrom(src => src.Adresa));
            CreateMap<User, DirektorReadDto>()
                    .ForMember(dest => dest.Ime, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<DirektorCreateDto, Direktor>();
            CreateMap<Glumac, GlumacReadDto>()
                    .ForMember(dest => dest.Ime, opt => opt.MapFrom(src => src.Ime))
                    .ForMember(dest => dest.Prezime, opt => opt.MapFrom(src => src.Prezime))
                    .ForMember(dest => dest.Adresa, opt => opt.MapFrom(src => src.Adresa));
            CreateMap<GlumacCreateDto, Glumac>();

            CreateMap<Film, FilmReadDto>();
            CreateMap<FilmCreateDto, Film>();
            CreateMap<Film, FilmPublishedDto>(); ;

            CreateMap<GrpcIdentityModel, AuthenticateResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdentityId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Lastname))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.JwtToken, opt => opt.MapFrom(src => src.Jwttoken));
            CreateMap<AuthenticateRequest, GrpcUserModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src =>src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src =>src.Password));

            CreateMap<User, AuthenticateResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));            
            CreateMap<AuthenticateResponse, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));

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
}