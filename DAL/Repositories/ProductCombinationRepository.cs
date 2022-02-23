using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductCombinationRepository : Repository<ProductCombination>, IProductCombinationRepository
    {
        public ProductCombinationRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Product Combination
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public async Task<PagingModel<ProductCombination>> GetProductCombination(
            string id, string productId,
            int?[] status, int? limit, int? queryPage,
            bool isAsc, string propertyName)
        {
            IQueryable<ProductCombination> query = _context.ProductCombinations.Where(p => p.ProductCombinationId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(p => p.ProductCombinationId.Equals(id));

            //filter by product id
            if (!string.IsNullOrEmpty(productId))
                query = query.Where(p => p.ProductId.Equals(productId) || p.BaseProductId.Equals(productId));

            //filter by status
            if (status.Length != 0)
                query = query.Where(p => status.Contains(p.Status));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(10);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<ProductCombination>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}