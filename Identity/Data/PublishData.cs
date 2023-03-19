using System.Text.Json;
using Identity.Models.Authenticate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public static class PublishDb
{
    public static void PublishPopulation(IApplicationBuilder app, ConfigurationManager config,bool isProd)
    {
        using( var serviceScope = app.ApplicationServices.CreateScope())
        {
            MigrateDataBase(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            SeedUserData(serviceScope.ServiceProvider.GetService<AppDbContext>(), config, isProd);
        }
    }
    // preuzimanje podataka o Korisnicima iz Json dokumenta
    private static void SeedUserData(AppDbContext context, ConfigurationManager config, bool isProd)
    {
        if(!context.Users.Any())
        {
            Console.WriteLine($"--> Reading from file {config["UsersData"]} Data UsersData...");
            List<Models.Authenticate.User> userItems = new List<Models.Authenticate.User>();
            using (StreamReader r = new StreamReader(config["UsersData"]))
            {
                string json = r.ReadToEnd();
                userItems = JsonSerializer.Deserialize<List<Models.Authenticate.User>>(json);
                
            }
            Console.WriteLine("--> Seeding Data UsersData...");
            foreach (var user in userItems)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                context.Users.AddRange(user); 
            }
            context.SaveChanges();            
        }
        else
        {
            Console.WriteLine("--> We already have UsersData data");
        }
    }

    private static void MigrateDataBase(AppDbContext context, bool isProd)
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
        //else
        //{
        //    Console.WriteLine("--> Attempting to apply migrations...");
        //    try
        //    {
        //        context.Database.Migrate();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"--> Could not run migrations: {ex.Message}");
        //    }
        //}
    }
   
}