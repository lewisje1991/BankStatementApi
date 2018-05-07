using BankStatementApi.DTOs;
using CsvHelper.Configuration;

namespace BankStatementApi.Mappers
{
    public class BankOfScotlandCsvMapper : ClassMap<TransactionDto>
    {
        public BankOfScotlandCsvMapper()
        {
            Map(m => m.Type).Name("Transaction Type");
            Map(m => m.Date).Name("Transaction Date");
            Map(m => m.Description).Name("Transaction Description");
            Map(m => m.Debit).Name("Debit Amount").Default(0);
            Map(m => m.Credit).Name("Credit Amount").Default(0);
        }
    }
}