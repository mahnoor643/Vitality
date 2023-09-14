using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class HospitalPharmacyPrescription
    {
        public int PrescriptionId { get; set; }
        public int PatientsId { get; set; }
        public string Medicines { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual PatientsRegistration Patients { get; set; } = null!;
    }
}
