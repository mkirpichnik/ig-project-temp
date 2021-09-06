using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingForward.Data.Models
{
    public class AccountsSessionSet
    {
        [Key]
        public string SessionId { get; set; }

        public string Url { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string AccountId { get; set; }

        public bool IsAlive { get; set; }
    }
}
