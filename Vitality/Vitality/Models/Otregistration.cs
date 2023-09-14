using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Otregistration
    {
        public int PatientsOtid { get; set; }
        public int PatientsCardId { get; set; }
        public int Ottime { get; set; }
        public string Otdate { get; set; } = null!;
        public int DoctorId { get; set; }
        public int? Status { get; set; }

        public virtual DoctorsRegistration Doctor { get; set; } = null!;
        public virtual OttimeSlot OttimeNavigation { get; set; } = null!;
        public virtual PatientsIdcard PatientsCard { get; set; } = null!;
    }
}
