using DAL.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IApartmentRepository Apartments { get; }
        IMenuRepository Menus { get; }
        IMerchantStoreRepository MerchantStores { get; }
        INewsRepository News { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IPaymentMethodRepository PaymentMethods { get; }
        IPaymentRepository Payments { get; }
        IPoiRepository Pois { get; }
        IProductCategoryRepository ProductCategories { get; }
        IProductCombinationRepository ProductCombinations { get; }
        IProductInMenuRepository ProductInMenus { get; }
        IProductRepository Products { get; }
        IResidentRepository Residents { get; }
        IRoleRepository Roles { get; }
        IStoreMenuDetailRepository StoreMenuDetails { get; }
        ISystemCategoryRepository SystemCategories { get; }
        Task SaveChangesAsync();
    }
}
