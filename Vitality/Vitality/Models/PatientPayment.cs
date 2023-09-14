using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class PatientPayment
    {
        public int PayId { get; set; }
        public int Pay { get; set; }
        public int PatientsCardId { get; set; }
        public int Status { get; set; }

        public virtual PatientsIdcard PatientsCard { get; set; } = null!;
    }
}
