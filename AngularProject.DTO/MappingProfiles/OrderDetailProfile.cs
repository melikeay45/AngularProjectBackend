using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.MappingProfiles
{
    public class OrderDetailProfile:Profile
    {
        public OrderDetailProfile()
        {
            //Dto içerisindeki varlıkları veritabanı içerisindeki varlıklar ile eşler.
            CreateMap<OrderDetailTBL, OrderDetailDto>()
                .ForMember(d => d.detailID, o => o.MapFrom(s => s.detailID))
                .ForMember(d => d.orderID, o => o.MapFrom(s => s.orderID))
                .ForMember(d => d.productID, o => o.MapFrom(s => s.productID))
                .ForMember(d => d.quantity, o => o.MapFrom(s => s.quantity))
                .ForMember(d => d.unitPrice, o => o.MapFrom(s => s.unitPrice))
                .ReverseMap()
                .ForMember(s => s.detailID, o => o.MapFrom(d => d.detailID))
                .ForMember(s => s.orderID, o => o.MapFrom(d => d.orderID))
                .ForMember(s => s.productID, o => o.MapFrom(d => d.productID))
                .ForMember(s => s.quantity, o => o.MapFrom(d => d.quantity))
                .ForMember(s => s.unitPrice, o => o.MapFrom(d => d.unitPrice))
                ;
            ;
        }
    }
}
