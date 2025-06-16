using AirlineManagement.Model;

namespace ClassLibraryValidation.Validators
{
    public static class FlightValidator
    {
        public static bool IsValid(Flight flight, out string error)
        {
            if (string.IsNullOrWhiteSpace(flight.FlightName))
            {
                error = "Flight name cannot be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(flight.DepAirport) || string.IsNullOrWhiteSpace(flight.ArrAirport))
            {
                error = "Both departure and arrival airports must be provided.";
                return false;
            }

            if (flight.DepAirport == flight.ArrAirport)
            {
                error = "Departure and arrival airports must be different.";
                return false;
            }

            if (flight.DepDate > flight.ArrDate ||
               (flight.DepDate == flight.ArrDate && flight.DepTime >= flight.ArrTime))
            {
                error = "Departure time must be before arrival time.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
