using System;
using System.Data.SqlClient;
using TEbucksServer.Models;
using TEbucksServer.DAO;

namespace TEbucksServer.DAO
{
    public class TransferSqlDao 
    {
        private readonly string connectionString;
        public TransferSqlDao(string dbconnectionstring)
        {
            connectionString = dbconnectionstring;
        }
        public Transfer GetTransferById(int id)
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
        public Transfer CreateNewTransfer(NewTransfer transfer, TransferStatus transStatus)
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
                    cmd.Parameters.AddWithValue("@transfer_status", transStatus.TransferStatusUpdate);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    newTransID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                newTrans = GetTransferById(newTransID);
            }
            catch (SqlException)
            {
                throw new Exception();
            }
            return newTrans;
            }

            public Transfer MapRowToTransfer(SqlDataReader reader)
            {
                Transfer transfer = new Transfer();
                transfer.TransferID = Convert.ToInt32(reader["transfer_id"]);
                transfer.TransferType = Convert.ToInt32(reader["transfer_type"]);
                transfer.TransferStatus = Convert.ToInt32(reader["transfer_status"]);
                return transfer;
            }

    }
}
