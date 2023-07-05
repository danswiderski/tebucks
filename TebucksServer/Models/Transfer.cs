using TEBucksServer.Models;

namespace TEbucksServer.Models
{
    public class Transfer
    {
        public int TransferID { get; set; }

        public int TransferType { get; set; }

        public int TransferStatus { get; set; }

        public User UserFrom { get; set; }

        public User UserTo { get; set; }

        public decimal Amount { get; set; }

    }

    public class NewTransfer
    {
        public int UserFrom { get; set; }
        public int UserTo { get; set; }
        public decimal Amount { get; set; }
        public string TransferType { get; set; }

    }
    public class TransferStatus
    {
        public string TransferStatusUpdate { get; set; }
    }
}
