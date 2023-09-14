using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class StaffSalary
    {
        public StaffSalary()
        {
            StaffSalaryStatuses = new HashSet<StaffSalaryStatus>();
        }

        public int StaffSalaryId { get; set; }
        public int SalaryAmount { get; set; }
        public int StaffId { get; set; }

        public virtual Staff Staff { get; set; } = null!;
        public virtual ICollection<StaffSalaryStatus> StaffSalaryStatuses { get; set; }
    }
}
