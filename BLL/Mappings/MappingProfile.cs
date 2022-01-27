using AutoMapper;
using BLL.Dtos.Account;
using BLL.Dtos.Apartment;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.Product;
using BLL.Dtos.ProductCategory;
using BLL.Dtos.SystemCategory;
using BLL.Dtos.News;
using DAL.Models;
using BLL.Dtos.POI;
using BLL.Dtos.Menu;
using BLL.Dtos.ProductInMenu;
using BLL.Dtos.StoreMenuDetail;
using BLL.Dtos.Payment;
using BLL.Dtos.PaymentMethod;
using BLL.Dtos.Resident;
using BLL.Dtos.OrderDetail;
using BLL.Dtos.Order;
using BLL.Dtos.ProductCombination;

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

            //Account Mapping
            CreateMap<AccountLoginRequest, Account>();
            CreateMap<AccountRegisterRequest, Account>();
            CreateMap<AccountResponse, Account>().ReverseMap();

            //SystemCategory Mapping
            CreateMap<SystemCategoryRequest, SystemCategory>();
            CreateMap<SystemCategoryUpdateRequest, SystemCategory>();
            CreateMap<SystemCategory, SystemCategoryResponse>().ReverseMap();
            CreateMap<SystemCategory, SystemCategoryForAutoCompleteResponse>().ReverseMap();

            //MerchantStore Mapping
            CreateMap<MerchantStoreRequest, MerchantStore>();
            CreateMap<MerchantStoreUpdateRequest, MerchantStore>();
            CreateMap<MerchantStore, MerchantStoreResponse>().ReverseMap();

            //ProCategory Mapping
            CreateMap<ProductCategoryRequest, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryResponse>().ReverseMap();

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

            //POI Mapping
            CreateMap<PoiRequest, Poi>();
            CreateMap<PoiUpdateRequest, Poi>();
            CreateMap<Poi, PoiResponse>();

            //Menu Mapping
            CreateMap<MenuRequest, Menu>();
            CreateMap<Menu, MenuResponse>();
            CreateMap<MenuUpdateRequest, Menu>();
            CreateMap<Menu, MenuForStoreResponse>();

            //Product In Menu Mapping
            CreateMap<ProductInMenuRequest, ProductInMenu>();
            CreateMap<ProductInMenu, ProductInMenuResponse>();

            //Store Menu Detail Mapping
            CreateMap<StoreMenuDetailRequest, StoreMenuDetail>();
            CreateMap<StoreMenuDetail, StoreMenuDetailResponse>();

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
            CreateMap<Resident, ResidentResponseForApartment>();

            //Order & Order Detail Mapping
            CreateMap<OrderDetailRequest, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailResponse>();
            CreateMap<Order, OrderResponse>();

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
