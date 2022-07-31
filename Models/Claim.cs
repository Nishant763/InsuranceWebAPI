using System;
using System.Collections.Generic;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Claim
    {
        public int Id { get; set; }
        public DateTime ClaimDate { get; set; }
        public bool? Isapproved { get; set; }
        public int? PolicyId { get; set; }

        public virtual Policy Policy { get; set; }
    }
}
