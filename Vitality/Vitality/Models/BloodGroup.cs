using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class BloodGroup
    {
        public BloodGroup()
        {
            BloodDonors = new HashSet<BloodDonor>();
        }

        public int BloodGroupId { get; set; }
        public string BloodGroup1 { get; set; } = null!;

        public virtual ICollection<BloodDonor> BloodDonors { get; set; }
    }
}
