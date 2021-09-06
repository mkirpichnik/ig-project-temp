using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingForward.Data.Models
{
    [Table("Posts")]
    public class PostSet
    {
        [Key]
        public string PostId { get; set; }

        public string PostLink { get; set; }

        public string PostType { get; set; }

        public string OwnerUsername { get; set; }

        [ForeignKey(nameof(OwnerUsername))]
        public AccountSet Owner { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime LastUpdate { get; set; }

        public List<PostContentSet> PostContents { get; set; } = new List<PostContentSet>();
    }
}
