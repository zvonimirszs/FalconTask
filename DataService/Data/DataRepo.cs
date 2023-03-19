//using LEX_LegalSettings;
using DataService.Models;
using DataService.Models.Authenticate;
using DataService.SyncDataServices.Grpc;

namespace DataService.Data
{
    public class DataRepo : IDataRepo
    {
        private readonly AppDbContext _context;
        private readonly IIdentityDataClient _identityservice;
        private readonly IConfiguration _configuration;

        public DataRepo(AppDbContext context, IIdentityDataClient identityservice, IConfiguration configuration)
        {
            _context = context;
            _identityservice = identityservice;
            _configuration = configuration;
        }
        public bool SaveChanges()
        {
            return(_context.SaveChanges() >= 0);
        }

        #region Authentifikacija
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var grpcClient = _identityservice;
            var auth = grpcClient.ReturnAuthenticateResponse(model);

            return auth;    
        }

        public AuthenticateResponse ValidateToken(string token)
        {
            var grpcClient = _identityservice;
            var auth = grpcClient.ReturnValidateTokenResponse(token);

            return auth;
        }
        #endregion

        #region Film
        public IEnumerable<Film> GetAllFilmovi()
        {
            return (from f in _context.Filmovi
                        select new Film
                        {
                            Id = f.Id,
                            Opis = f.Opis,
                            Naziv = f.Naziv,
                            Budzet = f.Budzet,
                            PocetakSnimanja = f.PocetakSnimanja,
                            KrajSnimanja = f.KrajSnimanja,
                            Glumci = f.Glumci,
                            Zanrovi = f.Zanrovi,
                            Direktor = f.Direktor
                        }
                    ).ToList();
        }
        public Film GetFilmById(int id)
        {
            //return _context.Filmovi
            //    .Where(c => c.Id == id).FirstOrDefault();

            return (from f in _context.Filmovi
                        select new Film
                        {
                            Id = f.Id,
                            Opis = f.Opis,
                            Naziv = f.Naziv,
                            Budzet = f.Budzet,
                            PocetakSnimanja = f.PocetakSnimanja,
                            KrajSnimanja = f.KrajSnimanja,
                            Glumci = f.Glumci,
                            Zanrovi = f.Zanrovi,
                            Direktor = f.Direktor
                        }
                    ).Where(c => c.Id == id).FirstOrDefault();
        }
        IEnumerable<Film> IDataRepo.GetFilmoviForDirektor(int direktorId)
        {
            return _context.Filmovi
                .Where(c => c.Direktor.Id == direktorId).ToList();
        }

        public Film GetFilm(int filmId)
        {
            var filmItem = _context.Filmovi
                .Where(c => c.Id == filmId).FirstOrDefault();
            //if (filmItem != null)
            //{
            //    filmItem.Direktor = _context.Direktori.FirstOrDefault(p => p.Id == filmItem.DirektorId);
            //}
            return filmItem;
        }

        public void CreateFilm(int direktorId, Film film)
        {
            if (film == null)
            {
                throw new ArgumentNullException(nameof(film));
            }
            //film.Direktor = _context.Direktori.FirstOrDefault(p => p.Id == filmItem.DirektorId);
            _context.Filmovi.Add(film);
        }

        public void CreateFilm(Film film)
        {
            if (film == null)
            {
                throw new ArgumentNullException(nameof(film));
            }
            _context.Filmovi.Add(film);
        }

        public void UpdateFilm(Film film)
        {
            _context.Filmovi.Update(film);
        }
        public void DeleteFilm(Film film)
        {
            _context.Filmovi.Remove(film);
        }
        public bool FilmExists(string key)
        {
            return _context.Filmovi.Any(p => (p.Naziv + p.Direktor.Id.ToString()) == key);
        }
        #endregion

        #region Zanr
        public IEnumerable<Zanr> GetAllZanrovi()
        {
            return (from z in _context.Zanrovi
                    select new Zanr
                    {
                        Id = z.Id,
                        Opis = z.Opis, 
                        Naziv = z.Naziv
                    }
           ).ToList();
        }

        public Zanr GetZanrById(int id)
        {
            return _context.Zanrovi.Where(c => c.Id == id).FirstOrDefault();
        }

        public void CreateZanr(Zanr zanr)
        {
            if (zanr == null)
            {
                throw new ArgumentNullException(nameof(zanr));
            }
            _context.Zanrovi.Add(zanr);
        }

        public void UpdateZanr(Zanr glumac)
        {
            throw new NotImplementedException();
        }

        public void DeleteZanr(Zanr glumac)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Direktor
        public IEnumerable<Direktor> GetAllDirektori()
        {
            var direktori = (from r in _context.Direktori
                                select new Direktor
                                {
                                    Id = r.Id,
                                    Ime = r.Ime,
                                    Prezime = r.Prezime,
                                    Adresa = r.Adresa
                                }).ToList();

            return direktori;
        }
        public Direktor GetDirektorById(int id)
        {
            return _context.Direktori
                .Where(c => c.Id == id).FirstOrDefault();
        }
        public void CreateDirektor(Direktor direktor)
        {
            if (direktor == null)
            {
                throw new ArgumentNullException(nameof(direktor));
            }
            _context.Direktori.Add(direktor);
        }
        public void UpdateDirektor(Direktor direktor)
        {
            _context.Direktori.Update(direktor);
        }
        public void DeleteDirektor(Direktor direktor)
        {
            _context.Direktori.Remove(direktor);
        }
        public bool DirektorExists(string key)
        {
            return _context.Direktori.Any(p => (p.Ime + p.Prezime) == key);
        }
        #endregion

        #region Glumac
        public IEnumerable<Glumac> GetAllGlumci()
        {
            var glumci = (from r in _context.Glumci
                             select new Glumac
                             {
                                 Id = r.Id,
                                 Ime = r.Ime,
                                 Prezime = r.Prezime,
                                 Adresa = r.Adresa,
                                 OcekivaniHonorar = r.OcekivaniHonorar
                             }).ToList();

            return glumci;
        }
        public Glumac GetGlumacById(int id)
        {
            return _context.Glumci
                .Where(c => c.Id == id).FirstOrDefault();
        }
        public void CreateGlumac(Glumac glumac)
        {
            if (glumac == null)
            {
                throw new ArgumentNullException(nameof(glumac));
            }
            _context.Glumci.Add(glumac);
        }
        public void UpdateGlumac(Glumac glumac)
        {
            _context.Glumci.Update(glumac);
        }
        public void DeleteGlumac(Glumac glumac)
        {
            _context.Glumci.Remove(glumac);
        }
        public bool GlumacExists(string key)
        {
            return _context.Glumci.Any(p => (p.Ime + p.Prezime) == key);
        }
        #endregion

    }
}