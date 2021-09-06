using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WeddingForward.ApplicationServices.Automation.AccountSession;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;
using WeddingForward.ApplicationServices.Automation.Execution;
using WeddingForward.ApplicationServices.Extensions;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.PythonAPI;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Posts.Queries.Handlers
{
    internal class PostQueryHandler: IDataRequestHandler<PostQuery, Post>
    {
        private readonly IMapper _mapper;

        private readonly IScriptRunner _scriptRunner;

        private readonly IAccountsSessionManager _sessionManager;

        public PostQueryHandler(IMapper mapper, IScriptRunner scriptRunner, IAccountsSessionManager sessionManager)
        {
            _mapper = mapper;
            _scriptRunner = scriptRunner;
            _sessionManager = sessionManager;
        }

        public async Task<Post> ExecuteAsync(PostQuery request)
        {
            ServiceResult<AccountSession> availableSession =
                await _sessionManager.GetAvailableSessionAsync()
                    .ConfigureAwait(false);

            if (availableSession.ResultType == ServiceResultType.Error)
            {
                throw availableSession.Exception ?? new Exception(availableSession.ErrorMessage);
            }

            AccountSession session = availableSession.Result;

            ServiceResult<PostInfoDto> postInfoResult = await _scriptRunner.RunAsync(Scripts.PostInfo, new[]
            {
                //request.AuthData.ToString(),
                request.PostId
            }, session).ConfigureAwait(false);

            if (postInfoResult.ResultType == ServiceResultType.Error)
            {
                return null;
            }

            PostInfoDto postInfoDto = postInfoResult.Result;

            PostContent content = CreateContentFromDto(postInfoDto);
            if (content == null)
            {
                return null;
            }

            var post = new Post
            {
                PostId = request.PostId,
                Content = content,
                CommentsCount = postInfoDto.CommentsCount,
                OwnerUsername = postInfoDto.OwnerUsername,
                LastUpdate = DateTime.UtcNow,
                LikesCount = postInfoDto.LikesCount,
                PostLink = $"https://www.instagram.com/p/{request.PostId}/",
                CreatedDateTime = DateTimeHelpers.UnixTimeStampToDateTime(postInfoDto.CreatedTimeStamp)
            };

            return post;
        }

        private PostContent CreateContentFromDto(PostInfoDto dto)
        {
            PostContent content = (PostContent) _mapper.Map(dto, typeof(PostInfoDto), Post.MappingTypes[dto.Type]);

            if (dto.Type == PostContentType.Carousel && dto.Children?.Count > 0)
            {
                PostCarouselContent carouselContent = content as PostCarouselContent;
                if (carouselContent != null)
                {
                    int order = 1;
                    
                    var contents = new List<PostContent>();

                    foreach (PostInfoDto child in dto.Children)
                    {
                        PostContent postContent =
                            _mapper.Map(child, typeof(PostInfoDto), Post.MappingTypes[child.Type]) as PostContent;

                        postContent.Order = order++;

                        contents.Add(postContent);
                    }

                    carouselContent.Slides = contents;
                }
            }

            return content;
        }
    }
}
