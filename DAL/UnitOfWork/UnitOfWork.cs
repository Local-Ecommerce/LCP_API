using DAL.Models;
using System;
using DAL.Repositories;
using System.Threading.Tasks;
using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoichDBContext _context;

        public IAccountRepository Accounts { get; private set; }
        public IApartmentRepository Apartments { get; private set; }
        public ICollectionRepository Collections { get; private set; }
        public ICollectionMappingRepository CollectionMappings { get; private set; }
        public IMenuRepository Menus { get; private set; }
        public IMerchantStoreRepository MerchantStores { get; private set; }
        public INewsRepository News { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IPaymentMethodRepository PaymentMethods { get; private set; }
        public IPoiRepository Pois { get; private set; }
        public IProductRepository Products { get; private set; }
        public IProductCategoryRepository ProductCategories { get; private set; }
        public IProductCombinationRepository ProductCombinations { get; private set; }
        public IProductInMenuRepository ProductInMenus { get; private set; }
        public IResidentRepository Residents { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IStoreMenuDetailRepository StoreMenuDetails { get; private set; }
        public ISystemCategoryRepository SystemCategories { get; private set; }

        public UnitOfWork(LoichDBContext context)
        {
            _context = context;
            Accounts = new AccountRepository(context);
            Apartments = new ApartmentRepository(context);
            Collections = new CollectionRepository(context);
            CollectionMappings = new CollectionMappingRepository(context);
            Menus = new MenuRepository(context);
            MerchantStores = new MerchantStoreRepository(context);
            News = new NewsRepository(context);
            Orders = new OrderRepository(context);
            OrderDetails = new OrderDetailRepository(context);
            Payments = new PaymentRepository(context);
            PaymentMethods = new PaymentMethodRepository(context);
            Pois = new PoiRepository(context);
            Products = new ProductRepository(context);
            ProductCategories = new ProductCategoryRepository(context);
            ProductCombinations = new ProductCombinationRepository(context);
            ProductInMenus = new ProductInMenuRepository(context);
            Residents = new ResidentRepository(context);
            Roles = new RoleRepository(context);
            StoreMenuDetails = new StoreMenuDetailRepository(context);
            SystemCategories = new SystemCategoryRepository(context);
        }

        /// <summary>
        /// Commit Unit of Work
        /// </summary>
        /// <returns></returns>
        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


        /// <summary>
        /// Cancel the commit
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
