﻿using System;
using System.Collections.Generic;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Policy
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? PlansId { get; set; }
        public int? ClaimId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string RegistrationNumber { get; set; }
        public decimal RenewAmount { get; set; }

        public virtual Claim Claim { get; set; }
        public virtual Plan Plans { get; set; }
        public virtual Vehicle RegistrationNumberNavigation { get; set; }
        public virtual Customer User { get; set; }
    }
}
