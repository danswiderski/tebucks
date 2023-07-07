using System;
using System.Data.SqlClient;
using TEbucksServer.Models;
using TEbucksServer.DAO;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;

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
            string sql = "select * from transfer where transder_id = @id";
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


            Transfer newTrans = null;

            string sql = "INSERT INTO transfer (to_user_id, from_user_id , transfer_type, transfer_status, amount )" + "OUTPUT INSERTED.transfer_id VALUES (@to_user_id, @from_user_id , @transfer_type, @transfer_status, @amount);";
            try
            {
                int newTransID;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@to_user_id", transfer.UserTo);
                    cmd.Parameters.AddWithValue("from_user_id", transfer.UserFrom);
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
                        transfers.Add(MapRowToTransfer(reader));
                        return transfers;
                    }

                }

            }
            catch
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






        public Transfer MapRowToTransfer(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            {
                transfer.TransferID = Convert.ToInt32(reader["transfer_id"]);
                transfer.TransferType = Convert.ToString(reader["transfer_type"]);
                transfer.TransferStatus = Convert.ToInt32(reader["transfer_status"]);
            }
            return transfer;
        }


    }
    
}
