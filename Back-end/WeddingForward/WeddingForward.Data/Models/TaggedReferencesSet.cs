using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingForward.Data.Models
{
    [Table("Tagged")]
    public class TaggedReferencesSet
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string PostContentId { get; set; }

        [ForeignKey(nameof(PostContentId))]
        public PostContentSet PostContent { get; set; }
    }
}
