using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices.Extensions;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Posts.Queries.DataBase.Handlers
{
    internal class FetchPostQueryHandler: DataBaseRequestHandler<FetchPostQuery, Post>
    {
        public FetchPostQueryHandler(IMapper mapper, WeddingForwardContext context)
            : base(mapper, context)
        {
        }

        public override async Task<Post> ExecuteAsync(FetchPostQuery request)
        {
            if (String.IsNullOrEmpty(request.PostId))
            {
                throw new ArgumentNullException(nameof(request.PostId));
            }

            PostSet postSet = await Context.Posts
                .Include(set => set.PostContents)
                .ThenInclude(set => set.Tagged)
                .FirstOrDefaultAsync(set => set.PostId.Equals(request.PostId))
                .ConfigureAwait(false);

            Post post = Mapper.Map<Post>(postSet);
            if (post == null)
            {
                return null;
            }

            post.Content = ConvertPostContent(postSet.PostContents);

            return post;
        }

        private PostContent ConvertPostContent(IList<PostContentSet> sets)
        {
            IReadOnlyList<PostContent> postContents = sets.Select(set =>
                Mapper.Map(set, typeof(PostContentSet),
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
