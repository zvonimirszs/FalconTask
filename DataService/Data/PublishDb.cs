using System.Text.Json;
using DataService.Models;
using Microsoft.EntityFrameworkCore;

namespace DataService.Data;

public static class PublishDb
{
    public static void PublishPopulation(IApplicationBuilder app, ConfigurationManager config,bool isProd)
    {
        using( var serviceScope = app.ApplicationServices.CreateScope())
        {
            MigrationData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            SeedZanrData(serviceScope.ServiceProvider.GetService<AppDbContext>(), config, isProd);
            SeedDirektorData(serviceScope.ServiceProvider.GetService<AppDbContext>(), config, isProd);
            SeedGlumciData(serviceScope.ServiceProvider.GetService<AppDbContext>(), config, isProd);
            SeedFilmoviData(serviceScope.ServiceProvider.GetService<AppDbContext>(), config, isProd);
        }
    }
    // TO DO: Migracija
    private static void MigrationData(AppDbContext context, bool isProd)
    {
        if(isProd)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }
        // samo ako želimo da se zvi podaci resetiraju
        else
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }
    }
    // preuzimanje podataka o Žanrovi iz Json dokumenta
    private static void SeedZanrData(AppDbContext context, ConfigurationManager config, bool isProd)
    {
        if(!context.Zanrovi.Any())
        {
            Console.WriteLine($"--> Reading from file {config["ZanrData"]} Data ZanrData...");
            List<Models.Zanr> zanrovi = new List<Models.Zanr>();
            using (StreamReader r = new StreamReader(config["ZanrData"]))
            {
                string json = r.ReadToEnd();
                zanrovi = JsonSerializer.Deserialize<List<Models.Zanr>>(json);
            }
            Console.WriteLine("--> Seeding Data {...");
            foreach (var zanr in zanrovi)
            {
                context.Zanrovi.AddRange(zanr); 
            }
            context.SaveChanges();            
        }
        else
        {
            Console.WriteLine("--> We already have ZanrData data");
        }
    }

    // preuzimanje podataka o Direktori iz Json dokumenta
    private static void SeedDirektorData(AppDbContext context, ConfigurationManager config, bool isProd)
    {
        if (!context.Direktori.Any())
        {
            Console.WriteLine($"--> Reading from file {config["DirektorData"]} Data DirektorData...");
            List<Models.Direktor> direktori = new List<Models.Direktor>();
            using (StreamReader r = new StreamReader(config["DirektorData"]))
            {
                string json = r.ReadToEnd();
                direktori = JsonSerializer.Deserialize<List<Models.Direktor>>(json);
            }
            Console.WriteLine("--> Seeding Data DirektorData...");
            foreach (var direktor in direktori)
            {
                context.Direktori.AddRange(direktor);
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have DirektorData data");
        }
    }

    // preuzimanje podataka o Glumci iz Json dokumenta
    private static void SeedGlumciData(AppDbContext context, ConfigurationManager config, bool isProd)
    {
        if (!context.Glumci.Any())
        {
            Console.WriteLine($"--> Reading from file {config["GlumacData"]} Data GlumacData...");
            List<Models.Glumac> glumci = new List<Models.Glumac>();
            using (StreamReader r = new StreamReader(config["GlumacData"]))
            {
                string json = r.ReadToEnd();
                glumci = JsonSerializer.Deserialize<List<Models.Glumac>>(json);
            }
            Console.WriteLine("--> Seeding Data DirektorData...");
            foreach (var glumac in glumci)
            {
                context.Glumci.AddRange(glumac);
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have GlumacData data");
        }
    }

    // preuzimanje podataka o Glumci iz Json dokumenta
    private static void SeedFilmoviData(AppDbContext context, ConfigurationManager config, bool isProd)
    {
        if (!context.Filmovi.Any())
        {
            Console.WriteLine($"--> Reading from file {config["FilmData"]} Data FilmData...");
            List<Dtos.FilmCreateDto> filmoviCreate = new List<Dtos.FilmCreateDto>();
            using (StreamReader r = new StreamReader(config["FilmData"]))
            {
                string json = r.ReadToEnd();
                filmoviCreate = JsonSerializer.Deserialize<List<Dtos.FilmCreateDto>>(json);
            }
            Console.WriteLine("--> Seeding Data FilmData...");
            foreach (var filmCreate in filmoviCreate)
            {
                Film film = new Film();
                film.Id = filmCreate.Id;
                film.Naziv = filmCreate.Naziv;
                film.Opis = filmCreate.Opis;
                film.Budzet = filmCreate.Budzet;
                film.PocetakSnimanja = filmCreate.PocetakSnimanja;
                film.KrajSnimanja = filmCreate.KrajSnimanja;
                foreach (var zanrId in filmCreate.Zanrovi)
                {
                    film.Zanrovi.Add(context.Zanrovi.Where(c => c.Id == zanrId).FirstOrDefault());
                }
                foreach (var glumacId in filmCreate.Glumci)
                {
                    film.Glumci.Add(context.Glumci.Where(c => c.Id == glumacId).FirstOrDefault());
                }
                film.Direktor = context.Direktori.Where(c => c.Id == filmCreate.DirektorId).FirstOrDefault(); 
                context.Filmovi.Add(film);
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have FilmData data");
        }
    }
}
public static class JsonFileReader
{
    public static T Read<T>(string filePath)
    {
        string text = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(text);
    }
}