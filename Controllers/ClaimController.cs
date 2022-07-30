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
            try
            {
                if (ModelState.IsValid)
                {
                    db.Claims.Add(c);
                    db.SaveChanges();
                }
                else
                {
                    return BadRequest($"Claim not validate");
                }

                return Created($"Claim added .....",c);
            }
            catch(Exception ex)
            {
                return BadRequest($"Exception occured .... {ex.Message}");
            }
        }
    }
}
