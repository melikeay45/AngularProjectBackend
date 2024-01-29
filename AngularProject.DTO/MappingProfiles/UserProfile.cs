using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper;


namespace AngularProject.DTO.MappingProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            //Dto içerisindeki varlıkları veritabanı içerisindeki varlıklar ile eşler.
            CreateMap<UserTBL, UserDto>()
                .ForMember(d => d.userID, o => o.MapFrom(s => s.userID))
                .ForMember(d => d.name, o => o.MapFrom(s => s.name))
                .ForMember(d => d.surname, o => o.MapFrom(s => s.surname))
                .ForMember(d => d.username, o => o.MapFrom(s => s.username))
                .ForMember(d => d.password, o => o.MapFrom(s => s.password))
                .ForMember(d => d.email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.address, o => o.MapFrom(s => s.address))
                .ForMember(d => d.phoneNumber, o => o.MapFrom(s => s.phoneNumber))
                .ForMember(d => d.registrationDate, o => o.MapFrom(s => s.registrationDate))
                .ForMember(d => d.userType, o => o.MapFrom(s => s.userType))
                .ForMember(d => d.birthdate, o => o.MapFrom(s => s.birthdate))
                .ForMember(d => d.isDelete, o => o.MapFrom(s => s.isDelete))
                .ReverseMap()
                .ForMember(s => s.userID, o => o.MapFrom(d => d.userID))
                .ForMember(s => s.name, o => o.MapFrom(d => d.name))
                .ForMember(s => s.surname, o => o.MapFrom(d => d.surname))
                .ForMember(s => s.username, o => o.MapFrom(d => d.username))
                .ForMember(s => s.password, o => o.MapFrom(d => d.password))
                .ForMember(s => s.email, o => o.MapFrom(d => d.email))
                .ForMember(s => s.address, o => o.MapFrom(d => d.address))
                .ForMember(s => s.phoneNumber, o => o.MapFrom(d => d.phoneNumber))
                .ForMember(s => s.registrationDate, o => o.MapFrom(d => d.registrationDate))
                .ForMember(s => s.userType, o => o.MapFrom(d => d.userType))
                .ForMember(s => s.birthdate, o => o.MapFrom(d => d.birthdate))
                .ForMember(s => s.isDelete, o => o.MapFrom(d => d.isDelete))
                ;
            ;
        }
    }
}
