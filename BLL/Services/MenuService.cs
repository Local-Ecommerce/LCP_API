using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Menu;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IValidateDataService _validateDataService;
        private const string PREFIX = "MN_";

        public MenuService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IValidateDataService validateDataService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _validateDataService = validateDataService;
        }

        /// <summary>
        /// Create Menu
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="menuRequest"></param>
        /// <returns></returns>
        public async Task<MenuResponse> CreateMenu(string residentId, MenuRequest menuRequest)
        {
            Menu menu = _mapper.Map<Menu>(menuRequest);
            try
            {
                //check if another menu use that time
                string menuName = await GetOtherMenuHasSameTime(TimeSpan.Parse(menuRequest.TimeStart),
                    TimeSpan.Parse(menuRequest.TimeEnd), menuRequest.RepeatDate, residentId);

                if (menuName != null)
                    throw new BusinessException($"Đã có menu {menuName} sử dụng khung giờ đó");

                menu.MenuId = _utilService.CreateId(PREFIX);
                menu.CreatedDate = _utilService.CurrentTimeInVietnam();
                menu.UpdatedDate = _utilService.CurrentTimeInVietnam();
                menu.Status = (int)MenuStatus.ACTIVE_MENU;
                menu.BaseMenu = false;

                _unitOfWork.Menus.Add(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.CreateMenu()]: " + e.Message);
                throw;
            }

            return _mapper.Map<MenuResponse>(menu);
        }


        /// <summary>
        ///  Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuUpdateRequest"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<MenuResponse> UpdateMenuById(string id, MenuUpdateRequest menuUpdateRequest, string residentId)
        {
            Menu menu;
            try
            {
                menu = await _unitOfWork.Menus.FindAsync(m => m.MenuId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateMenuById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), id);
            }

            //Update Menu to DB
            try
            {
                //check is valid time
                if ((menuUpdateRequest.TimeStart != null && !_validateDataService.IsValidTime(menuUpdateRequest.TimeStart)) ||
                (menuUpdateRequest.TimeEnd != null && !_validateDataService.IsValidTime(menuUpdateRequest.TimeEnd)))
                    throw new BusinessException("Định dạng thời gian không hợp lệ");

                //check if another menu use that time
                if (menuUpdateRequest.TimeStart != null || menuUpdateRequest.TimeEnd != null)
                {
                    TimeSpan timeStart = menuUpdateRequest.TimeStart != null ?
                        TimeSpan.Parse(menuUpdateRequest.TimeStart) : (TimeSpan)menu.TimeStart;

                    TimeSpan timeEnd = menuUpdateRequest.TimeEnd != null ?
                        TimeSpan.Parse(menuUpdateRequest.TimeEnd) : (TimeSpan)menu.TimeEnd;

                    string menuName = await GetOtherMenuHasSameTime(timeStart, timeEnd, menuUpdateRequest.RepeatDate, residentId);

                    if (menuName != null)
                        throw new BusinessException($"Đã có menu {menuName} sử dụng khung giờ đó");
                }

                menu = _mapper.Map(menuUpdateRequest, menu);
                menu.UpdatedDate = _utilService.CurrentTimeInVietnam();

                _unitOfWork.Menus.Update(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateMenuById()]: " + e.Message);
                throw;
            }

            return _mapper.Map<MenuResponse>(menu);
        }


        /// <summary>
        /// Delete Menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteMenuById(string id)
        {
            //Check id
            Menu menu;
            try
            {
                menu = await _unitOfWork.Menus.FindAsync(menu => menu.MenuId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteMenuById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), id);
            }

            //Delete Menu
            try
            {
                menu.Status = (int)MenuStatus.DELETED_MENU;
                menu.UpdatedDate = _utilService.CurrentTimeInVietnam();

                _unitOfWork.Menus.Update(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteMenuById()]: " + e.Message);
                throw;
            }
        }


        /// <summary>
        /// Create Base Menu
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        public MenuResponse CreateBaseMenu(string merchantStoreId)
        {
            Menu menu = new()
            {
                MenuId = _utilService.CreateId(PREFIX),
                MenuName = "Bảng giá gốc",
                CreatedDate = _utilService.CurrentTimeInVietnam(),
                UpdatedDate = _utilService.CurrentTimeInVietnam(),
                TimeStart = new TimeSpan(0, 0, 0),
                TimeEnd = new TimeSpan(23, 59, 59),
                RepeatDate = "0123456",
                Status = (int)MenuStatus.ACTIVE_MENU,
                MerchantStoreId = merchantStoreId,
                BaseMenu = true
            };

            _unitOfWork.Menus.Add(menu);

            MenuResponse menuResponse = _mapper.Map<MenuResponse>(menu);

            return menuResponse;
        }


        /// <summary>
        /// Get Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="residentId"></param>
        /// <param name="apartmentId"></param>
        /// <param name="merchantStoreId"></param>
        /// <param name="productId"></param>
        /// <param name="search"></param>
        /// <param name="isActive"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<object> GetMenus(
            string id, int?[] status, string residentId,
            string apartmentId, string merchantStoreId, string productId,
            string search, bool? isActive, int? limit,
            int? page, string sort, string[] include, string role)
        {
            PagingModel<Menu> menus;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                if (!role.Equals(ResidentType.MERCHANT))
                    residentId = null;
                menus = await _unitOfWork.Menus
                    .GetMenu(id, status, residentId, apartmentId, merchantStoreId,
                        productId, search, isActive, limit, page, isAsc, propertyName, include);
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetMenu()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendMenuResponse>
            {
                List = _mapper.Map<List<ExtendMenuResponse>>(menus.List),
                Page = menus.Page,
                LastPage = menus.LastPage,
                Total = menus.Total,
            };
        }


        /// <summary>
        /// Get Other Menu Has Same Time
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <param name="repeatDate"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<string> GetOtherMenuHasSameTime(TimeSpan timeStart, TimeSpan timeEnd, string repeatDate, string residentId)
        {
            try
            {
                string[] repeatDateCharArray = repeatDate.ToCharArray().Select(c => c.ToString()).ToArray();

                List<Menu> menus = (await _unitOfWork.Menus.GetMenu(residentId: residentId)).List;
                foreach (Menu m in menus)
                {
                    //if contain same day of week
                    if (!(bool)m.BaseMenu && repeatDateCharArray.Any(m.RepeatDate.Contains))
                    {
                        //if m.TimeStart <= timeStart or timeEnd <= m.TimeEnd
                        if ((TimeSpan.Compare(timeStart, (TimeSpan)m.TimeStart) >= 0 &&
                                        (TimeSpan.Compare(timeStart, (TimeSpan)m.TimeEnd) < 0))
                            || (TimeSpan.Compare(timeEnd, (TimeSpan)m.TimeStart) > 0 &&
                                        (TimeSpan.Compare(timeEnd, (TimeSpan)m.TimeEnd) <= 0)))
                            return m.MenuName;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.IsSameTimeWithOtherMenu()]" + e.Message);
                throw;
            }
            return null;
        }
    }
}
