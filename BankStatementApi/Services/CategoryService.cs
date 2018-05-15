using BankStatementApi.DTOs;
using BankStatementApi.Models;
using BankStatementApi.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BankStatementApi.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryReposiory)
        {
            _categoryRepository = categoryReposiory;
        }

        public Category GetCategoryForTransactionName(string transactionName)
        {
            Dictionary<Category, List<string>> categories = RetrieveCategories();

            foreach (KeyValuePair<Category, List<string>> category in categories)
            {
                if(IsTransactionNameInCategory(category.Value, transactionName))
                {
                    return category.Key;
                }
            }

            return null;
        }

        private bool IsTransactionNameInCategory(List<string> categoryList, string transactionName)
        {
            foreach (var category in categoryList)
            {
                if (transactionName.Contains(category.Trim())) //TODO remove need for trim fix UI
                {
                    return true;
                }
            }

            return false;
        }

        private Dictionary<Category, List<string>> RetrieveCategories()
        {
            return _categoryRepository.GetAll().ToDictionary(x => x, x => x.TransactionNames.Split(',').ToList());
        }

        public bool SaveCategory(CategoryDto categoryDto)
        {
            var model = new Category()
            {
                Target = categoryDto.Target,
                Name = categoryDto.Name.ToLower(),
                TransactionNames = categoryDto.TransactionNames.ToLower().Trim(),
                UserId = 1
            };

            _categoryRepository.Add(model);
            if (_categoryRepository.Save() == 0)
            {
                return false;
            }

            return true;
        }
    }
}