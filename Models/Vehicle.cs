using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Policies = new HashSet<Policy>();
        }

       // [Required(ErrorMessage = "ManufacturerName is compulsory...")]
        public string ManufacturerName { get; set; }

        //[Required(ErrorMessage = "Model is compulsory...")]
        public string Model { get; set; }

        //[Required(ErrorMessage = "License is compulsory...")]
        public string License { get; set; }

        //[Required(ErrorMessage = "PurchaseDate is compulsory...")]
        public DateTime PurchaseDate { get; set; }

        //[Required(ErrorMessage = "RegistrationNumber is compulsory...")]
        public string RegistrationNumber { get; set; }

        //[Required(ErrorMessage = "EngineNumber is compulsory...")]
        public string EngineNumber { get; set; }

        //[Required(ErrorMessage = "ChassisNumber is compulsory...")]
        public string ChassisNumber { get; set; }

        //[Required(ErrorMessage = "TypeOfVehicle is compulsory...")]
        public string TypeOfVehicle { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}
