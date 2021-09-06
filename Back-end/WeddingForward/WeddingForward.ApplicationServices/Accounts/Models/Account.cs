using System;

namespace WeddingForward.ApplicationServices.Accounts.Models
{
    public class Account
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public int Follow { get; set; }

        public int Following { get; set; }

        public int PostsCount { get; set; }

        public bool IsPrivate { get; set; }

        public string ProfilePicUrl { get; set; }

        public string ProfilePicUrlHD { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
