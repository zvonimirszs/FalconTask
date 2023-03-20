using AutoMapper;
using DataService.Attributes.Authorization;
using DataService.Data;
using DataService.Dtos;
using DataService.Models;
using DataService.Models.Authenticate;
using Microsoft.AspNetCore.Mvc;

namespace DataService.Controllers
{
    [ApiController]
    public class DirektorController : Controller
    {
        private readonly IDataRepo _repository;
        private readonly IMapper _mapper;
        public DirektorController(IDataRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Authorize]
        [Route("api/podaci/direktor")]
        [HttpGet]
        public ActionResult<IEnumerable<DirektorReadDto>> GetDirektori()
        {
            Console.WriteLine("--> Getting Direktori...");

            var direktori = _repository.GetAllDirektori();

            return Ok(_mapper.Map<IEnumerable<DirektorReadDto>>(direktori));
        }

        [Authorize]
        [Route("api/podaci/direktor/{id}")]
        [HttpGet]
        public ActionResult<DirektorReadDto> GetDirektorById(int id)
        {
            Console.WriteLine($"--> Getting Direktor By Direktor Id: {id}...");

            var direktor = _repository.GetDirektorById(id);
            if (direktor == null)
            {
                return NotFound();
            }  
            return Ok(_mapper.Map<DirektorReadDto>(direktor));
        }

        [Authorize]
        [Route("api/podaci/direktor/")]
        [HttpPost]
        public ActionResult Post(DirektorCreateDto direktorCreateDto)
        {
            Console.WriteLine($"--> Hit CreateDirektor!");
            var key = direktorCreateDto.Ime + direktorCreateDto.Prezime;
            if (_repository.DirektorExists(key))
            {
                return Conflict($"--> Direktor sa tim imenom i prezimenom već postoji");
            }
            var direktor = _mapper.Map<Direktor>(direktorCreateDto);

            _repository.CreateDirektor(direktor);
            _repository.SaveChanges();
            //TO DO:  kreirati User objekt u Identity servisu (može: MQ ili grpc)
            User user = new User
            {
                FirstName = direktor.Ime,
                Id = direktor.Id,
                LastName = direktor.Prezime,
                Username = key,
                Role = "Direktor"
            };
            var userCreated = _repository.CreateUser(user);
            return Ok(_mapper.Map<DirektorReadDto>(userCreated));
        }

        [Authorize]
        [Route("api/podaci/direktor/{id}")]
        [HttpPut]
        public ActionResult<DirektorReadDto> Put(int id, DirektorCreateDto direktorCreateDto)
        {
            Console.WriteLine($"--> Update Direktor : {id}...");
            var direktor = _mapper.Map<Direktor>(direktorCreateDto);
            if ((direktor == null) || (id != direktor.Id))
            {
                return BadRequest($"--> Direktor objekt je krivi ");
            }
            _repository.UpdateDirektor(direktor);
            _repository.SaveChanges();
            return Ok(_mapper.Map<DirektorReadDto>(direktor));
        }

        [Authorize]
        [Route("api/podaci/direktor/{id}")]
        [HttpDelete]
        public ActionResult<DirektorReadDto> Delete(int id)
        {
            Console.WriteLine($"--> Delete Direktor: {id}...");
            var direktor = _repository.GetDirektorById(id);
            if (direktor == null)
            {
                return NotFound();
            }
            //TO DO: provjeriti da li direktor je na nekom filmu (ako je NEMA brisanja) 
            _repository.DeleteDirektor(direktor);
            _repository.SaveChanges();
            return Ok(_mapper.Map<DirektorReadDto>(direktor));
        }
    }
}
