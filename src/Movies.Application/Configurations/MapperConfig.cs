using AutoMapper;
using Movies.Domain.Entities;

namespace Movies.Application.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<string, Genre>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src));
        }
    }
}
