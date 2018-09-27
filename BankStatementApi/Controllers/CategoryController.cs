using BankStatementApi.DTOs;
using BankStatementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        // GET api/category
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var categories = _categoryService.RetrieveCategoriesForUserId();

            if(categories == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An Error Occured During Retrieval of Categories");
            }

            return Ok(categories);
        }

        // POST api/category
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]CategoryDto categoryDto)
        {
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