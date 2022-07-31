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
        public int Id { get; set; }

        public DateTime claim_date { get; set; }
        public bool? isapproved { get; set; }
        public decimal renew_amount{ get; set; }
        public string email{ get; set; }
    }
}
