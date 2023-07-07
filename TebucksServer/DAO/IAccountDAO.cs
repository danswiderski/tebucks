using System.Collections.Generic;
using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public interface IAccountDAO
    {
        Account GetAccount(int accountId);

        Account GetAccountByUserName(string username);
        Account CreateAccount(int user_id);
    }
}
