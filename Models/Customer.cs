using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InsuranceWebAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Policies = new HashSet<Policy>();
        }
        
        //[Required(ErrorMessage = "Id is compulsory...")]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Name is compulsory...")]
        public string Name { get; set; }

        //[DataType(DataType.EmailAddress)]
        //[Remote("CheckEmail", "Emp", ErrorMessage = "Duplicate Email")]
        //[Required(ErrorMessage = "Email is compulsory...")]
        public string Email { get; set; }

       // [Required(ErrorMessage = "ContactNumber is compulsory...")]
        public string ContactNumber { get; set; }

        //[Required(ErrorMessage = "Address is compulsory...")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Dob is compulsory...")]
        public DateTime? DateOfBirth { get; set; }

        //[Required(ErrorMessage = "Id is compulsory...")]
        public string Password { get; set; }


        public virtual ICollection<Policy> Policies { get; set; }
    }
}
