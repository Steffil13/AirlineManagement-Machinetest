using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data.SqlClient;
using AirlineManagement.Model;
using AirlineManagement.Service;
using DBConnectionHelper;

namespace AirlineManagementSystem
{
    public class FlyWithMe
    {
        static void Main(string[] args)
        {
            Console.Title = "FlyWithMe Airline Management System";

            if (!Login())
            {
                Console.WriteLine("Login failed. Exiting...");
                return;
            }

            IFlightService flightService = new FlightService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== FlyWithMe Dashboard ===");
                Console.WriteLine("1. Add Flight");
                Console.WriteLine("2. View All Flights");
                Console.WriteLine("3. View Flight by ID");
                Console.WriteLine("4. Update Flight");
                Console.WriteLine("5. Delete Flight");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddFlight(flightService);
                        break;
                    case "2":
                        ViewAllFlights(flightService);
                        break;
                    case "3":
                        ViewFlightById(flightService);
                        break;
                    case "4":
                        UpdateFlight(flightService);
                        break;
                    case "5":
                        DeleteFlight(flightService);
                        break;
                    case "6":
                        Console.WriteLine("Thank you for using FlyWithMe.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static bool Login()
        {
            Console.Clear();
            Console.WriteLine("=== Admin Login ===");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            using var con = DBConnectivity.GetConnection();
            con.Open();

            string query = "SELECT AdminId FROM Admin WHERE UserName = @User AND Password = @Pass";

            using var cmd = con.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@User", username);
            cmd.Parameters.AddWithValue("@Pass", password);

            using var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }

        static void AddFlight(IFlightService service)
        {
            Console.WriteLine("\n=== Add New Flight ===");

            var flight = ReadFlightDetails();

            if (service.AddFlight(flight, out string error))
            {
                Console.WriteLine("Flight added successfully.");
            }
            else
            {
                Console.WriteLine($"Error: {error}");
            }
        }

        static void ViewAllFlights(IFlightService service)
        {
            Console.WriteLine("\n=== All Flights ===");
            var flights = service.GetAllFlights();

            if (flights.Count == 0)
            {
                Console.WriteLine("No flights found.");
                return;
            }

            foreach (var f in flights)
            {
                PrintFlight(f);
            }
        }

        static void ViewFlightById(IFlightService service)
        {
            Console.Write("\nEnter Flight ID to view: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var flight = service.GetFlightById(id);
                if (flight != null)
                {
                    PrintFlight(flight);
                }
                else
                {
                    Console.WriteLine("Flight not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static void UpdateFlight(IFlightService service)
        {
            Console.Write("\nEnter Flight ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var existing = service.GetFlightById(id);
                if (existing == null)
                {
                    Console.WriteLine("Flight not found.");
                    return;
                }

                Console.WriteLine("Enter new details:");
                var updated = ReadFlightDetails();
                updated.FlightId = id;

                if (service.UpdateFlight(updated, out string error))
                {
                    Console.WriteLine("Flight updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: {error}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static void DeleteFlight(IFlightService service)
        {
            Console.Write("\nEnter Flight ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var existing = service.GetFlightById(id);
                if (existing == null)
                {
                    Console.WriteLine("Flight not found.");
                    return;
                }

                service.DeleteFlight(id);
                Console.WriteLine("Flight deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static Flight ReadFlightDetails()
        {
            Console.Write("Flight Name: ");
            string name = Console.ReadLine();

            string dep = GetCityFromUser("Departure");
            string arr = GetCityFromUser("Arrival");

            Console.Write("Departure Date (yyyy-mm-dd): ");
            DateTime depDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Departure Time (HH:mm): ");
            DateTime depTime = DateTime.ParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture);

            Console.Write("Arrival Date (yyyy-mm-dd): ");
            DateTime arrDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Arrival Time (HH:mm): ");
            DateTime arrTime = DateTime.ParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture);

            Console.Write("Is Active? (true/false): ");
            bool status = bool.Parse(Console.ReadLine());

            return new Flight
            {
                FlightName = name,
                DepAirport = dep,
                ArrAirport = arr,
                DepDate = depDate,
                DepTime = depTime,
                ArrDate = arrDate,
                ArrTime = arrTime,
                Status = status
            };
        }

        static string GetCityFromUser(string label)
        {
            Console.WriteLine($"\n=== Select {label} Airport ===");

            using var con = DBConnectivity.GetConnection();
            con.Open();

            string query = @"SELECT c.CityId, c.CityName, co.CountryName
                     FROM Cities c
                     INNER JOIN Countries co ON c.CountryId = co.CountryId";

            using var cmd = con.CreateCommand();
            cmd.CommandText = query;

            using var reader = cmd.ExecuteReader();

            var cities = new List<(int Id, string Name, string Country)>();
            while (reader.Read())
            {
                cities.Add((
                    (int)reader["CityId"],
                    reader["CityName"].ToString(),
                    reader["CountryName"].ToString()
                ));
            }

            for (int i = 0; i < cities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {cities[i].Name}, {cities[i].Country}");
            }

            int choice;
            do
            {
                Console.Write("Enter option number: ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > cities.Count);

            return cities[choice - 1].Name;
        }



        static void PrintFlight(Flight f)
        {
            Console.WriteLine($"ID: {f.FlightId} | Name: {f.FlightName}");
            Console.WriteLine($"From: {f.DepAirport} on {f.DepDate:yyyy-MM-dd} at {f.DepTime:HH:mm}");
            Console.WriteLine($"To  : {f.ArrAirport} on {f.ArrDate:yyyy-MM-dd} at {f.ArrTime:HH:mm}");
            Console.WriteLine($"Status: {(f.Status ? "Active" : "Cancelled")}");
            Console.WriteLine("---------------------------------------------");
        }
    }
}
