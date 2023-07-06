using System.Collections.Generic;
using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransferByID(int id);

        Transfer CreateNewTransfer(NewTransfer transfer, TransferStatus transStatus);

        Transfer UpdateTransferStatus(int id, string newStatus);

        List<Transfer> GetAccountTransfer(string username);


    }
}
