using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class DoctorSlotDay
    {
        public int DoctorSlotDaysId { get; set; }
        public string DoctorSlotDays { get; set; } = null!;
        public int DoctorsId { get; set; }

        public virtual DoctorsRegistration Doctors { get; set; } = null!;
    }
}
