using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WeddingForward.Data.Models
{
    [Table("ApplicationLogsHistory")]
    public class LogsSet
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string AccountId { get; set; }

        public string PostId { get; set; }

        public string Error { get; set; }

        public DateTime DateTime { get; set; }
    }
}
