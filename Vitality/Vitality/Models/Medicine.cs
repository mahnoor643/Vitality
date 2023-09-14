using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Medicine
    {
        public Medicine()
        {
            MedicineOrderDetails = new HashSet<MedicineOrderDetail>();
        }

        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = null!;
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }

        public virtual MedicineCategory Category { get; set; } = null!;
        public virtual MedicineSupplier Supplier { get; set; } = null!;
        public virtual ICollection<MedicineOrderDetail> MedicineOrderDetails { get; set; }
    }
}
