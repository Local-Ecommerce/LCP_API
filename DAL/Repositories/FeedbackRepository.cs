using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(LoichDBContext context) : base(context) { }
    }
}