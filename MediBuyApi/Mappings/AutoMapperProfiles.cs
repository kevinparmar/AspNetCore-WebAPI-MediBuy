using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MediBuyApi.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<IdentityUser, UserDTO>().ReverseMap();

            CreateMap<Category, EditCategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();

            //CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, EditProductDTO>().ReverseMap();

            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<CartDetail, CartDetailDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<OrderStatus, OrderStatusDTO>().ReverseMap();

            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.StatusName))
            .ReverseMap();

            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        }
    }
}
