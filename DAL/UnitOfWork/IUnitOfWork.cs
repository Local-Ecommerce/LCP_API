﻿using DAL.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IApartmentRepository Apartments { get; }
        ICollectionRepository Collections { get; }
        ICollectionMappingRepository CollectionMappings { get; }
        ICustomerRepository Customers { get; }
        IDeliveryAddressRepository DeliveryAddresses { get; }
        IMarketManagerRepository MarketManagers { get; }
        IMenuRepository Menus { get; }
        IMerchantLevelRepository MerchantLevels { get; }
        IMerchantRepository Merchants { get; }
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
