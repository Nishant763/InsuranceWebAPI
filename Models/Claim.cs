using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Claim
    {
        
        //[Required(ErrorMessage ="Id is compulsory....")]
        public int Id { get; set; }

        //[Required(ErrorMessage ="ClaimDate is compulsory....")]
        public DateTime ClaimDate { get; set; }

        //[Required(ErrorMessage ="Isapproved is compulsory.....")]
        public bool? Isapproved { get; set; }

        //[Required(ErrorMessage ="Policy Id is compulsory.....")]
        public int? PolicyId { get; set; }

        public virtual Policy Policy { get; set; }
    }
}
