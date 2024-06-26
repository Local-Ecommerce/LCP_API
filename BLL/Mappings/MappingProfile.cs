﻿using AutoMapper;
using BLL.Dtos.Account;
using BLL.Dtos.Apartment;
using BLL.Dtos.Feedback;
using BLL.Dtos.Menu;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.News;
using BLL.Dtos.Order;
using BLL.Dtos.OrderDetail;
using BLL.Dtos.Payment;
using BLL.Dtos.PaymentMethod;
using BLL.Dtos.POI;
using BLL.Dtos.Product;
using BLL.Dtos.ProductInMenu;
using BLL.Dtos.RefreshToken;
using BLL.Dtos.Resident;
using BLL.Dtos.SystemCategory;
using DAL.Models;

namespace BLL.Mappings {
	public class MappingProfile : Profile {
		public MappingProfile() {
			//Product Mapping
			CreateMap<ProductRequest, Product>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<Product, ProductResponse>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<ProductResponse, Product>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<Product, UpdateProductResponse>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<Product, BaseProductResponse>()
					.ForMember(dest => dest.RelatedProducts, act => act.MapFrom(src => src.InverseBelongToNavigation));
			CreateMap<ExtendProductRequest, Product>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<BaseProductRequest, Product>()
					.ForSourceMember(src => src.ToBaseMenu, dest => dest.DoNotValidate());
			CreateMap<UpdateProductResponse, BaseProductResponse>();
			CreateMap<Product, RelatedProductResponse>()
					.ForMember(dest => dest.BaseProduct, act => act.MapFrom(src => src.BelongToNavigation));

			//Apartment Mapping
			CreateMap<ApartmentRequest, Apartment>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<GetApartmentRequest, ApartmentPagingRequest>();
			CreateMap<Apartment, ApartmentResponse>().ReverseMap();
			CreateMap<Apartment, ExtendApartmentResponse>().ReverseMap();

			//Account Mapping
			CreateMap<AccountRequest, Account>();
			CreateMap<AccountResponse, Account>().ReverseMap();
			CreateMap<Account, ExtendAccountResponse>().ReverseMap();

			//SystemCategory Mapping
			CreateMap<SystemCategoryRequest, SystemCategory>();
			CreateMap<SystemCategoryUpdateRequest, SystemCategory>()
							.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<SystemCategory, SystemCategoryResponse>().ReverseMap();
			CreateMap<SystemCategory, ParentSystemCategoryResponse>()
					.ForMember(dest => dest.Children, act => act.MapFrom(src => src.InverseBelongToNavigation));
			CreateMap<SystemCategory, ChildSystemCategoryResponse>()
					.ForMember(dest => dest.Parent, act => act.MapFrom(src => src.BelongToNavigation));

			//MerchantStore Mapping
			CreateMap<MerchantStoreRequest, MerchantStore>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<MerchantStore, MerchantStoreResponse>();
			CreateMap<MerchantStore, ExtendMerchantStoreResponse>().ReverseMap();

			//News Mapping
			CreateMap<NewsRequest, News>();
			CreateMap<NewsUpdateRequest, News>()
							.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<News, NewsResponse>();
			CreateMap<News, ExtendNewsResponse>();

			//POI Mapping
			CreateMap<PoiRequest, Poi>();
			CreateMap<PoiUpdateRequest, Poi>()
							.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<Poi, PoiResponse>();
			CreateMap<Poi, ExtendPoiResponse>();

			//Menu Mapping
			CreateMap<MenuRequest, Menu>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<MenuUpdateRequest, Menu>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<Menu, MenuResponse>();
			CreateMap<Menu, ExtendMenuResponse>();

			//Product In Menu Mapping
			CreateMap<ProductInMenuRequest, ProductInMenu>();
			CreateMap<ProductInMenu, ProductInMenuResponse>();
			CreateMap<ProductInMenu, ExtendProductInMenuResponse>();
			CreateMap<ExtendProductInMenuResponse, BaseProductInMenuResponse>();
			CreateMap<ProductInMenu, BaseProductInMenuResponse>();

			//Payment Mapping
			CreateMap<PaymentRequest, Payment>();
			CreateMap<Payment, PaymentResponse>();

			//Payment Method Mapping
			CreateMap<PaymentMethodRequest, PaymentMethod>();
			CreateMap<PaymentMethod, PaymentMethodResponse>();

			//Resident Mapping
			CreateMap<ResidentUpdateRequest, Resident>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<Resident, ResidentResponse>().ReverseMap();
			CreateMap<Resident, ExtendResidentResponse>().ReverseMap();

			//Order & Order Detail Mapping
			CreateMap<OrderDetailRequest, OrderDetail>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<OrderDetail, OrderDetailResponse>();
			CreateMap<Order, OrderResponse>();
			CreateMap<Order, ExtendOrderResponse>();

			//Refresh Token Mapping
			CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
			CreateMap<RefreshToken, ExtendRefreshTokenDto>().ReverseMap();

			//Feedback Mapping
			CreateMap<FeedbackRequest, Feedback>()
					.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<Feedback, FeedbackResponse>();
		}
	}
}
