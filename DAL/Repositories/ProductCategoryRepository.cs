using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly LoichDBContext _context;

        public ProductCategoryRepository(LoichDBContext context) : base(context) { }
    }
}