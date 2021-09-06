using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Posts.Commands.Handlers
{
    internal class StorePostCommandHandler: DataBaseRequestHandler<StorePostCommand, bool>
    {
        public StorePostCommandHandler(IMapper mapper, WeddingForwardContext context)
            : base(mapper, context)
        {
        }

        public override async Task<bool> ExecuteAsync(StorePostCommand request)
        {
            Post post = request.Post;
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            PostSet postSet = Mapper.Map<PostSet>(post);

            bool exists = await Context.Posts.AnyAsync(set => set.PostId.Equals(postSet.PostId))
                .ConfigureAwait(false);

            if (!exists)
            {
                IReadOnlyList<PostContentSet> contentSets = GetPostContentSets(post);

                IReadOnlyList<TaggedReferencesSet> taggedReferencesSets = GetTaggedReferences(post.Content);

                await Context.Posts.AddAsync(postSet).ConfigureAwait(false);

                await Context.PostsContent.AddRangeAsync(contentSets).ConfigureAwait(false);

                await Context.TaggedInPost.AddRangeAsync(taggedReferencesSets).ConfigureAwait(false);
            }
            else
            {
                Context.Posts.Update(postSet);
            }

            int changesAsync = await Context.SaveChangesAsync().ConfigureAwait(false);
            return changesAsync > 0;
        }

        private IReadOnlyList<PostContentSet> GetPostContentSets(Post post)
        {
            PostContent content = post.Content;
            if (content.ContentType != PostContentType.Carousel)
            {
                PostContentSet postContentSet = Mapper.Map<PostContentSet>(content);
                postContentSet.PostId = post.PostId;

                return new []
                {
                    postContentSet    
                };
            }

            PostCarouselContent carouselContent = content as PostCarouselContent;

            IEnumerable<PostContent> contents = carouselContent.Slides.Union(new []
            {
                content
            });
            
            var postContents = new List<PostContentSet>();

            foreach (PostContent carouselContentSlide in contents)
            {
                PostContentSet postContentSet = Mapper.Map<PostContentSet>(carouselContentSlide);
                postContentSet.PostId = post.PostId;
                postContents.Add(postContentSet);
            }

            return postContents;
        }

        private IReadOnlyList<TaggedReferencesSet> GetTaggedReferences(PostContent content)
        {
            if (content.ContentType != PostContentType.Carousel)
            {
                return content.Tagged.Select(s => new TaggedReferencesSet
                {
                    Id = Guid.NewGuid(),
                    PostContentId = content.Id,
                    Username = s
                }).ToList();
            }

            PostCarouselContent postCarouselContent = content as PostCarouselContent;

            var referencesSets = new List<TaggedReferencesSet>();

            foreach (PostContent postContent in postCarouselContent.Slides)
            {
                referencesSets.AddRange(postContent.Tagged.Select(s => new TaggedReferencesSet
                {
                    Id = Guid.NewGuid(),
                    PostContentId = postContent.Id,
                    Username = s
                }));
            }

            return referencesSets;
        }
    }
}
