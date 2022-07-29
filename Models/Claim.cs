using System;
using System.Collections.Generic;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Claim
    {
        public Claim()
        {
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public DateTime ClaimDate { get; set; }
        public bool? Isapproved { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}
