using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
        private readonly LoichDBContext _context;

        public CollectionRepository(LoichDBContext context) : base(context) { }
    }
}