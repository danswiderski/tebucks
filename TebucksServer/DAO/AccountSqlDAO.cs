﻿using System;
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
        public Account GetAccount(int accountId)
        {
            Account fetchedAccount = new Account();

            try
            {
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT username, accountId, account.user_Id, balance  FROM account join tebucks_user as u on u.user_id = account.user_id WHERE accountId = @accountId", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
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
        public List<Transfer> GetAccountTransfer(string username)
        {
            List<Transfer> transfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT tranferId, to_user_Id, account.user_Id, balance  " +
                        "FROM transfer WHERE to_user_id(select top 1 user_id from user where username = @accountname) or " +
                        "from_user_id(select top 1 user_id from user where username = @accountname)", conn);
                    cmd.Parameters.AddWithValue("@accountname", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        transfers = MapRowToTransfer(reader);
                        return transfers;
                    }

                }
                //TODO make sure Dan has Transfer
            }
            catch
            {            
                throw new NotImplementedException();
            }
            return transfers;
        }
        //TODO Dan please create a MapRowToTransfer :)
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


