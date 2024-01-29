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
                .ForMember(d => d.address, o => o.MapFrom(s => s.address))
                .ForMember(d => d.phoneNumber, o => o.MapFrom(s => s.phoneNumber))
                .ForMember(d => d.productID, o => o.MapFrom(s => s.productID))
                .ForMember(d => d.quantity, o => o.MapFrom(s => s.quantity))
                .ForMember(d => d.unitPrice, o => o.MapFrom(s => s.unitPrice))
                .ForMember(d => d.isDelete, o => o.MapFrom(s => s.isDelete))
                .ReverseMap()
                .ForMember(s => s.orderID, o => o.MapFrom(d => d.orderID))
                .ForMember(s => s.userID, o => o.MapFrom(d => d.userID))
                .ForMember(s => s.totalAmount, o => o.MapFrom(d => d.totalAmount))
                .ForMember(s => s.orderDate, o => o.MapFrom(d => d.orderDate))
                .ForMember(s => s.orderStatus, o => o.MapFrom(d => d.orderStatus))
                .ForMember(s => s.address, o => o.MapFrom(d => d.address))
                .ForMember(s => s.phoneNumber, o => o.MapFrom(d => d.phoneNumber))
                .ForMember(s => s.productID, o => o.MapFrom(d => d.productID))
                .ForMember(s => s.quantity, o => o.MapFrom(d => d.quantity))
                .ForMember(s => s.unitPrice, o => o.MapFrom(d => d.unitPrice))
                .ForMember(s => s.isDelete, o => o.MapFrom(d => d.isDelete))
                ;
            ;
        }
    }
}
