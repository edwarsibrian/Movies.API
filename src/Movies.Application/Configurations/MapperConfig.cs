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
            ConfigureActorMappings();

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

        private void ConfigureActorMappings()
        {
            CreateMap<CreateActorCommand, Actor>();
            //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            //.ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.PictureUrl));
            CreateMap<Actor, ActorDTO>();
        }
    }
}
