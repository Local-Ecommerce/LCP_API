using DAL.Models;
using System;
using DAL.Repositories;
using System.Threading.Tasks;
using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private LoichDBContext _context;
        private IAccountRepository _accountRepository;
        private IApartmentRepository _apartmentRepository;
        private IMenuRepository _menuRepository;
        private IOrderRepository _orderRepository;
        private IMerchantStoreRepository _merchantStoreRepository;
        private IProductRepository _productRepository;
        private INewsRepository _newsRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IPaymentMethodRepository _paymentMethodRepository;
        private IPaymentRepository _paymentRepository;
        private IPoiRepository _poiRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private IProductCombinationRepository _productCombinationRepository;
        private IProductInMenuRepository _productInMenuRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IResidentRepository _residentRepository;
        private IRoleRepository _roleRepository;
        private IStoreMenuDetailRepository _storeMenuDetailRepository;
        private ISystemCategoryRepository _systemCategoryRepository;

        public IAccountRepository Accounts
        {
            get
            {
                if (_accountRepository == null) _accountRepository = new AccountRepository(_context);
                return _accountRepository;
            }
        }

        public IApartmentRepository Apartments
        {
            get
            {
                if (_apartmentRepository == null) _apartmentRepository = new ApartmentRepository(_context);
                return _apartmentRepository;
            }
        }

        public IMenuRepository Menus
        {
            get
            {
                if (_menuRepository == null) _menuRepository = new MenuRepository(_context);
                return _menuRepository;
            }
        }

        public IMerchantStoreRepository MerchantStores
        {
            get
            {
                if (_merchantStoreRepository == null) _merchantStoreRepository = new MerchantStoreRepository(_context);
                return _merchantStoreRepository;
            }
        }

        public INewsRepository News
        {
            get
            {
                if (_newsRepository == null) _newsRepository = new NewsRepository(_context);
                return _newsRepository;
            }
        }

        public IOrderDetailRepository OrderDetails
        {
            get
            {
                if (_orderDetailRepository == null) _orderDetailRepository = new OrderDetailRepository(_context);
                return _orderDetailRepository;
            }
        }

        public IOrderRepository Orders
        {
            get
            {
                if (_orderRepository == null) _orderRepository = new OrderRepository(_context);
                return _orderRepository;
            }
        }

        public IPaymentRepository Payments
        {
            get
            {
                if (_paymentRepository == null) _paymentRepository = new PaymentRepository(_context);
                return _paymentRepository;
            }
        }

        public IPaymentMethodRepository PaymentMethods
        {
            get
            {
                if (_paymentMethodRepository == null) _paymentMethodRepository = new PaymentMethodRepository(_context);
                return _paymentMethodRepository;
            }
        }

        public IPoiRepository Pois
        {
            get
            {
                if (_poiRepository == null) _poiRepository = new PoiRepository(_context);
                return _poiRepository;
            }
        }

        public IProductRepository Products
        {
            get
            {
                if (_productRepository == null) _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }

        public IProductCategoryRepository ProductCategories
        {
            get
            {
                if (_productCategoryRepository == null) _productCategoryRepository = new ProductCategoryRepository(_context);
                return _productCategoryRepository;
            }
        }

        public IProductCombinationRepository ProductCombinations
        {
            get
            {
                if (_productCombinationRepository == null) _productCombinationRepository = new ProductCombinationRepository(_context);
                return _productCombinationRepository;
            }
        }

        public IProductInMenuRepository ProductInMenus
        {
            get
            {
                if (_productInMenuRepository == null) _productInMenuRepository = new ProductInMenuRepository(_context);
                return _productInMenuRepository;
            }
        }
        public IResidentRepository Residents
        {
            get
            {
                if (_residentRepository == null) _residentRepository = new ResidentRepository(_context);
                return _residentRepository;
            }
        }
        public IRoleRepository Roles
        {
            get
            {
                if (_roleRepository == null) _roleRepository = new RoleRepository(_context);
                return _roleRepository;
            }
        }
        public IStoreMenuDetailRepository StoreMenuDetails
        {
            get
            {
                if (_storeMenuDetailRepository == null) _storeMenuDetailRepository = new StoreMenuDetailRepository(_context);
                return _storeMenuDetailRepository;
            }
        }

        public ISystemCategoryRepository SystemCategories
        {
            get
            {
                if (_systemCategoryRepository == null)
                    _systemCategoryRepository = new SystemCategoryRepository(_context);
                return _systemCategoryRepository;
            }
        }

        public IRefreshTokenRepository RefreshTokens
        {
            get
            {
                if (_refreshTokenRepository == null)
                    _refreshTokenRepository = new RefreshTokenRepository(_context);
                return _refreshTokenRepository;
            }
        }

        public UnitOfWork(LoichDBContext context)
        {
            _context = context;
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
