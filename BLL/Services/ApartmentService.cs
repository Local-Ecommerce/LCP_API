using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Apartment;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BLL.Services {
	public class ApartmentService : IApartmentService {
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;
		private readonly IUtilService _utilService;
		private const string PREFIX = "APM_";

		public ApartmentService(IUnitOfWork unitOfWork,
				ILogger logger,
				IMapper mapper,
				IUtilService utilService) {
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
			_utilService = utilService;
		}


		/// <summary>
		/// Create Apartment
		/// </summary>
		/// <param name="apartmentRequest"></param>
		/// <returns></returns>
		public async Task<ApartmentResponse> CreateApartment(ApartmentRequest apartmentRequest) {
			Apartment apartment = _mapper.Map<Apartment>(apartmentRequest);
			try {
				apartment.ApartmentId = _utilService.CreateId(PREFIX);
				apartment.Status = (int)ApartmentStatus.ACTIVE_APARTMENT;

				_unitOfWork.Apartments.Add(apartment);

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e) {
				_logger.Error("[ApartmentService.CreateApartment()]: " + e.Message);
				throw;
			}

			return _mapper.Map<ApartmentResponse>(apartment);
		}


		/// <summary>
		/// Delete Apartment
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task DeleteApartment(string id) {
			//Check id
			Apartment apartment;
			try {
				apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));
			}
			catch (Exception e) {
				_logger.Error("[ApartmentService.DeleteApartment()]: " + e.Message);

				throw new EntityNotFoundException(typeof(Apartment), id);
			}

			//Delete Apartment
			try {
				apartment.Status = (int)ApartmentStatus.INACTIVE_APARTMENT;

				_unitOfWork.Apartments.Update(apartment);

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e) {
				_logger.Error("[ApartmentService.DeleteApartment()]: " + e.Message);
				throw;
			}
		}


		/// <summary>
		/// Update Apartment
		/// </summary>
		/// <param name="id"></param>
		/// <param name="apartmentRequest"></param>
		/// <returns></returns>
		public async Task<ApartmentResponse> UpdateApartmentById(string id, ApartmentRequest apartmentRequest) {
			//biz ruie

			//Check id
			Apartment apartment;
			try {
				apartment = await _unitOfWork.Apartments.FindAsync(ap => ap.ApartmentId.Equals(id));
			}
			catch (Exception e) {
				_logger.Error("[ApartmentService.UpdateApartment()]: " + e.Message);

				throw new EntityNotFoundException(typeof(Apartment), id);
			}

			//Update Apartment To DB
			try {
				apartment = _mapper.Map(apartmentRequest, apartment);

				_unitOfWork.Apartments.Update(apartment);

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e) {
				_logger.Error("[ApartmentService.UpdateApartment()]: " + e.Message);
				throw;
			}

			return _mapper.Map<ApartmentResponse>(apartment);
		}


		/// <summary>
		/// Get Apartments
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<object> GetApartments(GetApartmentRequest request) {
			PagingModel<Apartment> apartments;
			string propertyName = default;
			bool isAsc = false;

			if (!string.IsNullOrEmpty(request.Sort)) {
				isAsc = request.Sort[0].ToString().Equals("+");
				propertyName = _utilService.UpperCaseFirstLetter(request.Sort[1..]);
			}

			try {
				apartments = await _unitOfWork.Apartments.GetApartment(_mapper.Map<ApartmentPagingRequest>(request));
			}
			catch (Exception e) {
				_logger.Error($"{nameof(GetApartments)}: {e.Message}");
				throw;
			}

			return new PagingModel<ExtendApartmentResponse> {
				List = _mapper.Map<List<ExtendApartmentResponse>>(apartments.List),
				Page = apartments.Page,
				LastPage = apartments.LastPage,
				Total = apartments.Total,
			};
		}
	}
}
