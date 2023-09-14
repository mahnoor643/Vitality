using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Bed
    {
        public Bed()
        {
            BedAllotments = new HashSet<BedAllotment>();
        }

        public int BedId { get; set; }
        public int Status { get; set; }
        public string Bed1 { get; set; } = null!;
        public int BedAmount { get; set; }

        public virtual ICollection<BedAllotment> BedAllotments { get; set; }
    }
}
