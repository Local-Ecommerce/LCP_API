using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(LoichDBContext context) : base(context) { }
    }
}