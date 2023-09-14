using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class DoctorsAppointment
    {
        public int DoctorAppointmentId { get; set; }
        public DateTime DoctorAppointmentDate { get; set; }
        public int DoctorAppointmentTime { get; set; }
        public int DoctorAppointment { get; set; }
        public string Status { get; set; } = null!;
        public int PatientsId { get; set; }

        public virtual DoctorsRegistration DoctorAppointmentNavigation { get; set; } = null!;
        public virtual DoctorSlotTime DoctorAppointmentTimeNavigation { get; set; } = null!;
        public virtual PatientsRegistration Patients { get; set; } = null!;
    }
}
