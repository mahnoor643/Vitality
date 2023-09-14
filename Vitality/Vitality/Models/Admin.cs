using System;
using System.Collections.Generic;

namespace Vitality.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public string AdminUsername { get; set; } = null!;
        public string AdminPwd { get; set; } = null!;
        public string AdminName { get; set; } = null!;
    }
}
