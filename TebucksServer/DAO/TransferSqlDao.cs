using System;
using System.Data.SqlClient;
using TEbucksServer.Models;
using TEbucksServer.DAO;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using TEBucksServer.DAO;

namespace TEbucksServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;
        public TransferSqlDao(string dbconnectionstring)
        {
            connectionString = dbconnectionstring;
        }
        public Transfer GetTransferByID(int id)
        {
            Transfer output = null;
            string sql = "select * from transfer join transfer_type on transfer.transfer_type = type_id where transfer_id = @id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        output = MapRowToTransfer(reader);
                        return output;
                    }
                }
            }
            catch { }
            return output;
        }
        public Transfer CreateNewTransfer(NewTransfer transfer)
        {


            Transfer newTrans = new Transfer();
            string toAccountSQL = "select accountId from account where user_id = @userTo";
            string fromAccountSQL = "select accountId from account where user_id = @userFrom";
            string sql = "INSERT INTO transfer (to_user_id, from_user_id , transfer_type, transfer_status, amount )" + "OUTPUT INSERTED.transfer_id VALUES (@to_user_id, @from_user_id , (select type_id from transfer_type where transfer_name = @transfer_type), (select status_id from status where status_name = @transfer_status), @amount);";
            try
            {
                int newTransID;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(toAccountSQL, conn);
                    cmd.Parameters.AddWithValue("@userTo", transfer.UserTo);
                    int toAccountID = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd = new SqlCommand(fromAccountSQL, conn);
                    cmd.Parameters.AddWithValue("@userFrom", transfer.UserFrom);
                    int fromAccountID = Convert.ToInt32(cmd.ExecuteScalar());


                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@to_user_id", toAccountID);
                    cmd.Parameters.AddWithValue("@from_user_id", fromAccountID);
                    cmd.Parameters.AddWithValue("@transfer_type", transfer.TransferType);
                    cmd.Parameters.AddWithValue("@transfer_status", StatusHelper(transfer));
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    newTransID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                newTrans = GetTransferByID(newTransID);
            }
            catch (SqlException e)
            {
                throw new Exception();
            }
            return newTrans;
        }
        public List<Transfer> GetAccountTransfer(string username)
        {
            List<Transfer> transfers = new List<Transfer>();
            string nameSQL = "select type_id from transfer_type where transfer_name = @transferid";
            //string fromAccountSQL = "select  from account where user_id = @userFrom";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {


                    conn.Open();

                    SqlCommand cmd = new SqlCommand(nameSQL, conn);
                    cmd.Parameters.AddWithValue("@transferid", username);
                    int nameID = Convert.ToInt32(cmd.ExecuteScalar());



                    cmd = new SqlCommand("SELECT transfer_Id, to_user_Id, from_user_Id, amount, transfer_status, transfer_name FROM transfer" +
                        " join transfer_type on transfer.transfer_type = type_id  WHERE to_user_id = " 
                       , conn);

                    cmd.Parameters.AddWithValue("@accountname", username);




                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        transfers.Add(MapRowToTransfer(reader));
                    }
                    return transfers;

                }

            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
            return transfers;
        }
        public Transfer UpdateTransferStatus(int id, string newStatus)
        {
            Transfer updatedTrans = null;

            string sql = "UPDATE transfer SET transfer_status = @transfer_status WHERE transfer_id = @transfer_id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_status", newStatus);
                    cmd.Parameters.AddWithValue("@transfer_id", id);
                    int numberOfRows = cmd.ExecuteNonQuery();
                    if (numberOfRows == 0)
                    {
                        throw new Exception("Zero rows affected, expected at least one");
                    }
                }
                updatedTrans = GetTransferByID(id);
            }
            catch (SqlException ex)
            {
                throw new Exception($"SQL exception occurred", ex);
            }

            return updatedTrans;
        }

        public string StatusHelper(NewTransfer newTransfer)
        {
            string transStatus = "";

            if (newTransfer.TransferType == "Send")
            {
                transStatus = "Approved";
            }
            if (newTransfer.TransferType == "Request")
            {
                transStatus = "Pending";
            }
            return transStatus;
        }
        //TODO this needs to be a helper for all balance transfers
        //public Transfer UpdateBalance()
        //{

        //}





        public Transfer MapRowToTransfer(SqlDataReader reader)
        {
            UserSqlDao userDao = new UserSqlDao(connectionString);
            AccountSqlDAO accountDao = new AccountSqlDAO(connectionString);
            Transfer transfer = new Transfer();
            transfer.transferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.transferType = Convert.ToString(reader["transfer_name"]);
            transfer.transferStatus = Convert.ToInt32(reader["transfer_status"]);
            transfer.userFrom = userDao.GetUserByAccountId(Convert.ToInt32(reader["from_user_id"]));
            transfer.userTo = userDao.GetUserByAccountId(Convert.ToInt32(reader["to_user_id"]));
            transfer.amount = Convert.ToDecimal(reader["amount"]);

            return transfer;
        }


    }

}
