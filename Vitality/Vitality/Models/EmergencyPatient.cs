using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class EmergencyPatient
    {
        public int EmergencyPatientsId { get; set; }
        public string EmergencyPatientsName { get; set; } = null!;
        public int AmountPayable { get; set; }
        public int AmountPaid { get; set; }
        public int Status { get; set; }
    }
}
