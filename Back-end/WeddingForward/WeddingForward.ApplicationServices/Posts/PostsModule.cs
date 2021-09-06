using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WeddingForward.ApplicationServices.Posts.Commands;
using WeddingForward.ApplicationServices.Posts.Commands.Handlers;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.Posts.Queries;
using WeddingForward.ApplicationServices.Posts.Queries.DataBase;
using WeddingForward.ApplicationServices.Posts.Queries.DataBase.Handlers;
using WeddingForward.ApplicationServices.Posts.Queries.Handlers;

namespace WeddingForward.ApplicationServices.Posts
{
    internal static class PostsModule
    {
        public static IServiceCollection UsePostsModule(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<Profile, PostMapping>()
                .AddSingleton<Profile, PostContentMapping>()
                .AddTransient<Profile, PostContentMappingCongiguration>()
                .AddTransient<Profile, UserPostMapping>()
                .AddTransient<IPostsService, PostsService>()
                .AddTransient<IDataRequestHandler<PostQuery, Post>, PostQueryHandler>()
                .AddTransient<IDataRequestHandler<StorePostCommand, bool>, StorePostCommandHandler>()
                .AddTransient<IDataRequestHandler<StoreAccountPostsCommand, bool>, StoreAccountPostsCommandHandler>()
                .AddTransient<IDataRequestHandler<FetchAccountPostsQuery, IReadOnlyList<UserPost>>, FetchAccountPostsQueryHandler>()
                .AddTransient<IDataRequestHandler<FetchPostQuery, Post>, FetchPostQueryHandler>()
                .AddTransient<IDataRequestHandler<UserPostsQuery, IReadOnlyList<UserPost>>, UserPostsQueryHandler>();
        }
    }
}
