using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TEbucksServer.Models;
using TEBucksServer.Security;
using TEBucksServer.Security.Models;

namespace TEbucksServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;
        public AccountSqlDAO(string dbconnectionstring)
        {
            connectionString = dbconnectionstring;
        }
        public Account GetAccount(int userID)
        {
            Account fetchedAccount = new Account();
            string sql = "SELECT username, accountId, account.user_Id, balance  FROM account join tebucks_user as u on u.user_id = account.user_id WHERE u.user_id = @userId;";
            try
            {
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@userId", userID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        fetchedAccount = MapRowToAccount(reader);
                        return fetchedAccount;
                    }
                    
                       
                    
                }
            }
            catch (Exception)
            {
                throw;

            }

            return fetchedAccount;
        }
        public Account CreateAccount(int user_id)
        {
            Account newAccount = new Account();
            string sql = "insert into account (user_id, balance) output inserted.accountId values (@user_id, 1000);";
            try
            {
                int newAccID;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    newAccID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                newAccount = GetAccount(user_id);
            }
            catch (Exception)
            {

                throw;
            }
            return newAccount;
        }

        public Account GetAccountByUserName(string username)
        {
            Account fetchedAccount = new Account();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT username, accountId, account.user_Id, balance  FROM account join tebucks_user as u on u.user_id = account.user_id WHERE username = @accountname", conn);
                    cmd.Parameters.AddWithValue("@accountname", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        fetchedAccount = MapRowToAccount(reader);
                        return fetchedAccount;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return fetchedAccount;
        }
        
        
        private Account MapRowToAccount(SqlDataReader reader)
        {
            Account account = new Account();
            account.accountId = Convert.ToInt32(reader["accountId"]);
            account.user_Id = Convert.ToInt32(reader["user_id"]);
            account.balance = Convert.ToDecimal(reader["balance"]);

            return account;
        }


    }

}


