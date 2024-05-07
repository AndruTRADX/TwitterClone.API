using AutoMapper;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Tweet, TweetDTO>().ReverseMap();
            CreateMap<Tweet, TweetDTOListItem>()
                .ForMember(dest => dest.CommentsCount,
                           opt => opt.MapFrom(src => src.Comments.Count))
                .ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
