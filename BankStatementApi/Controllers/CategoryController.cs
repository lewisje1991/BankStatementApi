using BankStatementApi.DTOs;
using BankStatementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankStatementApi.Controllers
{
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
        public bool Post(CategoryDto categoryDto)
        {
            if (!_categoryService.SaveCategory(categoryDto))
            {
                return false;
            }

            return _transactionService.ReCategoriseTransactions();
        }
    }
}