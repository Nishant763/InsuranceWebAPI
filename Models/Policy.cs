using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Policy
    {
        public Policy()
        {
            Claims = new HashSet<Claim>();
        }

        //[Required(ErrorMessage = "Id is compulsory...")]
        public int Id { get; set; }

        //[Required(ErrorMessage = "UserId is compulsory...")]
        public int UserId { get; set; }

        //[Required(ErrorMessage = "PlansId is compulsory...")]
        public int? PlansId { get; set; }

        //[Required(ErrorMessage = "PurchaseDate is compulsory...")]
        public DateTime PurchaseDate { get; set; }

        //[Required(ErrorMessage = "RegistrationNumber is compulsory...")]
        public string RegistrationNumber { get; set; }

        //[Required(ErrorMessage = "RenewAmount is compulsory...")]
        public decimal RenewAmount { get; set; }

        public virtual Plan Plans { get; set; }
        public virtual Vehicle RegistrationNumberNavigation { get; set; }
        public virtual Customer User { get; set; }
        public virtual ICollection<Claim> Claims { get; set; }
    }
}
