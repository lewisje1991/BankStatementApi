using BankStatementApi.DTOs;
using BankStatementApi.Models;
using System.Collections.Generic;

namespace BankStatementApi.Services
{
    public interface ICategoryService
    {
        Category GetCategoryForTransactionName(string transactionName);

        bool SaveCategory(CategoryDto categoryDto);

        List<Category> RetrieveCategoriesForUserId();
    }
}