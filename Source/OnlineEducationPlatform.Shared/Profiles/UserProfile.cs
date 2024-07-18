using AutoMapper;
using OnlineEducationPlatform.Shared.DTOs;
using OnlineEducationPlatform.Shared.Entities;

namespace OnlineEducationPlatform.Shared.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();
            CreateMap<AuthenticateUserDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();
        }
    }

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseDTO, Course>().ReverseMap();
            CreateMap<ReadCourseDTO, Course>().ReverseMap();
            CreateMap<UpdateCourseDTO, Course>().ReverseMap();
            CreateMap<UpdatedCourseDTO, Course>().ReverseMap();
        }
    }
}