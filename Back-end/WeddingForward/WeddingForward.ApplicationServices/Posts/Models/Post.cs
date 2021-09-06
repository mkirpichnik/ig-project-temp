using System;
using System.Collections.Generic;

namespace WeddingForward.ApplicationServices.Posts.Models
{
    public class Post
    {
        public static readonly IDictionary<PostContentType, Type> MappingTypes = new Dictionary<PostContentType, Type>
        {
            {
                PostContentType.Photo, typeof(PostPhotoContent)
            },
            {
                PostContentType.Video, typeof(PostVideoContent)
            },
            {
                PostContentType.Carousel, typeof(PostCarouselContent)
            }
        };

        public string PostId { get; set; }

        public string PostLink { get; set; }

        public string OwnerUsername { get; set; }

        public PostContentType? PostType => Content?.ContentType;

        public PostContent Content { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime LastUpdate { get; set; }
    }

    internal class PostInfoDto
    {
        public string Id { get; set; }

        public PostContentType Type { get; set; }

        public string OwnerUsername { get; set; }

        public string DisplayUrl { get; set; }

        public bool IsVideo { get; set; }

        public IReadOnlyList<string> Tagged { get; set; }

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        public int ViewsCount { get; set; }

        public string VideoUrl { get; set; }

        public double CreatedTimeStamp { get; set; }

        public IReadOnlyList<PostInfoDto> Children { get; set; }
    }
}
