using AirlineManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineManagement.Service
{
    public interface IFlightService
    {
        List<Flight> GetAllFlights();
        Flight GetFlightById(int id);
        bool AddFlight(Flight flight, out string error);
        bool UpdateFlight(Flight flight, out string error);
        void DeleteFlight(int id);
    }
}
