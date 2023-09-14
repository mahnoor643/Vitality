using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class StaffSalaryStatus
    {
        public int SalaryToPayId { get; set; }
        public int SalaryToPay { get; set; }
        public int StaffSalariesId { get; set; }
        public int Status { get; set; }

        public virtual StaffSalary StaffSalaries { get; set; } = null!;
    }
}
