using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class LabTestAllocation
    {
        public int LabTestAllocationId { get; set; }
        public int LabTestId { get; set; }
        public int PatientsCardId { get; set; }
        public string CurrentDateTime { get; set; } = null!;
        public int Status { get; set; }

        public virtual LabTest LabTest { get; set; } = null!;
        public virtual PatientsIdcard PatientsCard { get; set; } = null!;
    }
}
