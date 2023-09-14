using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class BloodDonor
    {
        public BloodDonor()
        {
            AvailedBloods = new HashSet<AvailedBlood>();
        }

        public int BloodDonorsId { get; set; }
        public int BloodGroup { get; set; }
        public string BloodDonorsName { get; set; } = null!;
        public string BloodDonorsEmail { get; set; } = null!;
        public long BloodDonorsPhoneNo { get; set; }
        public int Status { get; set; }

        public virtual BloodGroup BloodGroupNavigation { get; set; } = null!;
        public virtual ICollection<AvailedBlood> AvailedBloods { get; set; }
    }
}
