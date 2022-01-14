using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(LoichDBContext context) : base(context) { }
    }
}