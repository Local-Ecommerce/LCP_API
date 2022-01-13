using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly LoichDBContext _context;

        public PaymentRepository(LoichDBContext context) : base(context) { }
    }
}