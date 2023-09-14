using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class PharmacyEmployee
    {
        public PharmacyEmployee()
        {
            MedicineOrders = new HashSet<MedicineOrder>();
        }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string EmployeeEmail { get; set; } = null!;
        public long EmployeePhoneNo { get; set; }
        public string EmployeeImg { get; set; } = null!;
        public int Status { get; set; }

        public virtual ICollection<MedicineOrder> MedicineOrders { get; set; }
    }
}
