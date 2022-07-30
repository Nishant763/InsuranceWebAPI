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
    public class PlanController : ControllerBase
    {
        insuranceContext db = new insuranceContext();
        //get all the plans
        [HttpGet]
        [Route("GetPlans")]
        public IActionResult getPlans()
        {
            var pl = from p in db.Plans select p;
            return Ok(pl);
        }
    }
}
