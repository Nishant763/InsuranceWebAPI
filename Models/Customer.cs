using System;
using System.Collections.Generic;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Typeofvehicle { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}
