using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WeddingForward.Data.Models
{
    [Table("PostCheckDetailsSet")]
    public class PostCheckDetailsSet
    {
        [Key]
        public string Id { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}
