using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransferByID(int id);

        NewTransfer CreateNewTransfer(NewTransfer transfer);

        TransferStatus UpdateTransferStatus(string pending, string approved, string rejected);




    }
}
