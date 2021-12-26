using AutoMapper;
using BLL.Dtos.Account;
using BLL.Dtos.Apartment;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.Customer;
using BLL.Dtos.DeliveryAddress;
using BLL.Dtos.MarketManager;
using BLL.Dtos.Merchant;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.Product;
using BLL.Dtos.ProductCategory;
using BLL.Dtos.SystemCategory;
using BLL.Dtos.New;
using DAL.Models;
using BLL.Dtos.POI;

namespace BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Product Mapping
            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>().ReverseMap();

            //Merchant Mapping
            CreateMap<MerchantRequest, Merchant>();
            CreateMap<Merchant, MerchantResponse>().ReverseMap();

            //Apartment Mapping
            CreateMap<ApartmentRequest, Apartment>();
            CreateMap<Apartment, ApartmentResponse>();

            //Account Mapping
            CreateMap<AccountLoginRequest, Account>();
            CreateMap<AccountRegisterRequest, Account>();
            CreateMap<AccountResponse, Account>().ReverseMap();

            //SystemCategory Mapping
            CreateMap<SystemCategoryRequest, SystemCategory>();
            CreateMap<SystemCategory, SystemCategoryResponse>().ReverseMap();

            //MerchantStore Mapping
            CreateMap<MerchantStoreRequest, MerchantStore>();
            CreateMap<MerchantStore, MerchantStoreResponse>().ReverseMap();

            //ProCategory Mapping
            CreateMap<ProductCategoryRequest, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryResponse>().ReverseMap();

            //Customer Mapping
            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>().ReverseMap();

            //Collection Mapping
            CreateMap<CollectionRequest, Collection>();
            CreateMap<Collection, CollectionResponse>().ReverseMap();

            //CollectionMapping Mapping
            CreateMap<CollectionMapping, CollectionMappingResponse>();

            //DeliveryAddress Mapping
            CreateMap<DeliveryAddressRequest, DeliveryAddress>();
            CreateMap<DeliveryAddress, DeliveryAddressResponse>().ReverseMap();

            //MarketManager Mapping
            CreateMap<MarketManagerRequest, MarketManager>();
            CreateMap<MarketManager, MarketManagerResponse>();

            //News Mapping
            CreateMap<NewsRequest, News>();
            CreateMap<News, NewsResponse>();

            //POI Mapping
            CreateMap<PoiRequest, Poi>();
            CreateMap<Poi, PoiResponse>();
        }
    }
}
