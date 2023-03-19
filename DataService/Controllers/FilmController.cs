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
    public class FilmController : Controller
    {
        private readonly IDataRepo _repository;
        private readonly IMapper _mapper;
        public FilmController(IDataRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("api/podaci/autorizacija")]
        [HttpPost]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            Console.WriteLine("--> Getting Authenticate...");
            AuthenticateResponse authenticateResponse = _repository.Authenticate(model);
            if (authenticateResponse == null)
            {
                return Unauthorized("Korisničko ime i lozinka: NISU DOBRI!");
            }
            else
            {
                return (authenticateResponse);
            }
        }

        [Authorize]
        [Route("api/podaci/film")]
        [HttpGet]
        public ActionResult<IEnumerable<FilmReadDto>> GetFilmovi()
        {
            Console.WriteLine("--> Getting Filmovi...");

            var filmovi = _repository.GetAllFilmovi();
            var filmoviReadDto = _mapper.Map<IEnumerable<FilmReadDto>>(filmovi);

            return Ok(filmoviReadDto);
        }

        [Authorize]
        [Route("api/podaci/film/{id}")]
        [HttpGet]
        public ActionResult<FilmReadDto> GetFilmById(int id)
        {
            Console.WriteLine($"--> Getting Film By Film Id: {id}...");

            var film = _repository.GetFilmById(id);
            if (film == null)
            {
                return NotFound();
            }
            var filmReadDto = _mapper.Map<FilmReadDto>(film);
            return Ok(filmReadDto);
        }

        [Authorize]
        [Route("api/podaci/film/")]
        [HttpPost]
        public ActionResult Post(FilmCreateDto filmCreateDto)
        {
            Console.WriteLine($"--> Hit CreateFilm!");
            //TO DO:  prebaciti u Profil sa AutoMapperom
            #region automapper-todo
            Film film = new Film();
            film.Id = filmCreateDto.Id;
            film.Naziv = filmCreateDto.Naziv;
            film.Opis = filmCreateDto.Opis;
            film.Budzet = filmCreateDto.Budzet;
            film.PocetakSnimanja = filmCreateDto.PocetakSnimanja;
            film.KrajSnimanja = filmCreateDto.KrajSnimanja;
            foreach (var zanrId in filmCreateDto.Zanrovi)
            {
                film.Zanrovi.Add(_repository.GetZanrById(zanrId));
            }
            foreach (var glumacId in filmCreateDto.Glumci)
            {
                film.Glumci.Add(_repository.GetGlumacById(glumacId));
            }
            film.Direktor = _repository.GetDirektorById(filmCreateDto.DirektorId);
            #endregion

            //var film = _mapper.Map<Film>(filmCreateDto);

            //TO DO: poslati asinkronu poruku na MQ

            _repository.CreateFilm(film);
            _repository.SaveChanges();

            return Ok();
        }

        [Authorize]
        [Route("api/podaci/film/{id}")]
        [HttpPut]
        public ActionResult<Film> Put(int id, FilmCreateDto filmCreateDto)
        {
            Console.WriteLine($"--> Update Film : {id}...");
            var film = _mapper.Map<Film>(filmCreateDto);
            if ((film == null) || (id != film.Id))
            {
                return BadRequest();
            }
            _repository.UpdateFilm(film);
            _repository.SaveChanges();
            return Ok(_mapper.Map<FilmReadDto>(film));
        }

        [Authorize]
        [Route("api/podaci/film/{id}")]
        [HttpDelete]
        public ActionResult<Film> Delete(int id)
        {
            Console.WriteLine($"--> Delete Film: {id}...");
            var film = _repository.GetFilmById(id);
            if (film == null)
            {
                return NotFound();
            }
            _repository.DeleteFilm(film);
            _repository.SaveChanges();
            return Ok(_mapper.Map<FilmReadDto>(film));
        }

        //[Authorize]
        [Route("api/podaci/zanrovi")]
        [HttpGet]
        public ActionResult<IEnumerable<ZanrReadDto>> GetZanrovi()
        {
            Console.WriteLine("--> Getting Zanrovi...");

            var zanrovi = _repository.GetAllZanrovi();

            return Ok(_mapper.Map<IEnumerable<ZanrReadDto>>(zanrovi));
        }
    }
}
