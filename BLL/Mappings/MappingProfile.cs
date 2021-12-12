﻿using AutoMapper;
using BLL.Dtos.Account;
using BLL.Dtos.Apartment;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.Customer;
using BLL.Dtos.DeliveryAddress;
using BLL.Dtos.Merchant;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.Product;
using BLL.Dtos.ProductCategory;
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

            CreateMap<DeliveryAddressRequest, DeliveryAddress>();
            CreateMap<DeliveryAddress, DeliveryAddressResponse>().ReverseMap();
        }
    }
}
