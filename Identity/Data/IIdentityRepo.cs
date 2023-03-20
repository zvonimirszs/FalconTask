using Identity.Models.Authenticate;

namespace Identity.Data;
public interface IIdentityRepo
{
    bool SaveChanges();
    #region User
    User GetUserByUserName(string userName);
    void CreateUser(User user);
    void UpdateUser(User user);

    #endregion

}