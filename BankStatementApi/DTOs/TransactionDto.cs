using System;

namespace BankStatementApi.DTOs
{
    public class TransactionDto
    {
        private string description;

        public string Description {
            get
            {
                return description;
            }
            set
            {
                description = value.ToLower();
            }
        }

        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
}