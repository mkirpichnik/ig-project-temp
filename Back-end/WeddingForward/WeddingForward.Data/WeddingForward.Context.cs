using Microsoft.EntityFrameworkCore;
using WeddingForward.Data.Models;

namespace WeddingForward.Data
{
    public class WeddingForwardContext: DbContext
    {
        public WeddingForwardContext(DbContextOptions options)
            :base (options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<AccountSet> Accounts { get; set; }

        public DbSet<PostSet> Posts { get; set; }

        public DbSet<PostContentSet> PostsContent { get; set; }

        public DbSet<TaggedReferencesSet> TaggedInPost { get; set; }

        public DbSet<AccountsSessionSet> AccountsSession { get; set; }

        public DbSet<AccountLinkedPosts> AccountLinkedPosts { get; set; }

        public DbSet<PostCheckerHistorySet> PostCheckerHistory { get; set; }

        public DbSet<PostCheckDetailsSet> PostCheckDetails { get; set; }

        public DbSet<ScriptsScheduleSet> ScriptsSchedule { get; set; }

        public DbSet<LogsSet> Logs { get; set; }
    }
}
