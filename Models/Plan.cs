using System;
using System.Collections.Generic;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Plan
    {
        public Plan()
        {
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Typeofvehicle { get; set; }
        public int Term { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}
