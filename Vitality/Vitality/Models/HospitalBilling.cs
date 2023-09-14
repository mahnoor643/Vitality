using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class HospitalBilling
    {
        public int HospitalBillings { get; set; }
        public int BillFor { get; set; }
        public string Status { get; set; } = null!;
    }
}
