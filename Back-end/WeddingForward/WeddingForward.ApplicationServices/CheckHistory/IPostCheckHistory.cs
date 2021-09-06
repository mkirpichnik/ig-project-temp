using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices.Extensions;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.CheckHistory
{
    public interface IPostCheckHistory
    {
        Task<ServiceResult<IReadOnlyList<Post>>> GetAccountsPostsAsync(string accountName);
    }

    internal class PostCheckHistory : IPostCheckHistory
    {
        private readonly WeddingForwardContext _db;

        private readonly IMapper _mapper;

        public PostCheckHistory(WeddingForwardContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IReadOnlyList<Post>>> GetAccountsPostsAsync(string accountName)
        {
            IReadOnlyList<string> postsId = await _db.PostCheckerHistory
                .Where(set => set.AccountId.Equals(accountName))
                .Select(set => set.PostId)
                .Distinct()
                .ToListAsync().ConfigureAwait(false);

            if (postsId.Count == 0)
            {
                return new SuccessServiceResult<IReadOnlyList<Post>>(new List<Post>(0));
            }

            IReadOnlyList<PostSet> postSet = await _db.Posts
                .Where(set => postsId.Contains(set.PostId))
                .Include(set => set.PostContents)
                .ThenInclude(set => set.Tagged)
                .ToListAsync()
                .ConfigureAwait(false);

            IReadOnlyList<Post> posts = postSet.Select(set =>
            {
                Post post = _mapper.Map<Post>(set);
                if (post == null)
                {
                    return null;
                }

                post.Content = ConvertPostContent(set.PostContents);
                return post;
            }).ToList();

            return new SuccessServiceResult<IReadOnlyList<Post>>(posts);

            //foreach (PostSet postSet in posts)
            //{
            //    PostCheckerHistorySet postCheckerHistorySet = _db.PostCheckerHistory
            //        .Where(set => set.PostId.Equals(postSet.PostId))
            //        .OrderByDescending(set => set.CheckedAt).FirstOrDefault();

            //    if (postCheckerHistorySet == null)
            //    {
            //        continue;
            //    }

            //    PostCheckDetailsSet postCheckDetailsSet =
            //        _db.PostCheckDetails.FirstOrDefault(set => set.Id.Equals(postCheckerHistorySet.PostCheckDetailsId));

            //    if (postCheckDetailsSet == null)
            //    {
            //        continue;
            //    }

            //    postSet.LikesCount = postCheckDetailsSet.LikesCount;
            //    postSet.CommentsCount = postCheckDetailsSet.CommentsCount;
            //}
        }

        private PostContent ConvertPostContent(IList<PostContentSet> sets)
        {
            IReadOnlyList<PostContent> postContents = sets.Select(set =>
                    _mapper.Map(set, typeof(PostContentSet),
                        Post.MappingTypes[EnumExtensions.ToEnum<PostContentType>(set.ContentType)]) as PostContent)
                .ToList();

            PostCarouselContent carousel =
                postContents.FirstOrDefault(content => content.ContentType == PostContentType.Carousel) as
                    PostCarouselContent;

            if (carousel != null)
            {
                carousel.Slides = postContents.Where(p => p != carousel).OrderBy(content => content.Order).ToList();
                return carousel;
            }

            return postContents.FirstOrDefault();
        }
    }
}
