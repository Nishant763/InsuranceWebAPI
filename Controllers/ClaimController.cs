using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceWebAPI.Models;

namespace InsuranceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        insuranceContext db = new insuranceContext();
        //get claims 
        [HttpGet]
        [Route("GetClaims")]

        public IActionResult GetClaims()
        {
            var claims = from claim in db.Claims select claim;

            return Ok(claims);
        }



        //get claim based on id
        [HttpGet]
        [Route("Get/{id}")]

        public IActionResult getClaim(int? id)
        {
            var claim = (from cl in db.Claims where cl.Id == id select cl).FirstOrDefault();
            if (claim==null)
            {
                return BadRequest($"Claim {id} doesn't exist");
            }

            return Ok(claim);
        }


        //add claim
        [HttpPost]
        [Route("Add")]
        public IActionResult addClaim(Claim c)
        {
        
               // if (ModelState.IsValid)
                //{
                    try
                    {
                        Claim c1 = new Claim();
                        c1.ClaimDate = DateTime.Now;
                        c1.Isapproved = c.Isapproved;
                        c1.Policy = c.Policy;
                        c1.PolicyId = c.PolicyId;
                        
                        db.Claims.Add(c1);
                        db.SaveChanges();
                        return Ok();
                    }
                    catch(Exception ex)
                    {
                        return BadRequest($"{ex.InnerException.Message}");
                    }
                //}
                   // return BadRequest($"Claim not validate");
                

               
        }
    }
}
