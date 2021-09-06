using AutoMapper;

namespace WeddingForward.ApplicationServices.Posts.Models
{
    internal class PostContentMappingCongiguration: Profile
    {
        public PostContentMappingCongiguration()
        {
            CreateMap<PostInfoDto, PostContent>();
            CreateMap<PostInfoDto, PostPhotoContent>();
            CreateMap<PostInfoDto, PostVideoContent>();
            CreateMap<PostInfoDto, PostCarouselContent>();
        }
    }
}
