using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class BedAllotment
    {
        public int BedAllotmentId { get; set; }
        public int BedsId { get; set; }
        public string CurrentDateTime { get; set; } = null!;
        public int Days { get; set; }
        public int? PatientsCardNo { get; set; }
        public DateTime AllotTill { get; set; }
        public int Status { get; set; }

        public virtual Bed Beds { get; set; } = null!;
        public virtual PatientsIdcard? PatientsCardNoNavigation { get; set; }
    }
}
