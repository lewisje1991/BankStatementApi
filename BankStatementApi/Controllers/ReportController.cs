
using BankStatementApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authorization;

namespace BankStatementApi.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private ITransactionReportService _reportService;

        public ReportController(ITransactionReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult TransactionReport(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return BadRequest("End cannot be before start");
            }

            var content = _reportService.GenerateTransactionReport(start, end, 1);
            if (content == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed Generating Transaction Report");
            }

            return Ok(content);
        }

    }
}
