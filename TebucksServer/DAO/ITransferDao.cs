﻿using System.Collections.Generic;
using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransferByID(int id);

        Transfer CreateNewTransfer(NewTransfer transfer, TransferStatus transStatus);

 //       TransferStatus UpdateTransferStatus(string pending, string approved, string rejected);

        List<Transfer> GetAccountTransfer(string username);


    }
}
