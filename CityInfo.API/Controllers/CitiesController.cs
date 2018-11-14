using System;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : BaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {

                var result = this.Execute(() => CitiesDataStore.Current);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var reslt = this.Execute(() =>
            {
                CityDto city = CitiesDataStore.Current.Cities.Find(x => x.Id == id);
                return city;
            });
            return reslt != null ? Ok(reslt) : (IActionResult)NotFound();
        }
    }
}
