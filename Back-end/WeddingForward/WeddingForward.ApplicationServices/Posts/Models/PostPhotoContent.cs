namespace WeddingForward.ApplicationServices.Posts.Models
{
    public class PostPhotoContent: PostContent
    {
        public string DisplayUrl { get; set; }

        public override string ContentUrl => DisplayUrl;

        public override PostContentType ContentType => PostContentType.Photo;
    }
}
