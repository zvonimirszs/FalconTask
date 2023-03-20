using System;
using System.Collections.Generic;
using DataService.Dtos;
//using LEX_LegalSettings;
using DataService.Models;
using DataService.Models.Authenticate;

namespace DataService.Data;

public interface IDataRepo
{
    bool SaveChanges();
    
    #region Filmovi
    IEnumerable<Film> GetAllFilmovi();
    Film GetFilmById(int id);
    IEnumerable<Film> GetFilmoviForDirektor(int direktorId);
    Film GetFilm(int filmId);
    void CreateFilm(int direktorId, Film film);
    void CreateFilm(Film film);
    void UpdateFilm(Film film);
    void DeleteFilm(Film film);
    bool FilmExists(string key);
    #endregion

    #region Glumac
    IEnumerable<Glumac> GetAllGlumci();
    Glumac GetGlumacById(int id);
    void CreateGlumac(Glumac glumac);
    void UpdateGlumac(Glumac glumac);
    void DeleteGlumac(Glumac glumac);
    bool GlumacExists(string key);
    #endregion

    #region Direktor
    IEnumerable<Direktor> GetAllDirektori();
    Direktor GetDirektorById(int id);
    void CreateDirektor(Direktor direktor);
    void UpdateDirektor(Direktor direktor);
    void DeleteDirektor(Direktor direktor);
    bool DirektorExists(string key);
    #endregion

    #region Zanr
    IEnumerable<Zanr> GetAllZanrovi();
    Zanr GetZanrById(int id);
    void CreateZanr(Zanr zanr);
    void UpdateZanr(Zanr zanr);
    void DeleteZanr(Zanr zanr);
    #endregion

    #region Authentifikacija
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    AuthenticateResponse ValidateToken(string token);
    #endregion

    User CreateUser(User model);


}