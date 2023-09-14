using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class AvailedBlood
    {
        public int BloodRecieverId { get; set; }
        public int? BloodDonorsId { get; set; }
        public int PatientsCardId { get; set; }
        public int? BloodStatus { get; set; }

        public virtual BloodDonor? BloodDonors { get; set; }
        public virtual PatientsIdcard PatientsCard { get; set; } = null!;
    }
}
