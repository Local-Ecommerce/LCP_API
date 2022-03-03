using AutoMapper;
using BLL.Dtos.Account;
using BLL.Dtos.Apartment;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.Menu;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.MoMo.IPN;
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
using BLL.Dtos.RefreshToken;
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
            CreateMap<Apartment, ApartmentResponse>().ReverseMap();
            CreateMap<Apartment, ExtendApartmentResponse>().ReverseMap();

            //Account Mapping
            CreateMap<AccountRequest, Account>();
            CreateMap<AccountResponse, Account>().ReverseMap();
            CreateMap<Account, ExtendAccountResponse>().ReverseMap();

            //SystemCategory Mapping
            CreateMap<SystemCategoryRequest, SystemCategory>();
            CreateMap<SystemCategoryUpdateRequest, SystemCategory>();
            CreateMap<SystemCategory, SystemCategoryResponse>().ReverseMap();

            //MerchantStore Mapping
            CreateMap<MerchantStoreRequest, MerchantStore>();
            CreateMap<MerchantStoreUpdateRequest, MerchantStore>();
            CreateMap<MerchantStore, MerchantStoreResponse>().ReverseMap();
            CreateMap<MerchantStore, ExtendMerchantStoreResponse>().ReverseMap();

            //ProCategory Mapping
            CreateMap<ProductCategoryRequest, ProductCategory>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryResponse>().ReverseMap();
            CreateMap<ProductCategory, ExtendProductCategoryResponse>();

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
            CreateMap<Menu, ExtendMenuResponse>();

            //Product In Menu Mapping
            CreateMap<ProductInMenuRequest, ProductInMenu>();
            CreateMap<ProductInMenu, ProductInMenuResponse>();
            CreateMap<ProductInMenu, ExtendProductInMenuResponse>();

            //Payment Mapping
            CreateMap<PaymentRequest, Payment>();
            CreateMap<Payment, PaymentResponse>();

            //Payment Method Mapping
            CreateMap<PaymentMethodRequest, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodResponse>();

            //Resident Mapping
            CreateMap<ResidentRequest, Resident>();
            CreateMap<ResidentUpdateRequest, Resident>();
            CreateMap<Resident, ResidentResponse>().ReverseMap();
            CreateMap<Resident, ExtendResidentResponse>().ReverseMap();

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

            //MoMo Mapping
            CreateMap<MoMoIPNRequest, MoMoIPNResponse>();

            //Refresh Token Mapping
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
            CreateMap<RefreshToken, ExtendRefreshTokenDto>().ReverseMap();
        }
    }
}
