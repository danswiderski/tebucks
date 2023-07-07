using System.Collections.Generic;
using TEBucksServer.Models;

namespace TEBucksServer.DAO
{
    public interface IUserDao
    {
        User GetUser(string username);

        User GetUserById(int id);
        User AddUser(RegisterUser registerUser);
        List<User> GetUsers(string usermame = "");
    }
}
