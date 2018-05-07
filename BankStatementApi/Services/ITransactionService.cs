using System.IO;

namespace BankStatementApi.Services
{
    public interface ITransactionService
    {
        void ProcessTransactions(MemoryStream postedFile, string bankName);
        bool ReCategoriseTransactions();
    }
}