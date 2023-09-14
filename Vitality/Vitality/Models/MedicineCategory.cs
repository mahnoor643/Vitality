using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class MedicineCategory
    {
        public MedicineCategory()
        {
            Medicines = new HashSet<Medicine>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
