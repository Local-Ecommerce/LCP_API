using AutoMapper;
using BLL.Dtos.Account;
using BLL.Dtos.Apartment;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.Menu;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.News;
using BLL.Dtos.Order;
using BLL.Dtos.OrderDetail;
using BLL.Dtos.Payment;
using BLL.Dtos.PaymentMethod;
using BLL.Dtos.POI;
using BLL.Dtos.Product;
using BLL.Dtos.ProductCategory;
using BLL.Dtos.ProductCombination;
using BLL.Dtos.ProductInMenu;
using BLL.Dtos.Resident;
using BLL.Dtos.StoreMenuDetail;
using BLL.Dtos.SystemCategory;
using DAL.Models;

namespace BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Product Mapping
            CreateMap<ProductRequest, Product>();
            CreateMap<ProductRequest, UpdateProductRequest>();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product, ExtendProductResponse>();
            CreateMap<UpdateProductRequest, Product>();

            //Apartment Mapping
            CreateMap<ApartmentRequest, Apartment>();
            CreateMap<Apartment, ApartmentResponse>();
            CreateMap<Apartment, ExtendApartmentResponse>();


            //Account Mapping
            CreateMap<AccountLoginRequest, Account>();
            CreateMap<AccountRegisterRequest, Account>();
            CreateMap<AccountResponse, Account>().ReverseMap();
            CreateMap<Account, ExtendAccountResponse>();

            //SystemCategory Mapping
            CreateMap<SystemCategoryRequest, SystemCategory>();
            CreateMap<SystemCategoryUpdateRequest, SystemCategory>();
            CreateMap<SystemCategory, SystemCategoryResponse>().ReverseMap();
            CreateMap<SystemCategory, SystemCategoryForAutoCompleteResponse>().ReverseMap();

            //MerchantStore Mapping
            CreateMap<MerchantStoreRequest, MerchantStore>();
            CreateMap<MerchantStoreUpdateRequest, MerchantStore>();
            CreateMap<MerchantStore, MerchantStoreResponse>().ReverseMap();
            CreateMap<MerchantStore, ExtendMerchantStoreResponse>().ReverseMap();

            //ProCategory Mapping
            CreateMap<ProductCategoryRequest, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryResponse>().ReverseMap();
            CreateMap<ProductCategory, ExtendProductCategoryResponse>();

            //Collection Mapping
            CreateMap<CollectionRequest, Collection>();
            CreateMap<CollectionUpdateRequest, Collection>();
            CreateMap<Collection, CollectionResponse>().ReverseMap();

            //CollectionMapping Mapping
            CreateMap<CollectionMapping, CollectionMappingResponse>();

            //News Mapping
            CreateMap<NewsRequest, News>();
            CreateMap<NewsUpdateRequest, News>();
            CreateMap<News, NewsResponse>();
            CreateMap<News, ExtendNewsResponse>();

            //POI Mapping
            CreateMap<PoiRequest, Poi>();
            CreateMap<PoiUpdateRequest, Poi>();
            CreateMap<Poi, PoiResponse>();
            CreateMap<Poi, ExtendPoiResponse>();

            //Menu Mapping
            CreateMap<MenuRequest, Menu>();
            CreateMap<Menu, MenuResponse>();
            CreateMap<MenuUpdateRequest, Menu>();
            CreateMap<Menu, ExtendMenuResponses>();

            //Product In Menu Mapping
            CreateMap<ProductInMenuRequest, ProductInMenu>();
            CreateMap<ProductInMenu, ProductInMenuResponse>();
            CreateMap<ProductInMenu, ExtendProductInMenuResponse>();

            //Store Menu Detail Mapping
            CreateMap<StoreMenuDetailRequest, StoreMenuDetail>();
            CreateMap<StoreMenuDetail, StoreMenuDetailResponse>();
            CreateMap<StoreMenuDetail, ExtendStoreMenuDetailResponse>();

            //Payment Mapping
            CreateMap<PaymentRequest, Payment>();
            CreateMap<Payment, PaymentResponse>();

            //Payment Method Mapping
            CreateMap<PaymentMethodRequest, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodResponse>();

            //Resident Mapping
            CreateMap<ResidentRequest, Resident>();
            CreateMap<ResidentUpdateRequest, Resident>();
            CreateMap<Resident, ResidentResponse>();
            CreateMap<Resident, ExtendResidentResponse>();

            //Order & Order Detail Mapping
            CreateMap<OrderDetailRequest, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailResponse>();
            CreateMap<Order, OrderResponse>();
            CreateMap<Order, ExtendOrderResponse>();

            //Store Menu Detail Mapping
            CreateMap<StoreMenuDetailRequest, StoreMenuDetail>();
            CreateMap<StoreMenuDetailUpdateRequest, StoreMenuDetail>();
            CreateMap<StoreMenuDetail, StoreMenuDetailResponse>();

            //Product Combination Mapping
            CreateMap<ProductCombinationRequest, ProductCombination>();
            CreateMap<ProductCombination, ProductCombinationResponse>();
        }
    }
}
