using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class DoctorsRegistration
    {
        public DoctorsRegistration()
        {
            DoctorSlotDays = new HashSet<DoctorSlotDay>();
            DoctorSlotTimes = new HashSet<DoctorSlotTime>();
            DoctorsAppointments = new HashSet<DoctorsAppointment>();
            Otregistrations = new HashSet<Otregistration>();
        }

        public int DoctorsId { get; set; }
        public string DoctorsName { get; set; } = null!;
        public int DoctorsDegree { get; set; }
        public int DoctorsCategory { get; set; }
        public long DoctorsPhoneNo { get; set; }
        public int Status { get; set; }
        public string DoctorsEmail { get; set; } = null!;
        public string? DoctorsImg { get; set; }
        public string? DoctorsPwd { get; set; }

        public virtual DoctorCategory DoctorsCategoryNavigation { get; set; } = null!;
        public virtual Degree DoctorsDegreeNavigation { get; set; } = null!;
        public virtual ICollection<DoctorSlotDay> DoctorSlotDays { get; set; }
        public virtual ICollection<DoctorSlotTime> DoctorSlotTimes { get; set; }
        public virtual ICollection<DoctorsAppointment> DoctorsAppointments { get; set; }
        public virtual ICollection<Otregistration> Otregistrations { get; set; }
    }
}
