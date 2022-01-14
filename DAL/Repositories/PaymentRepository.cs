using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(LoichDBContext context) : base(context) { }
    }
}