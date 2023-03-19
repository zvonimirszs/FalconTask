using Identity.Models.Authenticate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> opt) : base(opt)
    {

    } 
    public DbSet<User> Users { get; set; }
}