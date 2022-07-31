using System;
using System.Collections.Generic;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Policies = new HashSet<Policy>();
        }

        public string ManufacturerName { get; set; }
        public string Model { get; set; }
        public string License { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string TypeOfVehicle { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}
