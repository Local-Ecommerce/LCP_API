using DAL.Models;
using DAL.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DAL.Constants;
using System;

namespace DAL.Repositories {
	public class ApartmentRepository : Repository<Apartment>, IApartmentRepository {
		public ApartmentRepository(LoichDBContext context) : base(context) { }

		/// <summary>
		/// Get Apartment
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<PagingModel<Apartment>> GetApartment(ApartmentPagingRequest request) {
			IQueryable<Apartment> query = _context.Apartments.Where(ap => ap.ApartmentId != null);

			//filter by id
			if (!string.IsNullOrEmpty(request.Id))
				query = query.Where(ap => ap.ApartmentId.Equals(request.Id));

			//filter by status
			if (request.Status != null && request.Status.Length != 0)
				query = query.Where(ap => request.Status.Contains(ap.Status));

			//filter by search
			if (!string.IsNullOrEmpty(request.Search))
				query = query.Where(ap => ap.ApartmentName.Contains(request.Search));

			//add include
			if (!string.IsNullOrEmpty(request.Include.FirstOrDefault()))
				if (request.Include.FirstOrDefault().Equals("marketmanager"))
					query = query.Include(ap => ap.Residents.Where(res => res.Type.Equals(ResidentType.MARKET_MANAGER)));

			//sort
			if (!string.IsNullOrEmpty(request.PropertyName)) {
				query = request.IsAsc ? query.OrderBy(c => request.PropertyName) : query.OrderByDescending(c => request.PropertyName);
			}

			//paging
			int perPage = request.Limit.GetValueOrDefault(Int32.MaxValue);
			int page = request.QueryPage.GetValueOrDefault(1) == 0 ? 1 : request.QueryPage.GetValueOrDefault(1);
			int total = query.Count();

			return new PagingModel<Apartment> {
				List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
				Total = total,
				Page = page,
				LastPage = (int)Math.Ceiling(total / (double)perPage)
			};
		}
	}
}
