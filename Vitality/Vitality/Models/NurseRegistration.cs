using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class NurseRegistration
    {
        public int NurseRegistrationsId { get; set; }
        public string NurseName { get; set; } = null!;
        public string NurseEmail { get; set; } = null!;
        public long NursePhoneNo { get; set; }
        public int Degree { get; set; }
        public string NurseImg { get; set; } = null!;
        public int Status { get; set; }

        public virtual Degree DegreeNavigation { get; set; } = null!;
    }
}
