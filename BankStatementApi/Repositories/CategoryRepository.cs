using BankStatementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankStatementApi.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BankStatementApiContext context) : base(context)
        {
        }
    }
}