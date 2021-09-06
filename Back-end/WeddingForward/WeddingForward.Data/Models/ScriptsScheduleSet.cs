using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingForward.Data.Models
{
    public class ScriptsScheduleSet
    {
        [Key]
        public Guid Id { get; set; }

        public string SessionId { get; set; }

        public string ScriptType { get; set; }

        public string Args { get; set; }

        public DateTime PlanedStart { get; set; }

        public bool IsStarted { get; set; }

        public bool IsFinished { get; set; }
    }
}
