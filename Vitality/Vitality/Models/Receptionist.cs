using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Receptionist
    {
        public int ReceptionistId { get; set; }
        public string ReceptionistName { get; set; } = null!;
        public long ReceptionistContactNo { get; set; }
        public string ReceptionistEmail { get; set; } = null!;
        public string ReceptionistPwd { get; set; } = null!;
    }
}
