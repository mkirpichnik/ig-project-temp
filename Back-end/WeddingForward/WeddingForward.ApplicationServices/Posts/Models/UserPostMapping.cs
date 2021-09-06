using AutoMapper;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Posts.Models
{
    internal class UserPostMapping: Profile
    {
        public UserPostMapping()
        {
            CreateMap<UserPost, AccountLinkedPosts>()
                .ForMember(posts => posts.Order, expression => expression.MapFrom(post => post.OrderNumber));

            CreateMap<AccountLinkedPosts, UserPost>()
                .ForMember(posts => posts.OrderNumber, expression => expression.MapFrom(post => post.Order));
        }
    }
}
