using AutoMapper;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;

namespace s4dServer.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            //User
            CreateMap<User, UserResponseDTO>();
            CreateMap<UserRequestDTO, User>();

            //Album
            CreateMap<Album, AlbumResponseDTO>();
            CreateMap<AlbumRequestDTO, Album>();

            //Artist
            CreateMap<Artist, ArtistResponseDTO>();
            CreateMap<ArtistRequestDTO, Artist>();

            //Category
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryRequestDTO, Category>();

            //Product
            CreateMap<Product, ProductResponseDTO>();
            CreateMap<ProductRequestDTO, Product>();

            //Order
            CreateMap<Order, OrderResponseDTO>();
            CreateMap<OrderRequestDTO, Order>();

            //OrderDetail
            CreateMap<OrderDetail, OrderDetailResponseDTO>();
            CreateMap<OrderDetailRequestDTO, OrderDetail>();

            //Promotion
            CreateMap<Promotion, PromotionResponseDTO>();
            CreateMap<PromotionRequestDTO, Promotion>();

            //Review
            CreateMap<Review, ReviewResponseDTO>();
            CreateMap<ReviewRequestDTO, Review>();
        }
    }
}
