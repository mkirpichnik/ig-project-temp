namespace WeddingForward.ApplicationServices.Posts.Models
{
    public class PostVideoContent: PostContent
    {
        public string VideoUrl { get; set; }

        public int ViewsCount { get; set; }

        public override string ContentUrl => VideoUrl;

        public override PostContentType ContentType => PostContentType.Video;
    }
}
