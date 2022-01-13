using DAL.Models;
using System;
using DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoichDBContext _context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IAccountRepository Accounts { get; private set; }
        public IApartmentRepository Apartments { get; private set; }
        public ICollectionRepository Collections { get; private set; }
        public ICollectionMappingRepository CollectionMappings { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IDeliveryAddressRepository DeliveryAddresses { get; private set; }
        public IMarketManagerRepository MarketManagers { get; private set; }
        public IMenuRepository Menus { get; private set; }
        public IMerchantRepository Merchants { get; private set; }
        public IMerchantLevelRepository MerchantLevels { get; private set; }
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
        public ISystemCategoryRepository SystemCategoryDetails { get; private set; }

        public UnitOfWork(LoichDBContext context)
        {
            _context = context;
            Accounts = new AccountRepository(_context);
            Apartments = new ApartmentRepository(_context);
            Collections = new CollectionRepository(_context);
            CollectionMappings = new CollectionMappingRepository(_context);
            Customers = new CustomerRepository(_context);
            DeliveryAddresses = new DeliveryAddressRepository(_context);
            MarketManagers = new MarketManagerRepository(_context);
            Menus = new MenuRepository(_context);
            Merchants = new MerchantRepository(_context);
            MerchantLevels = new MerchantLevelResponsitory(_context);
            MerchantStores = new MerchantStoreRepository(_context);
            News = new NewsRepository(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
            Payments = new PaymentRepository(_context);
            PaymentMethods = new PaymentMethodRepository(context);
            Pois = new PoiRepository(_context);
            Products = new ProductRepository(_context);
            ProductCategories = new ProductCategoryRepository(_context);
            ProductCombinations = new ProductCombinationRepository(_context);
            ProductInMenus = new ProductInMenuRepository(_context);
            Residents = new ResidentRepository(_context);
            Roles = new RoleRepository(_context);
            StoreMenuDetails = new StoreMenuDetailRepository(_context);
            SystemCategoryDetails = new SystemCategoryRepository(context);
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

        /// <summary>
        /// Get Repository of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)repositories[type];
        }
    }
}
