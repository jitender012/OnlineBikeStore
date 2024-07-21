using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.AutoMapperConfig
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<product, ProductViewModel>()
                .ForMember(dest=>dest.ImageFile,opt=>opt.Ignore());

            CreateMap<ProductViewModel, product> ();

            CreateMap<product, ProductDetailsViewModel>();
            CreateMap<ProductDetailsViewModel, product>();

            CreateMap<store, StoreViewModel>();
            CreateMap<StoreViewModel, store>();

            CreateMap<category, CategoryViewModel>();
            CreateMap<CategoryViewModel, category>();

            CreateMap<brand, BrandViewModel>();
            CreateMap<BrandViewModel, brand>();

            CreateMap<feedback, FeedbackViewModel>();
            CreateMap<FeedbackViewModel, feedback>();
           
            CreateMap<userCart,CartViewModel>();
            CreateMap<CartViewModel, userCart>();

            CreateMap<ProfileViewModel, user>();
            CreateMap<user, ProfileViewModel>();
        }
    }
}