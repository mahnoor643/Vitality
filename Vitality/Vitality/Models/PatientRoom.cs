using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class PatientRoom
    {
        public PatientRoom()
        {
            PatientsAllotedRooms = new HashSet<PatientsAllotedRoom>();
        }

        public int RoomId { get; set; }
        public string Room { get; set; } = null!;
        public int Status { get; set; }
        public int? RoomAmount { get; set; }

        public virtual ICollection<PatientsAllotedRoom> PatientsAllotedRooms { get; set; }
    }
}
