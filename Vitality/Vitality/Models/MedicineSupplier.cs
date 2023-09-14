using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class MedicineSupplier
    {
        public MedicineSupplier()
        {
            Medicines = new HashSet<Medicine>();
        }

        public int SuppliersId { get; set; }
        public string SuppliersName { get; set; } = null!;
        public long SuppliersPhoneNo { get; set; }
        public string SuppliersCity { get; set; } = null!;

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
