using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper;

namespace AngularProject.DTO.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            //Dto içerisindeki varlıkları veritabanı içerisindeki varlıklar ile eşler.
            CreateMap<ProductTBL, ProductDto>()
                .ForMember(d => d.productID, o => o.MapFrom(s => s.productID))
                .ForMember(d => d.productName, o => o.MapFrom(s => s.productName))
                .ForMember(d => d.productDescription, o => o.MapFrom(s => s.productDescription))
                .ForMember(d => d.price, o => o.MapFrom(s => s.price))
                .ForMember(d => d.stockQuantity, o => o.MapFrom(s => s.stockQuantity))
                .ForMember(d => d.categoryID, o => o.MapFrom(s => s.categoryID))
                .ForMember(d => d.imageURL, o => o.MapFrom(s => s.imageURL))
                .ReverseMap()
                .ForMember(s => s.productID, o => o.MapFrom(d => d.productID))
                .ForMember(s => s.productName, o => o.MapFrom(d => d.productName))
                .ForMember(s => s.productDescription, o => o.MapFrom(d => d.productDescription))
                .ForMember(s => s.price, o => o.MapFrom(d => d.price))
                .ForMember(s => s.stockQuantity, o => o.MapFrom(d => d.stockQuantity))
                .ForMember(s => s.categoryID, o => o.MapFrom(d => d.categoryID))
                .ForMember(s => s.imageURL, o => o.MapFrom(d => d.imageURL))
                ;
            ;
        }
    }
}
