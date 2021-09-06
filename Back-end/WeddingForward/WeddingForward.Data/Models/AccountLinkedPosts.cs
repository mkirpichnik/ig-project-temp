using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingForward.Data.Models
{
    public class AccountLinkedPosts
    {
        [Key]
        public Guid Id { get; set; }

        public string OwnerUsername { get; set; }

        [ForeignKey(nameof(OwnerUsername))]
        public AccountSet Owner { get; set; }

        public string PostId { get; set; }

        public string PostLink { get; set; }

        public int Order { get; set; }
    }
}
