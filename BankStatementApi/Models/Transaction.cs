namespace BankStatementApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string UserId { get; set; }

        public virtual Category Category { get; set; }
    }
}
