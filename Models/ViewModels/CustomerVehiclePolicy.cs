using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebAPI.ViewModels
{
    //get,

    public class CustomerVehiclePolicy
    {
        //policy_no,v_model,r_no,amount,email
        [Key]
        public int PolicyNo  {get;set;}
        public string Model { get; set; }
        public string RegistrationNumber
        {
            get; set;
         }
         public decimal RenewAmount
        {
            get; set;
        }
         public string Email
        {
            get; set;
         }
    }
}
