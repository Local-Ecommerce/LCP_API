using AutoMapper;
using BLL.Dtos.Account;
using BLL.Dtos.LocalZone;
using BLL.Dtos.Merchant;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.Product;
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
            CreateMap<Product, ProductResponse>().ReverseMap();

            //Merchant Mapping
            CreateMap<MerchantRequest, Merchant>();
            CreateMap<Merchant, MerchantResponse>().ReverseMap();

            //LocalZone Mapping
            CreateMap<LocalZoneRequest, LocalZone>();
            CreateMap<LocalZone, LocalZoneResponse>();

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
        }
    }
}
