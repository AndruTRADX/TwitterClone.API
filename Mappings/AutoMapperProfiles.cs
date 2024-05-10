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
            CreateMap<Tweet, TweetDTOListItem>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Like, LikeDTO>().ReverseMap();
            CreateMap<LikeToComment, LikeDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
        }
    }
}
