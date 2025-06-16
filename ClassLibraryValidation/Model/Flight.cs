using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineManagement.Model
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightName { get; set; }
        public string DepAirport { get; set;}
        public int AirportId { get; set; }
        public DateTime DepDate { get; set; }
        public DateTime DepTime { get; set; }
        public string ArrAirport { get; set; }
        public DateTime ArrDate { get; set; }
        public DateTime ArrTime { get; set; }
        public bool Status { get; set; }
        public int StatusId { get; set; }

    }
}
