using System;

namespace TEbucksServer.Models
{
    public class LogReturnDTO
    {
        public string description { get; set; }
        public string username_from { get; set; }
        public string username_to { get; set; }
        public decimal amount { get; set; }

        public int Log_id { get; set; }

        public DateTime createdDate { get; set; }

    }
}

