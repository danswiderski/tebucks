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
            Transfer output = new Transfer();
            string sql = "select transfer_id, to_user_id, from_user_id, status_name, transfer_name, amount from transfer as t " +
                "join status as s on s.status_id = t.transfer_status " +
                "join transfer_type as tt on tt.type_id = t.transfer_type " +
                "where transfer_id = @id";
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
                AccountSqlDAO accountDao = new AccountSqlDAO(connectionString);
                UpdateBalance(newTrans, accountDao.GetAccountByUserName(newTrans.userTo.Username), accountDao.GetAccountByUserName(newTrans.userFrom.Username));

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
            string nameSQL = "SELECT transfer_id, from_user_id, to_user_id, transfer_name, status_name, t.amount from transfer as t " +
                "JOIN account as a on a.accountId = t.from_user_id JOIN tebucks_user as tu on tu.user_id = a.user_id " +
                "JOIN account as a2 on a2.accountId = t.to_user_id JOIN tebucks_user as tu2 on tu2.user_id = a2.user_id " +
                "JOIN status as s on s.status_id = t.transfer_status JOIN transfer_type as tt on t.transfer_type = tt.type_id " +
                "where tu.username = @username or tu2.username = @username";
            //string fromAccountSQL = "select  from account where user_id = @userFrom";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {


                    conn.Open();

                    SqlCommand cmd = new SqlCommand(nameSQL, conn);
                    cmd.Parameters.AddWithValue("@username", username);

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
        public Transfer UpdateTransferStatus(int id, TransferStatusUpdateDto newStatus)
        {
            Transfer updatedTrans = null;

            string sql = "UPDATE transfer SET transfer_status = " +
                "(SELECT TOP 1 status_id FROM status WHERE status_name = @transfer_status) " +
                "WHERE transfer_id = @transfer_id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_status", newStatus.transferStatus);
                    cmd.Parameters.AddWithValue("@transfer_id", id);
                    int numberOfRows = cmd.ExecuteNonQuery();
                    if (numberOfRows == 0)
                    {
                        throw new Exception("Zero rows affected, expected at least one");
                    }
                }
                updatedTrans = GetTransferByID(id);
                AccountSqlDAO accountDao = new AccountSqlDAO(connectionString);
                UpdateBalance(updatedTrans, accountDao.GetAccountByUserName(updatedTrans.userTo.Username), accountDao.GetAccountByUserName(updatedTrans.userFrom.Username));

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
        public bool UpdateBalance(Transfer transfer, Account recipient, Account sender)
        {
            //transfer.userTo.userId
            //transfer.userFrom.userId
            //transfer.amount 
            //transfer.transferStatus
            //recipient.balance
            //recipient.user_Id
            //sender.balance
            //sender.user_Id


            if (transfer.userFrom.UserId != recipient.user_Id)
            {
                if (transfer.amount > 0)
                {
                    if (transfer.amount <= sender.balance)
                    {
                        if (transfer.transferStatus == "Approved")
                        {
                            recipient.balance += transfer.amount;
                            sender.balance -= transfer.amount;
                            string sql = "Update account Set balance = @balance where accountId = @accountid;";
                            try
                            {
                                using (SqlConnection conn = new SqlConnection(connectionString))
                                {
                                    conn.Open();

                                    SqlCommand recipientcmd = new SqlCommand(sql, conn);
                                    recipientcmd.Parameters.AddWithValue("@balance", recipient.balance);
                                    recipientcmd.Parameters.AddWithValue("@accountid", recipient.accountId);
                                    int numberOfRows = recipientcmd.ExecuteNonQuery();
                                    if (numberOfRows != 1)
                                    {
                                        throw new Exception("Zero rows affected, expected at least one");
                                    }
                                    else
                                    {

                                        SqlCommand sendercmd = new SqlCommand(sql, conn);
                                        sendercmd.Parameters.AddWithValue("@balance", sender.balance);
                                        sendercmd.Parameters.AddWithValue("@accountid", sender.accountId);
                                        numberOfRows = sendercmd.ExecuteNonQuery();
                                        if (numberOfRows != 1)
                                        {
                                            throw new Exception("Zero rows affected, expected at least one");
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            catch (SqlException ex)
                            {
                                throw new Exception($"SQL exception occurred", ex);
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }


        }



        public Transfer MapRowToTransfer(SqlDataReader reader)
        {
            UserSqlDao userDao = new UserSqlDao(connectionString);
            AccountSqlDAO accountDao = new AccountSqlDAO(connectionString);
            Transfer transfer = new Transfer();
            transfer.transferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.transferType = Convert.ToString(reader["transfer_name"]);
            transfer.transferStatus = Convert.ToString(reader["status_name"]);
            transfer.userFrom = userDao.GetUserByAccountId(Convert.ToInt32(reader["from_user_id"]));
            transfer.userTo = userDao.GetUserByAccountId(Convert.ToInt32(reader["to_user_id"]));
            transfer.amount = Convert.ToDecimal(reader["amount"]);

            return transfer;
        }


    }

}
