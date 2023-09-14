using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class PatientsAllotedRoom
    {
        public int PatientsAllotedRoomId { get; set; }
        public int PatientsRoomId { get; set; }
        public string CurrentDateTime { get; set; } = null!;
        public int Days { get; set; }
        public int PatientsCardId { get; set; }
        public int? Status { get; set; }
        public DateTime AllotTill { get; set; }

        public virtual PatientsIdcard PatientsCard { get; set; } = null!;
        public virtual PatientRoom PatientsRoom { get; set; } = null!;
    }
}
