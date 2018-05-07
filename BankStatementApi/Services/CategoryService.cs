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
            //TODO Find better way to categorise

            var categories = _categoryRepository.GetAll().ToDictionary(x => x, x => x.TransactionNames.Split(',').ToList());

            foreach(KeyValuePair<Category, List<string>> category in categories)
            {

               foreach(var categoryString in category.Value)
                {
                    if (transactionName.Contains(categoryString.Trim())) //TODO remove need for trim fix UI
                    {
                        return category.Key;
                    }
                }

            }

            return null;
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