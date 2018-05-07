using System.Collections.Generic;

namespace BankStatementApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TransactionNames { get; set; }
        public decimal Target { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}