using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingForward.Data.Models
{
    [Table("UserAccounts")]
    public class AccountSet
    {
        [Key]
        public string Username { get; set; }

        public string FullName { get; set; }

        public int Follow { get; set; }

        public int Following { get; set; }

        public int PostsCount { get; set; }

        public bool IsPrivate { get; set; }

        public string ProfilePicUrl { get; set; }

        public string ProfilePicUrlHD { get; set; }

        public DateTime LastUpdate { get; set; }

        public List<PostSet> Posts { get; set; } = new List<PostSet>();
    }
}
