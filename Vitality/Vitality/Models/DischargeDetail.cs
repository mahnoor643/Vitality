using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class DischargeDetail
    {
        public int DischargeId { get; set; }
        public int DischargePatientsCardId { get; set; }
        public string DischargeFrom { get; set; } = null!;

        public virtual PatientsIdcard DischargePatientsCard { get; set; } = null!;
    }
}
