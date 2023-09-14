using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class MedicineOrder
    {
        public MedicineOrder()
        {
            MedicineOrderDetails = new HashSet<MedicineOrderDetail>();
        }

        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public string OrderDate { get; set; } = null!;

        public virtual PharmacyEmployee Employee { get; set; } = null!;
        public virtual ICollection<MedicineOrderDetail> MedicineOrderDetails { get; set; }
    }
}
