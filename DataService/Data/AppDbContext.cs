using Microsoft.EntityFrameworkCore;
using DataService.Models;

namespace DataService.Data;
public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }
    public DbSet<Film> Filmovi { get; set; }
    public DbSet<Zanr> Zanrovi { get; set; }        
    public DbSet<Direktor> Direktori { get; set; }
    public DbSet<Glumac> Glumci { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Film>().HasMany(i => i.Zanrovi).WithMany();
        modelBuilder.Entity<Film>().HasMany(i => i.Glumci).WithMany();
        //modelBuilder
        //    .Entity<Zanr>()
        //    .HasKey(p => p.Id)
        //    .
        //    .WithOne(p=> p.Film!);

        //modelBuilder
        //    .Entity<Film>()
        //    .HasOne(p => p.Direktor)
        //    .WithMany(p => p.Subscriptions)
        //    .HasForeignKey(p => p.DirektorId);
    }
}

