using BankStatementApi.DTOs;
using BankStatementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace BankStatementApi.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;
        private ITransactionService _transactionService;

        public CategoryController(ICategoryService categoryService, ITransactionService transactionService)
        {
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

        // POST api/category
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]CategoryDto categoryDto)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryService.SaveCategory(categoryDto))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An Error Occured During Save");
            }

            if (_transactionService.ReCategoriseTransactions())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An Error Occured During Re-Categorisation.");
            }

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}