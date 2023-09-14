using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class MedicineOrderDetail
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int MedicinesId { get; set; }
        public int Quantity { get; set; }

        public virtual Medicine Medicines { get; set; } = null!;
        public virtual MedicineOrder Order { get; set; } = null!;
    }
}
