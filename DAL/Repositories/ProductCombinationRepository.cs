using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ProductCombinationRepository : Repository<ProductCombination>, IProductCombinationRepository
    {
        public ProductCombinationRepository(LoichDBContext context) : base(context) { }
    }
}