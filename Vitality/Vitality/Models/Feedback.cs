using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string Feedback1 { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public int Status { get; set; }
    }
}
