using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper;

namespace AngularProject.DTO.MappingProfiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            //Dto içerisindeki varlıkları veritabanı içerisindeki varlıklar ile eşler.
            CreateMap<OrderTBL, OrderDto>()
                .ForMember(d => d.orderID, o => o.MapFrom(s => s.orderID))
                .ForMember(d => d.userID, o => o.MapFrom(s => s.userID))
                .ForMember(d => d.totalAmount, o => o.MapFrom(s => s.totalAmount))
                .ForMember(d => d.orderDate, o => o.MapFrom(s => s.orderDate))
                .ForMember(d => d.orderStatus, o => o.MapFrom(s => s.orderStatus))
                .ReverseMap()
                .ForMember(s => s.orderID, o => o.MapFrom(d => d.orderID))
                .ForMember(s => s.userID, o => o.MapFrom(d => d.userID))
                .ForMember(s => s.totalAmount, o => o.MapFrom(d => d.totalAmount))
                .ForMember(s => s.orderDate, o => o.MapFrom(d => d.orderDate))
                .ForMember(s => s.orderStatus, o => o.MapFrom(d => d.orderStatus))
                ;
            ;
        }
    }
}
