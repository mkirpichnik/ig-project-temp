using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingForward.Data.Models
{
    [Table("PostCheckerHistory")]
    public class PostCheckerHistorySet
    {
        [Key]
        public Guid Id { get; set; }

        public string AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public AccountSet Account { get; set; }

        public string PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public PostSet Post { get; set; }

        public string PostCheckDetailsId { get; set; }

        [ForeignKey(nameof(PostCheckDetailsId))]
        public PostCheckDetailsSet PostCheckDetails { get; set; }

        public DateTime CheckedAt { get; set; }
    }
}
