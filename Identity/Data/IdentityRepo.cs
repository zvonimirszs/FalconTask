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
        throw new NotImplementedException();
    }
    public void UpdateUser(User user)
    {
        throw new NotImplementedException();
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