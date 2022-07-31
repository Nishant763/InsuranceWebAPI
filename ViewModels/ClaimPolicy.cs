using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebAPI.ViewModels
{
    //get
    public class ClaimPolicy
    {
        //Claim -> claim_id,date,approveornot
        //Policy -> amount

        [Key]
        public int Id;

        public DateTime ClaimDate;
        public bool? Isapproved;
        public decimal RenewAmount;
    }
}
