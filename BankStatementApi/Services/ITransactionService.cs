using System.IO;

namespace BankStatementApi.Services
{
    public interface ITransactionService
    {
        bool ProcessTransactions(MemoryStream postedFile, string bankName);
        bool ReCategoriseTransactions();
    }
}