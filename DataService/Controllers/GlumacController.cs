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
    public class GlumacController : Controller
    {
        private readonly IDataRepo _repository;
        private readonly IMapper _mapper;
        public GlumacController(IDataRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Authorize]
        [Route("api/podaci/glumac/")]
        [HttpGet]
        public ActionResult<IEnumerable<GlumacReadDto>> GetGlumci()
        {
            Console.WriteLine($"--> Getting Glumci");

            var glumci = _repository.GetAllGlumci();
            return Ok(_mapper.Map<IEnumerable<GlumacReadDto>>(glumci));
        }

        [Authorize]
        [Route("api/podaci/glumac/{id}")]
        [HttpGet]
        public ActionResult<GlumacReadDto> GetGlumacById(int id)
        {
            Console.WriteLine($"--> Getting Glumac By Glumac Id: {id}...");

            var glumac = _repository.GetGlumacById(id);
            if (glumac == null)
            {
                return NotFound();
            }  
            return Ok(_mapper.Map<GlumacReadDto>(glumac));
        }

        [Authorize]
        [Route("api/podaci/glumac/")]
        [HttpPost]
        public ActionResult Post(GlumacCreateDto glumacCreateDto)
        {
            Console.WriteLine($"--> Hit CreateGlumac!");

            var glumac = _mapper.Map<Glumac>(glumacCreateDto);

            _repository.CreateGlumac(glumac);
            _repository.SaveChanges();

            //TO DO:  kreirati User objekt u Identity servisu (može: MQ ili grpc)

            return Ok();
        }

        [Authorize]
        [Route("api/podaci/glumac/{id}")]
        [HttpPut]
        public ActionResult<GlumacReadDto> Put(int id, GlumacCreateDto glumacCreateDto)
        {
            Console.WriteLine($"--> Update Glumac : {id}...");
            var glumac = _mapper.Map<Glumac>(glumacCreateDto);
            if ((glumac == null) || (id != glumac.Id))
            {
                return BadRequest();
            }
            _repository.UpdateGlumac(glumac);
            _repository.SaveChanges();
            return Ok(_mapper.Map<GlumacReadDto>(glumac));
        }

        [Authorize]
        [Route("api/podaci/glumac/{id}")]
        [HttpDelete]
        public ActionResult<GlumacReadDto> Delete(int id)
        {
            Console.WriteLine($"--> Delete Glumac: {id}...");
            var glumac = _repository.GetGlumacById(id);
            if (glumac == null)
            {
                return NotFound();
            }
            //TO DO: provjeriti da li glumac je na nekom filmu (ako je = NEMA brisanja) 
            _repository.DeleteGlumac(glumac);
            _repository.SaveChanges();
            return Ok(_mapper.Map<GlumacReadDto>(glumac));
        }
    }
}
