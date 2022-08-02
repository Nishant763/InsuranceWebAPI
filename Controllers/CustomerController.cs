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

      /*  [HttpGet]
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
            
        }*/

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
            if (vehicle == null) { return BadRequest("data not recieved"); }
            try
            {

                Vehicle vehicle1 = new Vehicle();

                vehicle1 = db.Vehicles.Where(v => v.RegistrationNumber == vehicle.RegistrationNumber).FirstOrDefault();

                if (vehicle1 != null)
                {
                    Policy policy = new Policy();
                    policy = db.Policies.Where(p => p.RegistrationNumber == vehicle1.RegistrationNumber).FirstOrDefault();
                    if (policy != null)
                    {
                        Plan plan = new Plan();
                        plan = db.Plans.Where(p => p.Id == policy.PlansId).FirstOrDefault();

                        if (( DateTime.Now - policy.PurchaseDate).Days > plan.Term * 365)
                        {
                            return BadRequest("Already we have policy with this Vehicle and Insurance Duration is Completed!!\n Please Renew it through RenewInsurance in Home");
                        }
                        else
                        {
                            return BadRequest("Hurray!!! Already we have policy with this Vehicle !! \n For Extension of duration you can renew through renew insurance in Home");
                        }
                    }
                    else
                    {
                        return Ok();
                    }
                }














                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
         

        }


        /// <summary>
        /// Gets Policy  based on plan and registration_number
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
                int days = (vehicle.PurchaseDate - DateTime.Now).Days;
                int year = Math.Abs(days) / 365;
                resPlan.Amount = amountAccordingToAge(resPlan.Amount, year);

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

        /// <summary>
        /// Adds policy in db based on email(user) and registration number(vehicle) 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="email"></param>
        /// <param name="reg_no"></param>
        /// <returns></returns>
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

                Vehicle vehicle = new Vehicle();
                vehicle = db.Vehicles.Find(reg_no);

                int days = (vehicle.PurchaseDate - DateTime.Now).Days;
                days = Math.Abs(days);
                int year = days / 365;
                policy.RenewAmount = amountAccordingToAge(plan.Amount, year);

                db.Policies.Add(policy);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
       
        }

        /// <summary>
        /// Edit Customer based on EmailId
        /// </summary>
        /// <param name="emailid"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        
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
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Exception....{ex.InnerException.Message}");
                }
            }

            return BadRequest("Something went wrong.....");

        }



        /// <summary>
        /// Fetches all the claim with policy details which are not approved by admin
        /// </summary>
        /// <returns>List of ClaimPolicy Objects</returns>
        

         [HttpGet]
         [Route("GetClaimPolicy")]

         public IActionResult getClaimPolicy()
         {
            

            var data = db.ClaimPolicies.FromSqlInterpolated<ClaimPolicy>($"ShowClaimPolicyNotApproved");




             return Ok(data);

         }


            
            /// <summary>
            /// Fetches all the customer,vehicle,policy details of a particular user based on emailid
            /// </summary>
            /// <param name="email"></param>
            /// <returns>CustomerVehiclePolicy Objects</returns>
        [HttpGet]
        [Route("GetCustomerVehiclePolicy/{email}")]
        public IActionResult getCustomerVehiclePolicy(string email)
        {
           
            var data = db.CustomerVehiclePolicies.FromSqlInterpolated<CustomerVehiclePolicy>($"ShowCustomerVehiclePolicy {email}");




            return Ok(data);

        }




        //--GetPlanForReview
        [HttpGet]
        [Route("RenewInsurance/{policy_id}/{type}/{duration}")]
        public IActionResult GetPlanRenew(int policy_id, string type, int duration)
        {

            try
            {
                Policy policy = new Policy();
                Vehicle vehicle = new Vehicle();
                Plan plan = new Plan();

                policy = db.Policies.Find(policy_id);
                vehicle = db.Vehicles.Find(policy.RegistrationNumber);

                if (vehicle != null)
                {
                    string tp = vehicle.TypeOfVehicle;
                    plan = db.Plans.Where(p => (p.Term == duration) &&
                    (p.Type == type) && (p.Typeofvehicle == tp)).FirstOrDefault();
                }
                else
                {
                    return BadRequest("vehicle not found");
                }
                //    pid = from p in db.Plans where (p.Duration == plan1.Duration) && (p.Type == plan1.Type) && p.Id == 6  select p.Id;

                if (plan == null) return BadRequest("plan not found");

                //calculating age of vehicle
                int days = (vehicle.PurchaseDate - DateTime.Now).Days;
                days = Math.Abs(days);
                int year = days / 365;
                plan.Amount = amountAccordingToAge(plan.Amount, year);

                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }


        }


        //---RenewPolicy--
        [HttpPut]
        [Route("RenewInsurance/{policy_id}")]
        public IActionResult putPolicy( int policy_id,Plan plan)
        {

            try
            {
                Policy policy = new Policy();
                policy = db.Policies.Find(policy_id);

               policy.PlansId = plan.Id;
                policy.PurchaseDate = DateTime.Now;
               // policy.RenewAmount += plan.Amount;

                Vehicle vehicle = new Vehicle();
                vehicle = db.Vehicles.Find(policy.RegistrationNumber);

                int days = (vehicle.PurchaseDate - DateTime.Now).Days;
                days = Math.Abs(days);
                int year = days / 365;
                policy.RenewAmount += amountAccordingToAge(plan.Amount, year);
               

                db.SaveChanges();

                return Ok(policy);
            }
            catch (Exception Ex)
            {

                return BadRequest(Ex.InnerException.Message);
            }




        }

        /// <summary>
        /// Reset Password User
        /// </summary>
        /// <param name="emailid"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("Reset/{emailid}/{password}")]
        public IActionResult ResetPass(string emailid, string password)
        {
            if (emailid == null && password == null)
            {
                return BadRequest("Email id is null..");
            }
            try
            {
                Customer old = new Customer();
                old = db.Customers.Where(e => e.Email == emailid).FirstOrDefault();
                old.Password = password;
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception....{ex.InnerException.Message}");
            }

        }


        /// <summary>
        /// Calculation Estimate Vehicle
        /// </summary>
        /// <param name="typeofvehicle"></param>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("Calc/{typeofvehicle}/{type}/{duration}/{age}")]

        public IActionResult GetPlanCalc(string typeofvehicle, string type, int duration,int age)
        {

            try
            {

                Plan plan = new Plan();
                plan = db.Plans.Where(p => (p.Term == duration) &&
                 (p.Type == type) && (p.Typeofvehicle == typeofvehicle)).FirstOrDefault();

                if (plan == null) return BadRequest("plan not found");
                

                plan.Amount = amountAccordingToAge(plan.Amount, age);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        private decimal amountAccordingToAge(decimal planAmount, int Age)
        {


            if (Age < 3)
            {
                return planAmount;
            }
            else if (Age > 3 && Age < 5)
            {
                planAmount += (planAmount * 5) / 100;
            }
            else if (Age > 5 && Age < 10)
            {
                planAmount += (planAmount * 10) / 100;
            }
            else if (Age > 10 && Age < 20)
            {
                planAmount += (planAmount * 20) / 100;
            }
            else
            {
                planAmount += (planAmount * 30) / 100;

            }

            return planAmount;

        }


        [HttpGet]
        [Route("Validate/{policyId}/{contactNumber}/{email}")]

        public IActionResult GetValidate(int policyId, string contactNumber, string email)
        {
            

            Policy policy = new Policy();
            policy = db.Policies.Find(policyId);
            if (policy == null)
            {
                return BadRequest("No Policy with this policynumber! \n Enter Policy Number Correctly");
            }


            Customer customer = new Customer();
            customer = db.Customers.Find(policy.UserId);

            if (customer.Email != email)
            {
                return BadRequest("email is not matched! \n Enter email Correctly");
            }

            if (customer.ContactNumber != contactNumber)
            {
                return BadRequest("Contact number not matched! \n Enter Contact Number Correctly");
            }

            return Ok();

        }

        
    }
}
