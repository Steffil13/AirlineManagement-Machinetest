using AirlineManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineManagement.Repository
{
    public interface IFlightRepository
    {
        List<Flight> GetAllFlights();
        Flight GetFlightById(int id);
        void AddFlight(Flight flight);
        void UpdateFlight(Flight flight);
        void DeleteFlight(int id);
    }

}
