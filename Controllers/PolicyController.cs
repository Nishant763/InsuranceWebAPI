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
    public class PolicyController : ControllerBase
    {
        insuranceContext db = new insuranceContext();
        //get all policies
        [HttpGet]
        [Route("GetPolicies")]
        public IActionResult getPolicies()
        {
            var policies = from p in db.Policies select p;
            return Ok(policies);
        }


        //get policy based on id
        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult getPolicy(int? id)
        {
            var p = (from pl in db.Policies where pl.Id == id select pl).FirstOrDefault();
            if (p == null)
            {
                return BadRequest($"Policy {id} not found .....");
            }
            return Ok(p);
        }


        //add policy
        [HttpPost]
        [Route("AddPolicy")]
        public IActionResult addPolicy(Policy p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Policies.Add(p);
                    db.SaveChanges();
                }
                else
                {
                    return BadRequest($"Policy not valid .....");
                }

                return Created("Policy added....", p);
            }
            catch(Exception ex)
            {
                return BadRequest($"Exception occured .... {ex.Message}");
            }
        }
    }
}
