using AutoMapper;
using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;

namespace AngularProject.DTO.MappingProfiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {

            //Dto içerisindeki varlıkları veritabanı içerisindeki varlıklar ile eşler.
            CreateMap<CategoryTBL, CategoryDto>()
                .ForMember(d => d.categoryID, o => o.MapFrom(s => s.categoryID))
                .ForMember(d => d.categoryName, o => o.MapFrom(s => s.categoryName))
                .ForMember(d => d.description, o => o.MapFrom(s => s.description))
                .ForMember(d => d.isDelete, o => o.MapFrom(s => s.isDelete))


                .ReverseMap()
                .ForMember(s => s.categoryID, o => o.MapFrom(d => d.categoryID))
                .ForMember(s => s.categoryName, o => o.MapFrom(d => d.categoryName))
                .ForMember(d => d.description, o => o.MapFrom(s => s.description))
                .ForMember(s => s.isDelete, o => o.MapFrom(d => d.isDelete))

                ;
            ;
        }
    }
}
