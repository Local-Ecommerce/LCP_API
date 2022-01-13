using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ProductInMenuRepository : Repository<ProductInMenu>, IProductInMenuRepository
    {
        private readonly LoichDBContext _context;

        public ProductInMenuRepository(LoichDBContext context) : base(context) { }
    }
}