using Xunit;
using Moq;
using System.Collections.Generic;
using DataService.Models;
using DataService.Data;
using DataService.Controllers;
using DataService.Dtos;
using DataService.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using DataService.Models.Authenticate;
using DataService.AsyncDataServices;

namespace FalconServices.Tests
{
    public class DataServiceTest
    {
        private readonly Mock<IDataRepo> _dataRepo;
        private readonly Mock<IMessageBusClient> _messageBusClient;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configuration;
        public DataServiceTest()
        {
            _dataRepo = new Mock<IDataRepo>();
            _messageBusClient = new Mock<IMessageBusClient>();
            _configuration = new MapperConfiguration(config => {
                config.AddProfile<DatasProfile>();
            });
            _mapper = _configuration.CreateMapper();
        }

        #region Unit testovi
        [Fact]
        //naming convention MethodName_expectedBehavior_StateUnderTest
        public void GetDirektori_Sucess_OKObjectResult()
        {
            //arrange
            _dataRepo.Setup(x => x.GetAllDirektori())
                .Returns(GetDirektoriData);
            var controller = new DirektorController(_dataRepo.Object, _mapper);

            //act
            var actionResult = controller.GetDirektori();
            var result = actionResult.Result as OkObjectResult;

            //assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void GetDirektori_Sucess_StatusCodeOK()
        {
            //arrange
            _dataRepo.Setup(x => x.GetAllDirektori())
                .Returns(GetDirektoriData);
            var controller = new DirektorController(_dataRepo.Object, _mapper);

            //act
            var actionResult = controller.GetDirektori();
            var result = actionResult.Result as OkObjectResult;

            //assert
            Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);
        }
        [Fact]
        public void GetDirektori_NotNull_Result()
        {
            //arrange
            _dataRepo.Setup(x => x.GetAllDirektori())
                .Returns(GetDirektoriData);
            var controller = new DirektorController(_dataRepo.Object, _mapper);

            //act
            var actionResult = controller.GetDirektori();
            var result = actionResult.Result as OkObjectResult;

            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public void GetDirektori_IsType_DirektorReadDto()
        {
            //arrange
            _dataRepo.Setup(x => x.GetAllDirektori())
                .Returns(GetDirektoriData);
            var controller = new DirektorController(_dataRepo.Object, _mapper);

            //act
            var actionResult = controller.GetDirektori();
            var result = actionResult.Result as OkObjectResult;
            var actual = result?.Value as IEnumerable<DirektorReadDto>;

            //assert
            Assert.IsType<List<DirektorReadDto>>(result?.Value);
        }
        [Fact]
        public void GetDirektorById_NotFound_DirektorWithIdNotExists()
        {
            //arrange
            var direktoriReadDto = GetDirektoriReadDtoData();

            _dataRepo.Setup(x => x.GetAllDirektori())
                .Returns(GetDirektoriData);
            var controller = new DirektorController(_dataRepo.Object, _mapper);

            //act
            var actionResult = controller.GetDirektorById(100);
            var result = actionResult.Result;

            //assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Integracijski testovi
        //TO DO: napraviti integracijski test: poziv na autorizacija/dataservice/filmcontroller
        //TO DO: napraviti integracijski test: poziv na endpoint
        //TO DO: napraviti integracijski test: provjera DB-a

        #endregion

        #region Data Moq
        private List<Direktor> GetDirektoriData()
        {
            List<Direktor> direktori = new List<Direktor>
            {
                new Direktor
                {
                    Id = 1,
                    Ime = "Alfred",
                    Prezime = "Hitchcock",
                    Adresa = "SAD"
                }
            };
            return direktori;
        }

        private List<DirektorReadDto> GetDirektoriReadDtoData()
        {
            List<DirektorReadDto> direktori = new List<DirektorReadDto>
            {
                new DirektorReadDto
                {
                    Id = 1,
                    Ime = "Alfred",
                    Adresa = "SAD"
                }
            };
            return direktori;
        }

        private List<AuthenticateResponse> GetAuthenticateResponseData()
        {
            List<AuthenticateResponse> korisnici = new List<AuthenticateResponse>
                {
                    new AuthenticateResponse
                    {
                        Id = 1,
                        FirstName = "SuperKorisnik",
                        LastName = "Admin",
                        UserName = "testAdmin",
                        Role = "Admin",
                        JwtToken = ""
                    }
                };
            return korisnici;
        }

        private AuthenticateRequest GetAuthenticateRequestData()
        {
            return new AuthenticateRequest
            {
                Username = "glumac",
                Password =  "user"
            };
        }
        #endregion

    }

}