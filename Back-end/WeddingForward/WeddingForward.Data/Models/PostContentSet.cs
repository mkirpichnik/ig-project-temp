using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingForward.Data.Models
{
    [Table("PostContent")]
    public class PostContentSet
    {
        [Key]
        public string Id { get; set; }

        public string ContentUrl { get; set; }

        public string ContentType { get; set; }

        public int ViewsCount { get; set; }

        public int Order { get; set; }

        public string PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public PostSet Post { get; set; }

        public List<TaggedReferencesSet> Tagged { get; set; } = new List<TaggedReferencesSet>();
    }
}
