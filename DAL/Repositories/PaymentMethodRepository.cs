using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        private readonly LoichDBContext _context;

        public PaymentMethodRepository(LoichDBContext context) : base(context) { }
    }
}