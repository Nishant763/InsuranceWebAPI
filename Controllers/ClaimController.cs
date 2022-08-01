using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceWebAPI.Models;
using InsuranceWebAPI.ViewModels;

namespace InsuranceWebAPI.Controllers
{
    /// <summary>
    /// For managing claims made by the user 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        insuranceContext db = new insuranceContext();

        /// <summary>
        /// fetching all the claims in the database
        /// </summary>
        /// <returns>list of claims</returns>
         
        [HttpGet]
        [Route("GetClaims")]

        public IActionResult GetClaims()
        {
            var claims = from claim in db.Claims select claim;

            return Ok(claims);
        }


        /// <summary>
        /// get claim based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Claim based on id</returns>
        
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

        /// <summary>
        /// Add claim in the database
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("Add")]
        public IActionResult addClaim(Claim c)
        {
        
               if (ModelState.IsValid)
                {
                    try
                    {
                        Claim c1 = new Claim();
                        c1.ClaimDate = DateTime.Now;
                        c1.Isapproved = c.Isapproved;
                        c1.Policy = c.Policy;
                        c1.PolicyId = c.PolicyId;

                    Policy p = new Policy();
                    p = db.Policies.Find(c.PolicyId);

                    Plan pl = new Plan();
                    pl = db.Plans.Find(p.PlansId);

                    int days = (p.PurchaseDate - c1.ClaimDate).Days;
                    days = Math.Abs(days);
                    int year = (days / 365);
                    if(year > pl.Term)
                    {
                        return BadRequest("Can't be claimed due to timeout......");
                    }
                        
                        
                        db.Claims.Add(c1);
                        db.SaveChanges();
                        return Ok();
                    }
                    catch(Exception ex)
                    {
                        return BadRequest($"{ex.InnerException.Message}");
                    }
                }
                    return BadRequest($"Claim not validated...");
                

               
        }

        /// <summary>
        /// Edit Claim based on id -> Changes the isapproved attribute
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Claim object</returns>
        [HttpDelete]
        [Route("Edit/{id}")]

        public IActionResult editClaim(int id)
        {
            try
            {
                Claim oldC = new Claim();
                 oldC = db.Claims.Find(id);
                //old.Isapproved = !old.Isapproved;

                Policy p = new Policy();
                p = db.Policies.Find(oldC.PolicyId);
                p.RenewAmount = 0;
                
                db.Claims.Remove(oldC);
                db.SaveChanges();
                return Ok(oldC);
            }
            catch(Exception ex)
            {
                return BadRequest($"{ex.InnerException.Message}");
            }
        }

    }
}
