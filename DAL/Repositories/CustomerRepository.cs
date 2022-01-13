using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly LoichDBContext _context;

        public CustomerRepository(LoichDBContext context) : base(context) { }
    }
}