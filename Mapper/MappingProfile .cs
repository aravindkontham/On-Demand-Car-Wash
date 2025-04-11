using AutoMapper;
using CarWash.DTO;
using CarWash.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));
        CreateMap<UpdateProfileDto, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<CarDto, Car>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CustomerId));

        CreateMap<Car, CarDto>()
           .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.UserId));
        
        CreateMap<PackageDto, Package>().ReverseMap();


    


    }
}
