using Identity.Models.Authenticate;

namespace Identity.Data;
public class IdentityRepo : IIdentityRepo
{
    private readonly AppDbContext _context;
    public IdentityRepo(AppDbContext context)
    {
        _context = context;
    }

    public void CreateUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (user.Password == null || user.Password == "")
        {
            user.Password = user.Username;
        }
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Users.Add(user);
    }
    public void UpdateUser(User user)
    {
        if(user.Password == null || user.Password == "")
        {
            user.Password = user.Username;
        }
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Users.Update(user);
    }
    public User GetUserByUserName(string userName)
    {            
        return(_context.Users.SingleOrDefault(x => x.Username == userName));
    }
    public bool SaveChanges()
    {            
        return(_context.SaveChanges() >= 0);
    }
}