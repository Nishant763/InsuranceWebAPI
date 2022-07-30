using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceWebAPI.Models;
using Newtonsoft.Json.Linq;

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
        [Route("Login/{email}")]
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
                return Ok("Logged in....");
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
    }
}
