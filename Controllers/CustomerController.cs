using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceWebAPI.Models;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using InsuranceWebAPI.ViewModels;

namespace InsuranceWebAPI.Controllers
{
    /// <summary>
    /// CustomerController -> handle login user ,register user requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        insuranceContext db = new insuranceContext();

        /// <summary>
        /// Fetches List Of Customers
        /// </summary>
        /// <returns>List Of Customers</returns>
        [HttpGet]
        [Route("ListCustomers")]
        public IActionResult GetCustomers()
        {
            //var data = db.Departments.ToList();
            var data = from cust in db.Customers select cust;
            return Ok(data);

        }


        /// <summary>
        /// Registration Of Customer
        /// </summary>
        /// <param name="c">Customer Object</param>
        /// <returns>if registration is successful -> 201 created response with customer object else 400 Bad Request Response</returns>
        [HttpPost]
        [Route("Register")]
        public IActionResult AddCustomer(Customer c)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Customers.Add(c);
                    db.SaveChanges();
                    return Created($"Customer registered successfully.....",c);
                }
                return BadRequest($"Something went wrong while registeration...");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong while registeration...{ex.Message}");
            }
        }

        /// <summary>
        /// Login of customer based on password and email validation
        /// </summary>
        /// <param name="CustIdPass"></param>
        /// <returns>if successfull -> 200 Ok response else 400 Bad Request Response</returns>
        [HttpGet]
        [Route("Login/{email}/{password}")]
        public IActionResult LoginCustomer( string password,string email) 
        {

            //string password = CustIdPass.password;
            //find customer with that email
            //string email = Request.Query["value"];
            var cust = db.Customers.Where(o => o.Email == email).FirstOrDefault();

            if (cust == null)
            {
                return BadRequest($"No Customer found with given email id....");
            }

            //validate password
            if (cust.Password == password)
            {
                return Ok();
            }

            return BadRequest("Invalid Password....");
        }

        /// <summary>
        /// Get Customer with particular id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Customer Object if successful else 404 Not found response</returns>

        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult GetCustomer(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id can't be null...");
            }

            var data = from cust in db.Customers where cust.Id==id select cust;
            if (data.Count() == 0)
            {
                return StatusCode(404, $"Customer {id} not present");
            }
            return Ok(data);
            
        }

        /// <summary>
        /// Get Customer with particular id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Customer Object if successful else 404 Not found response</returns>

        [HttpGet]
        [Route("Get/{email}")]
        public IActionResult GetCustomer(string? email)
        {
            if (email == null)
            {
                return BadRequest("Id can't be null...");
            }

            var data = from cust in db.Customers where cust.Email == email select cust;
            if (data.Count() == 0)
            {
                return StatusCode(404, $"Customer {email} not present");
            }
            return Ok(data);

        }

        /// <summary>
        /// Buy Insurance Post Method based on user_id
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>200Ok response with vehicle_registration_number for further processing</returns>
        [HttpPost]
        [Route("BuyInsurance/{email}")]
        public IActionResult PostVehicle(Vehicle vehicle)
        {
            try
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         

        }


        /// <summary>
        /// Gets Policy  based on plan and ve
        /// </summary>
        /// <param name="plan1"></param>
        /// <param name="reg_no"></param>
        /// <returns>Policy with 200 response</returns>
        [HttpGet]
        [Route("BuyInsurance/{reg_no}/{type}/{duration}")]

        public IActionResult GetPlan(string reg_no, string type, int duration)
        {

           // planwithoutpolicies plan1 = new planwithoutpolicies();
            Plan resPlan = new Plan();

            try
            {
                var vehicle = db.Vehicles.Where(v => v.RegistrationNumber == reg_no).FirstOrDefault();
                if (vehicle != null)
                {
                    string tp = vehicle.TypeOfVehicle;
                    resPlan = db.Plans.Where(p => (p.Term == duration) &&
                    (p.Type == type) && (p.Typeofvehicle == tp)).FirstOrDefault();
                }
                else
                {
                    return BadRequest("vehicle not found");
                }
                //    pid = from p in db.Plans
                //          where (p.Duration == plan1.Duration) && (p.Type == plan1.Type)
                //&& p.Id == 6  select p.Id;

                if (resPlan == null) return BadRequest("plan not found");


                return Ok(resPlan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           // plan1.Amount = resPlan.Amount;
           // plan1.Duration = resPlan.Duration;
            //plan1.Id = resPlan.Id;
            //plan1.Typeofvehicle = resPlan.Typeofvehicle;
            //plan1.Type = resPlan.Type;
           
        }


        [HttpPost]
        [Route("BuyInsurance/addpolicy/{email}/{reg_no}")]
        public IActionResult PostPolicy(Plan plan, string email, string reg_no)
        {
            try
            {

                Customer customer = new Customer();

                Policy policy = new Policy();
                customer = db.Customers.Where(e => e.Email == email).FirstOrDefault();
                policy.UserId = customer.Id;
                policy.RegistrationNumber = reg_no;
                policy.PlansId = plan.Id;
                policy.PurchaseDate = DateTime.Now;
                policy.RenewAmount = plan.Amount;
                

                db.Policies.Add(policy);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }


        //customer edit based on emailid
        [HttpPut]
        [Route("Edit/{emailid}")]
        public IActionResult editCustomer(string? emailid, Customer c)
        {
            if (emailid == null)
            {
                return BadRequest("Email id is null..");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Customer actual = (from cust in db.Customers where cust.Email == emailid select cust).FirstOrDefault();
                    actual.Email = c.Email;
                    actual.Address = c.Address;
                    actual.ContactNumber = c.ContactNumber;
                    actual.DateOfBirth = c.DateOfBirth;
                    actual.Name = c.Name;
                    actual.Password = c.Password;
                    actual.Policies = c.Policies;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Exception....{ex.InnerException.Message}");
                }
            }

            return BadRequest("Something went wrong.....");

        }



        //CLaimPolicyModel

         [HttpGet]
         [Route("GetClaimPolicy")]

         public IActionResult getClaimPolicy()
         {
            //var data =  db.EmpDepts.FromSqlInterpolated<EmpDepartment>($"ShowEmp");

            var data = db.ClaimPolicies.FromSqlInterpolated<ClaimPolicy>($"ShowClaimPolicyNotApproved");




             return Ok(data);

         }


            //CustomerVehiclePolicy
            //To Do -> Stored procedure created but backend issue
        [HttpGet]
        [Route("GetCustomerVehiclePolicy/{email}")]
        public IActionResult getCustomerVehiclePolicy(string email)
        {
            //CustomerVehiclePolicy d = new CustomerVehiclePolicy();
            var data = db.CustomerVehiclePolicies.FromSqlInterpolated<CustomerVehiclePolicy>($"ShowCustomerVehiclePolicy {email}");




            return Ok(data);

        }




    }
}
