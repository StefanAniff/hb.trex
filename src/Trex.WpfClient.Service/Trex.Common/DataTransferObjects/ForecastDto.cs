using System;
using System.Collections.Generic;
using System.Linq;

namespace Trex.Common.DataTransferObjects
{
    public class ForecastMonthDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CreatedById { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLocked { get; set; }
        public ICollection<ForecastDto> ForecastDtos { get; set; }
    }

    public class ForecastDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal? DedicatedForecastTypeHours { get; set; }
        public ForecastTypeDto ForecastType { get; set; }
        public ICollection<ForecastProjectHoursDto> ForecastProjectHoursDtos { get; set; }
    }

    public class ForecastTypeDto
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ColorStringHex { get; set; }
        public bool SupportsProjectHours { get; set; }
        public bool SupportsDedicatedHours { get; set; }
    }    

    public class ForecastProjectHoursDto
    {
        public ProjectDto Project { get; set; } 
        public decimal Hours { get; set; }
    }

    public class ForecastUserDto
    {
        public int UserId { get; set; }

        /// <summary>
        /// Username/Initials
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Name/Full name
        /// </summary>
        public string Name { get; set; }

        public string SearchString
        {
            get { return string.Format("{0} {1}", UserName, Name); }
        }
    }
}