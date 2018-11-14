using System;
using System.Collections.Generic;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current => new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>(){
                new CityDto(){
                    Id=1,
                    Name="NewYork city",
                    Description="The one  with that big park.",
                    PointsOfInterest = new List<PointOfInterestDto>(){
                        new PointOfInterestDto{
                            Id=1,
                            Name="Central Park",
                            Description = "Most visited urban park"
                        },
                        new PointOfInterestDto{
                            Id=2,
                            Name="Empire state buildingk",
                            Description = "A 102-story skyscreeper"
                        }
                    }
                },
                new CityDto(){
                    Id= 2,
                    Name="Bogotá",
                    Description="The great city",
                    PointsOfInterest = new List<PointOfInterestDto>(){
                        new PointOfInterestDto{
                            Id=1,
                            Name="Central Park",
                            Description = "Most visited urban park"
                        },
                        new PointOfInterestDto{
                            Id=2,
                            Name="Empire state buildingk",
                            Description = "A 102-story skyscreeper"
                        }
                    }
                },
                new CityDto(){
                    Id=3,
                    Name="Tokyo",
                    Description="The most advanced city",
                    PointsOfInterest = new List<PointOfInterestDto>(){
                        new PointOfInterestDto{
                            Id=1,
                            Name="Central Park",
                            Description = "Most visited urban park"
                        },
                        new PointOfInterestDto{
                            Id=2,
                            Name="Empire state buildingk",
                            Description = "A 102-story skyscreeper"
                        }
                    }
                }
            };
        }
    }
}
