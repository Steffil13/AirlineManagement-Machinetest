using AirlineManagement.Model;
using DBConnectionHelper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AirlineManagement.Repository
{
    public class FlightRepository : IFlightRepository
    {
        public List<Flight> GetAllFlights()
        {
            List<Flight> flights = new();
            using SqlConnection con = DBConnectivity.GetConnection();
            con.Open();

            SqlCommand cmd = new("sp_GetAllFlights", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                flights.Add(new Flight
                {
                    FlightId = (int)reader["FlightId"],
                    FlightName = reader["FlightName"].ToString(),
                    DepAirport = reader["DepAirport"].ToString(),
                    ArrAirport = reader["ArrAirport"].ToString(),
                    DepDate = (DateTime)reader["DepDate"],
                    DepTime = DateTime.Today + (TimeSpan)reader["DepTime"],
                    ArrDate = (DateTime)reader["ArrDate"],
                    ArrTime = DateTime.Today + (TimeSpan)reader["ArrTime"],
                    Status = (bool)reader["Status"]
                });
            }

            return flights;
        }

        public Flight GetFlightById(int id)
        {
            using SqlConnection con = DBConnectivity.GetConnection();
            con.Open();

            SqlCommand cmd = new("sp_GetFlightById", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@FlightId", id);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Flight
                {
                    FlightId = (int)reader["FlightId"],
                    FlightName = reader["FlightName"].ToString(),
                    DepAirport = reader["DepAirport"].ToString(),
                    ArrAirport = reader["ArrAirport"].ToString(),
                    DepDate = (DateTime)reader["DepDate"],
                    DepTime = DateTime.Today + (TimeSpan)reader["DepTime"],
                    ArrDate = (DateTime)reader["ArrDate"],
                    ArrTime = DateTime.Today + (TimeSpan)reader["ArrTime"],
                    Status = (bool)reader["Status"]
                };
            }

            return null;
        }

        public void AddFlight(Flight flight)
        {
            using SqlConnection con = DBConnectivity.GetConnection();
            con.Open();

            SqlCommand cmd = new("sp_AddFlight", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@FlightName", flight.FlightName);
            cmd.Parameters.AddWithValue("@DepAirport", flight.DepAirport);
            cmd.Parameters.AddWithValue("@ArrAirport", flight.ArrAirport);
            cmd.Parameters.AddWithValue("@DepDate", flight.DepDate);
            cmd.Parameters.AddWithValue("@DepTime", flight.DepTime.TimeOfDay);
            cmd.Parameters.AddWithValue("@ArrDate", flight.ArrDate);
            cmd.Parameters.AddWithValue("@ArrTime", flight.ArrTime.TimeOfDay);
            cmd.Parameters.AddWithValue("@Status", flight.Status);

            cmd.ExecuteNonQuery();
        }

        public void UpdateFlight(Flight flight)
        {
            using SqlConnection con = DBConnectivity.GetConnection();
            con.Open();

            SqlCommand cmd = new("sp_UpdateFlight", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@FlightId", flight.FlightId);
            cmd.Parameters.AddWithValue("@FlightName", flight.FlightName);
            cmd.Parameters.AddWithValue("@DepAirport", flight.DepAirport);
            cmd.Parameters.AddWithValue("@ArrAirport", flight.ArrAirport);
            cmd.Parameters.AddWithValue("@DepDate", flight.DepDate);
            cmd.Parameters.AddWithValue("@DepTime", flight.DepTime.TimeOfDay);
            cmd.Parameters.AddWithValue("@ArrDate", flight.ArrDate);
            cmd.Parameters.AddWithValue("@ArrTime", flight.ArrTime.TimeOfDay);
            cmd.Parameters.AddWithValue("@Status", flight.Status);

            cmd.ExecuteNonQuery();
        }

        public void DeleteFlight(int id)
        {
            using SqlConnection con = DBConnectivity.GetConnection();
            con.Open();

            SqlCommand cmd = new("sp_DeleteFlight", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@FlightId", id);

            cmd.ExecuteNonQuery();
        }
    }
}
