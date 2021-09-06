using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;
using WeddingForward.ApplicationServices.Posts.Models;

namespace WeddingForward.ApplicationServices.PythonAPI
{
    internal class Scripts
    {
        public static readonly PythonScript<AccountSearchResultDto[]> SearchForAccount =
            new PythonScript<AccountSearchResultDto[]>
            {
                Path = "Scripts/search-for-user.exe"
            };

        public static readonly PythonScript<AccountSession> Auth =
            new PythonScript<AccountSession>
            {
                Path = "Scripts/auth.exe"
            };

        public static readonly PythonScript<Account> AccountInfo =
            new PythonScript<Account>
            {
                Path = "Scripts/account-info.exe"
            };

        public static readonly PythonScript<UserPost[]> UserPosts =
            new PythonScript<UserPost[]>
            {
                Path = "Scripts/user-posts.exe"
            };

        public static readonly PythonScript<PostInfoDto> PostInfo =
            new PythonScript<PostInfoDto>
            {
                Path = "Scripts/post-info.exe"
            };

        public static readonly PythonScript<CheckAccountResult> CheckAuth =
            new PythonScript<CheckAccountResult>
            {
                Path = "Scripts/check-auth.exe"
            };
    }
}
