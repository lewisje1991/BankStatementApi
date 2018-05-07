using BankStatementApi.DTOs;
using BankStatementApi.Models;

namespace BankStatementApi.Services
{
    public interface ICategoryService
    {
        Category GetCategoryForTransactionName(string transactionName);

        bool SaveCategory(CategoryDto categoryDto);
    }
}