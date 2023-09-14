using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class LabTest
    {
        public LabTest()
        {
            LabTestAllocations = new HashSet<LabTestAllocation>();
        }

        public int LabTestId { get; set; }
        public string LabTest1 { get; set; } = null!;
        public int? LabTestAmount { get; set; }

        public virtual ICollection<LabTestAllocation> LabTestAllocations { get; set; }
    }
}
