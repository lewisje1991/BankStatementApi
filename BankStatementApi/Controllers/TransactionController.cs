using BankStatementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BankStatementApi.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        public ITransactionService _transactionService { get; set; }

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // POST api/transaction
        [Authorize]
        [HttpPost]
        public IActionResult Post(IFormFile file)
        {
            if (file != null)
            {
                return BadRequest("Csv file was missing or invalid.");
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                if(!_transactionService.ProcessTransactions(stream, "BankOfScotland"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error during processing of transactions");
                }
            }

            return Ok("Transactions Uploaded");
        }
    }
}
