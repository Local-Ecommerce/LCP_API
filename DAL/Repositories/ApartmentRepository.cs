using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        private readonly LoichDBContext _context;

    public ApartmentRepository(LoichDBContext context) : base(context) { }
}
}
