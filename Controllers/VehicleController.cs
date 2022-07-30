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
    public class VehicleController : ControllerBase
    {
        insuranceContext db = new insuranceContext();
        //get all vehicles
        [HttpGet]
        [Route("GetVehicles")]
        public IActionResult getVehicles()
        {
            var vh = from v in db.Vehicles select v;
            return Ok(vh);
        }


        //get vehicle by id
        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult getVehicle(string? id)
        {
            var vh = (from v in db.Vehicles where v.RegistrationNumber == id select v).FirstOrDefault();
            if (vh == null)
            {
                return BadRequest($"Vehicle {id} not present .....");
            }
            return Ok(vh);
        }



        //add vehicle
        [HttpPost]
        [Route("Add")]
        public IActionResult addVehicle(Vehicle v)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Vehicles.Add(v);
                    db.SaveChanges();
                }
                else
                {
                    return BadRequest($"Model state not valid .....");
                }
                return Created("Vehicle added ....", v);
            }
            catch(Exception ex)
            {
                return BadRequest($"Exception Occured .... {ex.Message}");
            }
        }

    }
}
