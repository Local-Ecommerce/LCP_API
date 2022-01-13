using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly LoichDBContext _context;

        public ProductRepository(LoichDBContext context) : base(context) { }
    }
}