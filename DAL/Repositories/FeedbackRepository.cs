using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using DAL.Constants;

namespace DAL.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Feedback
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="residentId"></param>
        /// <param name="residenSendRequest"></param>
        /// <param name="apartmentId"></param>
        /// <param name="role"></param>
        /// <param name="rating"></param>
        /// <param name="date"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Feedback>> GetFeedback(
            string id, string productId, string residentId, string residenSendRequest, string role,
            string apartmentId, double? rating, DateTime? date, int? limit,
            int? queryPage, bool? isAsc, string propertyName, string[] include)
        {
            IQueryable<Feedback> query = _context.Feedbacks.Where(fb => fb.FeedbackId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(fb => fb.FeedbackId.Equals(id));

            //filter by productId
            if (!string.IsNullOrEmpty(productId))
                query = query.Where(fb => fb.ProductId.Equals(productId));

            //filter by date
            if (date.HasValue)
                query = query.Where(fb => fb.FeedbackDate.Equals(date.Value.Date));

            //filter by residentId
            if (!string.IsNullOrEmpty(residentId))
                query = query.Where(fb => fb.ResidentId.Equals(residentId));

            //filter by role
            switch (role)
            {
                case ResidentType.MERCHANT:
                    query = query.Include(fb => fb.Product)
                                    .Where(fb => fb.Product.ResidentId.Equals(residenSendRequest));
                    break;
                case ResidentType.MARKET_MANAGER:
                    query = query.Include(fb => fb.Product)
                                    .ThenInclude(p => p.Resident)
                                    .ThenInclude(r => r.MerchantStores)
                                    .Where(fb => fb.Product.Resident.ApartmentId.Equals(apartmentId))
                                .Include(fb => fb.Resident);
                    break;
            }

            //add include
            if (include != null && include.Length > 0)
            {
                foreach (var item in include)
                {
                    if (item.Equals(nameof(Feedback.Product)))
                        query = query.Include(fb => fb.Product);
                    if (item.Equals(nameof(Feedback.Resident)))
                        query = query.Include(fb => fb.Resident);
                }
            }

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = (bool)isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Feedback>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}