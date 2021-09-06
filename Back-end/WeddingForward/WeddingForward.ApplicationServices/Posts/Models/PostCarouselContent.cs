using System.Collections.Generic;
using System.Linq;

namespace WeddingForward.ApplicationServices.Posts.Models
{
    public class PostCarouselContent: PostContent
    {
        public string DisplayUrl { get; set; }
        
        public override string ContentUrl => DisplayUrl;

        public override PostContentType ContentType => PostContentType.Carousel;

        public IReadOnlyList<PostContent> Slides { get; set; }
    }
}
