using AutoMapper;
using Identity.Data;
using Identity.IdentityServices;
using Identity.Models.Authenticate;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FalconServices.Tests
{
    public  class IdentityServiceTest
    {
        private readonly Mock<IIdentityRepo> identityRepo;
        private readonly Mock<IUserService> userservice;
        private readonly Mock<IMapper> mapper;

        public IdentityServiceTest()
        {
            identityRepo = new Mock<IIdentityRepo>();
            userservice = new Mock<IUserService>();
            mapper = new Mock<IMapper>();
        }


        #region Data
        private List<User> GetUsersData()
        {
            List<User> korisnici = new List<User>
                {
                    new User
                    {
                        Id = 1,
                        FirstName = "SuperKorisnik",
                        LastName = "Admin",
                        Username = "testAdmin",
                        Role = "Admin",
                        Password = "Admin"
                    }
                };
            return korisnici;
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
        #endregion
    }

  

}
