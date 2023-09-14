using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class OttimeSlot
    {
        public OttimeSlot()
        {
            Otregistrations = new HashSet<Otregistration>();
        }

        public int OttimeId { get; set; }
        public string Ottime { get; set; } = null!;

        public virtual ICollection<Otregistration> Otregistrations { get; set; }
    }
}
