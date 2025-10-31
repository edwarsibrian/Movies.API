using AutoMapper;
using Movies.Application.Commands;
using Movies.Application.DTOs;
using Movies.Domain.Entities;

namespace Movies.Application.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //CreateMap<string, Genre>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src));
            //CreateMap<Genre, DTOs.GenreDTO>()
            //    .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Name))
            //    .ReverseMap();
            //CreateMap<UpdateGenreCommand, Genre>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GenreName));
            //CreateMap<GenreDTO, UpdateGenreCommand>();
            ConfigureGenreMappings();

        }

        private void ConfigureGenreMappings()
        {
            CreateMap<string, Genre>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src));
            CreateMap<Genre, DTOs.GenreDTO>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<UpdateGenreCommand, Genre>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GenreName));
            CreateMap<GenreDTO, UpdateGenreCommand>();
        }
    }
}
