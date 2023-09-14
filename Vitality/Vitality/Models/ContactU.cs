using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class ContactU
    {
        public int ContactId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string ContactSubject { get; set; } = null!;
        public string ContactMsg { get; set; } = null!;
    }
}
