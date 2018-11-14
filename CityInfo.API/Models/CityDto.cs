using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PointOfInterestDto> PointsOfInterest { get; set; }
        = new List<PointOfInterestDto>();

        public int NumberOfPointsOfInterest => PointsOfInterest.Count();
    }
}
