using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper;

namespace AngularProject.DTO.MappingProfiles
{
    public class ShoppingCartProfile:Profile
    {
        public ShoppingCartProfile()
        {
            //Dto içerisindeki varlıkları veritabanı içerisindeki varlıklar ile eşler.
            CreateMap<ShoppingCartTBL, ShoppingCartDto>()
                .ForMember(d => d.cartID, o => o.MapFrom(s => s.cartID))
                .ForMember(d => d.userID, o => o.MapFrom(s => s.userID))
                .ForMember(d => d.productID, o => o.MapFrom(s => s.productID))
                .ForMember(d => d.quantity, o => o.MapFrom(s => s.quantity))
                .ForMember(d => d.price, o => o.MapFrom(s => s.price))
                .ReverseMap()
                .ForMember(s => s.cartID, o => o.MapFrom(d => d.cartID))
                .ForMember(s => s.userID, o => o.MapFrom(d => d.userID))
                .ForMember(s => s.productID, o => o.MapFrom(d => d.productID))
                .ForMember(s => s.quantity, o => o.MapFrom(d => d.quantity))
                .ForMember(s => s.price, o => o.MapFrom(d => d.price))

                ;
            ;
        }
    }
}
