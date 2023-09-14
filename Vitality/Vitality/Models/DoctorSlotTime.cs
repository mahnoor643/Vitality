using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class DoctorSlotTime
    {
        public DoctorSlotTime()
        {
            DoctorsAppointments = new HashSet<DoctorsAppointment>();
        }

        public int DoctorSlotTimeId { get; set; }
        public string DoctorSlotTime1 { get; set; } = null!;
        public int DoctorsId { get; set; }

        public virtual DoctorsRegistration Doctors { get; set; } = null!;
        public virtual ICollection<DoctorsAppointment> DoctorsAppointments { get; set; }
    }
}
