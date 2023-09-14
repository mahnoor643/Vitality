using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class PatientsRegistration
    {
        public PatientsRegistration()
        {
            DoctorsAppointments = new HashSet<DoctorsAppointment>();
            HospitalPharmacyPrescriptions = new HashSet<HospitalPharmacyPrescription>();
            PatientsIdcards = new HashSet<PatientsIdcard>();
        }

        public int PatientsId { get; set; }
        public string PatientsName { get; set; } = null!;
        public string PatientsEmail { get; set; } = null!;
        public long PatientsPhoneNo { get; set; }
        public string? PatientsPwd { get; set; }

        public virtual ICollection<DoctorsAppointment> DoctorsAppointments { get; set; }
        public virtual ICollection<HospitalPharmacyPrescription> HospitalPharmacyPrescriptions { get; set; }
        public virtual ICollection<PatientsIdcard> PatientsIdcards { get; set; }
    }
}
