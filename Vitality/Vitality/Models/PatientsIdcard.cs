using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class PatientsIdcard
    {
        public PatientsIdcard()
        {
            AvailedBloods = new HashSet<AvailedBlood>();
            BedAllotments = new HashSet<BedAllotment>();
            DischargeDetails = new HashSet<DischargeDetail>();
            LabTestAllocations = new HashSet<LabTestAllocation>();
            Otregistrations = new HashSet<Otregistration>();
            PatientPayments = new HashSet<PatientPayment>();
            PatientsAllotedRooms = new HashSet<PatientsAllotedRoom>();
        }

        public int PatientsCardId { get; set; }
        public int PatientsId { get; set; }
        public int AdvancePayment { get; set; }
        public int? Status { get; set; }
        public DateTime ValidDate { get; set; }
        public int PayableAmount { get; set; }

        public virtual PatientsRegistration Patients { get; set; } = null!;
        public virtual ICollection<AvailedBlood> AvailedBloods { get; set; }
        public virtual ICollection<BedAllotment> BedAllotments { get; set; }
        public virtual ICollection<DischargeDetail> DischargeDetails { get; set; }
        public virtual ICollection<LabTestAllocation> LabTestAllocations { get; set; }
        public virtual ICollection<Otregistration> Otregistrations { get; set; }
        public virtual ICollection<PatientPayment> PatientPayments { get; set; }
        public virtual ICollection<PatientsAllotedRoom> PatientsAllotedRooms { get; set; }
    }
}
