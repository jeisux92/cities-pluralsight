using System;
using System.Linq;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInteresController : BaseController
    {
        private readonly ILogger Logger;

        private readonly IMailService LocalMailService;

        public PointOfInteresController(ILogger<PointOfInteresController> logger, IMailService localMailService)
        {
            this.Logger = logger;
            this.LocalMailService = localMailService;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointOfInterest(int cityId)
        {
            try
            {
                var pointsOfInterest = this.Execute(() =>
                {
                    return CitiesDataStore.Current.Cities.Find(x => x.Id == cityId)?.PointsOfInterest;
                });

                return pointsOfInterest == null ? NotFound() : (IActionResult)Ok(pointsOfInterest);
            }
            catch (Exception ex)
            {
                this.Logger.LogInformation($"The city with id {cityId} wasn't found when accesing", ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{cityId}/pointsOfInterest/{id}", Name = "GetPointsOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var pointOfInterest = this.Execute(() =>
            {
                return CitiesDataStore.Current.Cities.Find(x => x.Id == cityId)
                                      ?.PointsOfInterest.FirstOrDefault(poi => poi.Id == id);
            });
            return pointOfInterest == null ? NotFound() : (IActionResult)Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterestForCreationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(cityd => cityd.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }
            var pointsOfInterest =
            CitiesDataStore.Current.Cities
                           .Find(x => x.Id == cityId)?.PointsOfInterest;



            if (pointsOfInterest == null)
            {
                return BadRequest();
            }

            int masPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(cities => cities.PointsOfInterest)
                                                      .Max(p => p.Id);
            PointOfInterestDto pointOfInterestDto = new PointOfInterestDto
            {
                Id = ++masPointOfInterestId,
                Description = pointOfInterestForCreationDto.Description,
                Name = pointOfInterestForCreationDto.Name
            };

            city.PointsOfInterest
            .Add(pointOfInterestDto);

            return CreatedAtRoute("GetPointsOfInterest",
                                  new { cityId, id = pointOfInterestDto.Id }, pointOfInterestDto);
        }

        [HttpPut("{cityId}/pointOfInterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody]  PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            if (pointOfInterestForUpdateDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PointOfInterestDto pointOfInterestDto = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId)?
                                             .PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (pointOfInterestDto == null)
            {
                return NotFound();
            }

            pointOfInterestDto.Name = pointOfInterestForUpdateDto.Name;
            pointOfInterestDto.Description = pointOfInterestForUpdateDto.Description;

            return NoContent();

        }

        [HttpPatch("{cityId}/pointOfInterest/{id}")]
        public IActionResult PartialUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> pathDocument)
        {
            if (pathDocument == null)
            {
                return BadRequest();
            }

            PointOfInterestDto pointOfInterestDto = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId)?.
                                                                   PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (pointOfInterestDto == null)
            {
                return NotFound();
            }

            PointOfInterestForUpdateDto pointOfInterestForUpdateDto = new PointOfInterestForUpdateDto
            {
                Description = pointOfInterestDto.Description,
                Name = pointOfInterestDto.Name
            };

            pathDocument.ApplyTo(pointOfInterestForUpdateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            TryValidateModel(pointOfInterestForUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            pointOfInterestDto.Name = pointOfInterestForUpdateDto.Name;
            pointOfInterestDto.Description = pointOfInterestForUpdateDto.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointOfInterest/{id}")]
        public IActionResult DeletePOintOfInteres(int cityId, int id)
        {

            PointOfInterestDto pointOfInterestDto = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId)?.
                                                                   PointsOfInterest.FirstOrDefault(x => x.Id == id);
            if (pointOfInterestDto == null)
            {
                return BadRequest();
            }

            CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id)?.
                           PointsOfInterest.Remove(pointOfInterestDto);

            LocalMailService.Send("Point of interest deleted.",
                                  $"Point of interest {pointOfInterestDto.Name} with id" +
                                  $"{pointOfInterestDto.Id} was deleted");

            return NoContent();
        }
    }
}
