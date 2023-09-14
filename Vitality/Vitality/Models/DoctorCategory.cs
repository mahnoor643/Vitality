using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class DoctorCategory
    {
        public DoctorCategory()
        {
            DoctorsRegistrations = new HashSet<DoctorsRegistration>();
        }

        public int DoctorCategoryId { get; set; }
        public string DoctorCategory1 { get; set; } = null!;

        public virtual ICollection<DoctorsRegistration> DoctorsRegistrations { get; set; }
    }
}
