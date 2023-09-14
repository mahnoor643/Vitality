using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Staff
    {
        public Staff()
        {
            StaffSalaries = new HashSet<StaffSalary>();
        }

        public int StaffId { get; set; }
        public string StaffName { get; set; } = null!;
        public string StaffDesignation { get; set; } = null!;

        public virtual ICollection<StaffSalary> StaffSalaries { get; set; }
    }
}
