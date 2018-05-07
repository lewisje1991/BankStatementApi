using BankStatementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BankStatementApi.Controllers
{
    public class TransactionController : Controller
    {
        public ITransactionService _transactionService { get; set; }

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // POST api/transaction
        [Authorize]
        public void Post(IFormFile file)
        {
            if (file != null)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    _transactionService.ProcessTransactions(stream, "BankOfScotland");
                }
            }

        }
    }
}
