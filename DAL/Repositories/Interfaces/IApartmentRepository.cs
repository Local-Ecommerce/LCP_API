using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IApartmentRepository : IRepository<Apartment>
    {
        Task<List<Apartment>> GetAllActiveApartment();

        Task<List<Apartment>> GetAllApartment();
    }
}