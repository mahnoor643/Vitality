using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Degree
    {
        public Degree()
        {
            DoctorsRegistrations = new HashSet<DoctorsRegistration>();
            NurseRegistrations = new HashSet<NurseRegistration>();
        }

        public int DegreeId { get; set; }
        public string Degree1 { get; set; } = null!;

        public virtual ICollection<DoctorsRegistration> DoctorsRegistrations { get; set; }
        public virtual ICollection<NurseRegistration> NurseRegistrations { get; set; }
    }
}
