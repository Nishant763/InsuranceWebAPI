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
       // [Key]
        public int Id;
        public string Model;
        public string RegistrationNumber;
         public decimal RenewAmount;
         public string Email;
    }
}
