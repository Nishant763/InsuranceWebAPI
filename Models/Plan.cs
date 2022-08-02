using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Plan
    {
        public Plan()
        {
            Policies = new HashSet<Policy>();
        }

        //[Required(ErrorMessage = "Id is compulsory...")]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Type is compulsory...")]
        public string Type { get; set; }

        //[Required(ErrorMessage = "Amount is compulsory...")]
        public decimal Amount { get; set; }

        //[Required(ErrorMessage = "Typeofvehicle is compulsory...")]
        public string Typeofvehicle { get; set; }

        //[Required(ErrorMessage = "Term is compulsory...")]
        public int Term { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}
