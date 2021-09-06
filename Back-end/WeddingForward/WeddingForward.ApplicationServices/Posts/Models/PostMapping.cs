using System.Linq;
using AutoMapper;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Posts.Models
{
    internal class PostMapping: Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostSet>();
            CreateMap<PostSet, Post>();
        }
    }

    internal class PostContentMapping : Profile
    {
        public PostContentMapping()
        {
            CreateMap<PostCarouselContent, PostContentSet>()
                .ForMember(set => set.Tagged, expression => expression.Ignore());
            CreateMap<PostPhotoContent, PostContentSet>()
                .ForMember(set => set.Tagged, expression => expression.Ignore());
            CreateMap<PostVideoContent, PostContentSet>()
                .ForMember(set => set.Tagged, expression => expression.Ignore());

            CreateMap<PostContentSet, PostCarouselContent>()
                .ForMember(content => content.DisplayUrl, expression => expression.MapFrom(set => set.ContentUrl))
                .ForMember(content => content.Tagged, expression =>
                        expression.MapFrom(set => set.Tagged.Select(referencesSet => referencesSet.Username)));
            CreateMap<PostContentSet, PostPhotoContent>()
                .ForMember(content => content.DisplayUrl, expression => expression.MapFrom(set => set.ContentUrl))
                .ForMember(content => content.Tagged, expression =>
                    expression.MapFrom(set => set.Tagged.Select(referencesSet => referencesSet.Username)));
            CreateMap<PostContentSet, PostVideoContent>()
                .ForMember(content => content.VideoUrl, expression => expression.MapFrom(set => set.ContentUrl))
                .ForMember(content => content.Tagged, expression =>
                    expression.MapFrom(set => set.Tagged.Select(referencesSet => referencesSet.Username)));
        }
    }
}
