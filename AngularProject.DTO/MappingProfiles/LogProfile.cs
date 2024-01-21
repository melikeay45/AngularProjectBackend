using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper;

namespace AngularProject.DTO.MappingProfiles
{
    public class LogProfile:Profile
    {
        public LogProfile()
        {
            //Dto içerisindeki varlıkları veritabanı içerisindeki varlıklar ile eşler.
            CreateMap<LogTBL, LogDto>()
                .ForMember(d => d.logID, o => o.MapFrom(s => s.logID))
                .ForMember(d => d.userID, o => o.MapFrom(s => s.userID))
                .ForMember(d => d.logActivity, o => o.MapFrom(s => s.logActivity))
                .ForMember(d => d.logDate, o => o.MapFrom(s => s.logDate))
                .ForMember(d => d.ipAddress, o => o.MapFrom(s => s.ipAddress))
                .ForMember(d => d.logUsername, o => o.MapFrom(s => s.logUsername))
                .ReverseMap()
                .ForMember(s => s.logID, o => o.MapFrom(d => d.logID))
                .ForMember(s => s.userID, o => o.MapFrom(d => d.userID))
                .ForMember(s => s.logActivity, o => o.MapFrom(d => d.logActivity))
                .ForMember(s => s.logDate, o => o.MapFrom(d => d.logDate))
                .ForMember(s => s.ipAddress, o => o.MapFrom(d => d.ipAddress))
                .ForMember(s => s.logUsername, o => o.MapFrom(d => d.logUsername))
                ;
            ;
        }
    }
}
