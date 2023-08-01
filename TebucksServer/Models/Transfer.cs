using System.Reflection.Metadata;
using TEBucksServer.Models;

namespace TEbucksServer.Models
{
    public class Transfer
    {
        public int transferId { get; set; }

        public string transferType { get; set; }

        public string transferStatus { get; set; }

        public User userFrom { get; set; }

        public User userTo { get; set; }

        public decimal amount { get; set; }

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
