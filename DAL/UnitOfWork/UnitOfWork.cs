using DAL.Models;
using System;
using DAL.Repositories;
using System.Threading.Tasks;
using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork {
	public class UnitOfWork : IUnitOfWork {
		private readonly LoichDBContext _context;
		private IAccountRepository _accountRepository;
		private IApartmentRepository _apartmentRepository;
		private IFeedbackRepository _feedbackRepository;
		private IMenuRepository _menuRepository;
		private IOrderRepository _orderRepository;
		private IMerchantStoreRepository _merchantStoreRepository;
		private IProductRepository _productRepository;
		private INewsRepository _newsRepository;
		private IOrderDetailRepository _orderDetailRepository;
		private IPaymentMethodRepository _paymentMethodRepository;
		private IPaymentRepository _paymentRepository;
		private IPoiRepository _poiRepository;
		private IProductInMenuRepository _productInMenuRepository;
		private IRefreshTokenRepository _refreshTokenRepository;
		private IResidentRepository _residentRepository;
		private IRoleRepository _roleRepository;
		private ISystemCategoryRepository _systemCategoryRepository;

		public IAccountRepository Accounts {
			get {
				_accountRepository ??= new AccountRepository(_context);
				return _accountRepository;
			}
		}

		public IApartmentRepository Apartments {
			get {
				_apartmentRepository ??= new ApartmentRepository(_context);
				return _apartmentRepository;
			}
		}

		public IFeedbackRepository Feedbacks {
			get {
				_feedbackRepository ??= new FeedbackRepository(_context);
				return _feedbackRepository;
			}
		}

		public IMenuRepository Menus {
			get {
				_menuRepository ??= new MenuRepository(_context);
				return _menuRepository;
			}
		}

		public IMerchantStoreRepository MerchantStores {
			get {
				_merchantStoreRepository ??= new MerchantStoreRepository(_context);
				return _merchantStoreRepository;
			}
		}

		public INewsRepository News {
			get {
				_newsRepository ??= new NewsRepository(_context);
				return _newsRepository;
			}
		}

		public IOrderDetailRepository OrderDetails {
			get {
				_orderDetailRepository ??= new OrderDetailRepository(_context);
				return _orderDetailRepository;
			}
		}

		public IOrderRepository Orders {
			get {
				_orderRepository ??= new OrderRepository(_context);
				return _orderRepository;
			}
		}

		public IPaymentRepository Payments {
			get {
				_paymentRepository ??= new PaymentRepository(_context);
				return _paymentRepository;
			}
		}

		public IPaymentMethodRepository PaymentMethods {
			get {
				_paymentMethodRepository ??= new PaymentMethodRepository(_context);
				return _paymentMethodRepository;
			}
		}

		public IPoiRepository Pois {
			get {
				_poiRepository ??= new PoiRepository(_context);
				return _poiRepository;
			}
		}

		public IProductRepository Products {
			get {
				_productRepository ??= new ProductRepository(_context);
				return _productRepository;
			}
		}
		public IProductInMenuRepository ProductInMenus {
			get {
				_productInMenuRepository ??= new ProductInMenuRepository(_context);
				return _productInMenuRepository;
			}
		}
		public IResidentRepository Residents {
			get {
				_residentRepository ??= new ResidentRepository(_context);
				return _residentRepository;
			}
		}
		public IRoleRepository Roles {
			get {
				_roleRepository ??= new RoleRepository(_context);
				return _roleRepository;
			}
		}

		public ISystemCategoryRepository SystemCategories {
			get {
				_systemCategoryRepository ??= new SystemCategoryRepository(_context);
				return _systemCategoryRepository;
			}
		}

		public IRefreshTokenRepository RefreshTokens {
			get {
				_refreshTokenRepository ??= new RefreshTokenRepository(_context);
				return _refreshTokenRepository;
			}
		}

		public UnitOfWork(LoichDBContext context) {
			_context = context;
		}

		/// <summary>
		/// Commit Unit of Work
		/// </summary>
		/// <returns></returns>
		public Task SaveChangesAsync() {
			return _context.SaveChangesAsync();
		}

		/// <summary>
		/// Cancel the commit
		/// </summary>
		public void Dispose() {
			_context.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
