namespace TEbucksServer.Models
{
    public class Account
    {
        public int accountId { get; set; }
        public int user_Id { get; set; }
        public decimal balance { get; set; }
    }
    public class ReturnAccount

    {
        public int user_id { get; set; }
        public decimal Balance { get; set; }
    }

}
