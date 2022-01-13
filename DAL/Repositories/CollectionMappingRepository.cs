using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class CollectionMappingRepository : Repository<CollectionMapping>, ICollectionMappingRepository
    {

        private readonly LoichDBContext _context;

        public CollectionMappingRepository(LoichDBContext context) : base(context) { }
    }
}