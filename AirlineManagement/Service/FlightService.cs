using AirlineManagement.Model;
using AirlineManagement.Repository;
using ClassLibraryValidation.Validators;
//using DBConnectionHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineManagement.Service
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _repo;

        public FlightService()
        {
            _repo = new FlightRepository();
        }

        public List<Flight> GetAllFlights() => _repo.GetAllFlights();

        public Flight GetFlightById(int id) => _repo.GetFlightById(id);
        //public bool AddFlight(Flight flight, out string error)
        //{
        //    error = string.Empty; // provide a dummy value
        //    _repo.AddFlight(flight);
        //    return true;
        //}

        //public bool UpdateFlight(Flight flight, out string error)
        //{
        //    error = string.Empty;
        //    _repo.UpdateFlight(flight);
        //    return true;
        //}

        public bool AddFlight(Flight flight, out string error)
        {
            if (!FlightValidator.IsValid(flight, out error))
                return false;

            _repo.AddFlight(flight);
            return true;
        }

        public bool UpdateFlight(Flight flight, out string error)
        {
            if (!FlightValidator.IsValid(flight, out error))
                return false;

            _repo.UpdateFlight(flight);
            return true;
        }

        public void DeleteFlight(int id) => _repo.DeleteFlight(id);
    }
}
