using AutoMapper;
using BLL.Dtos.Merchant;
using BLL.Dtos.Product;
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
        }
    }
}
