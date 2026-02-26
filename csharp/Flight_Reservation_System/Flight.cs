using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Flight_Reservation_System
{
    public class Flight
    {
        private int flightNumber;
        private string origin;
        private string destination;
        private int numOfPassengers;
        private int maxSeats;
        private static string[] headers = { "Flight number", "Origin", "Destination", "Number of passengers", "Maximum number of seats" };
        /// <summary>
        /// Initializes a new instance of the <see cref="Flight"/> class with the specified flight details.
        /// </summary>
        /// <param name="flightNumber"> Number of a Flight as integer.</param>
        /// <param name="origin">Origin as string.</param>
        /// <param name="destination"> Flight destination as string.</param>
        /// <param name="maxSeats"> Maximum number of available seats for Flight as int.</param>
        public Flight(int flightNumber, string origin, string destination, int maxSeats)
        {
            this.flightNumber = flightNumber;
            this.origin = origin;
            this.destination = destination;
            this.maxSeats = maxSeats;
            this.numOfPassengers = 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Flight"/> class with the specified flight details, including the number of passengers.
        /// This constructor is typically used when loading flight data from a file.
        /// </summary>
        /// <param name="flightNumber"> Number of a Flight as integer.</param>
        /// <param name="origin">Origin as string.</param>
        /// <param name="destination"> Flight destination as string.</param>
        /// <param name="numOfPassengers">Number of passengers for Flight as int.</param>
        /// <param name="maxSeats"> Maximum number of available seats for Flight as int.</param>
        public Flight(int flightNumber, string origin, string destination, int numOfPassengers, int maxSeats)
        {
            this.flightNumber = flightNumber;
            this.origin = origin;
            this.destination = destination;
            this.maxSeats = maxSeats;
            this.numOfPassengers = numOfPassengers;
        }
        /// <summary>
        /// Gets the Flight number.
        /// </summary>
        /// <returns>The Flight number as int.</returns>
        public int getFlightNumber() { return flightNumber; }
        /// <summary>
        /// Gets number of passengers for the Flight.
        /// </summary>
        /// <returns>The number of passengers as int.</returns>
        public int getNumOfPassengers() { return numOfPassengers; }
        /// <summary>
        /// Gets the origin of the Flight.
        /// </summary>
        /// <returns>The origin of the Flight as string.</returns>
        public string getOrigin() { return origin; }
        /// <summary>
        /// Gets the destination of the Flight.
        /// </summary>
        /// <returns>The destination of the Flight as string.</returns>
        public string getDestination() { return destination; }
        /// <summary>
        /// Gets maximum number of the Flight.
        /// </summary>
        /// <returns>The maximum number of the Flight as int.</returns>
        public int getmaxSeats() {  return maxSeats; }
        /// <summary>
        /// Calculates and returns the character widths of various properties of the flight.
        /// </summary>
        /// <returns>An array of integers representing the length (in characters) of the following properties:
        /// flight number, origin, destination, maximum number of seats, and number of passengers.</returns>
        public int[] getWidth()
        {
            int[] width = new int[headers.Length];
            width[0] = flightNumber.ToString().Length;
            width[1] = origin.Length;
            width[2] = destination.Length;
            width[3] = maxSeats.ToString().Length;
            width[4] = numOfPassengers.ToString().Length;
            return width;
        }
        /// <summary>
        /// Gets the headers used for displaying flight data.
        /// </summary>
        /// <returns>An array of strings representing the headers for flight data, such as flight number, origin, destination, etc.</returns>
        public static string[] getHeaders()
        {
            return headers;
        }
        /// <summary>
        /// Generates a formatted string with information about the flight, padded to align with specified column widths.
        /// </summary>
        /// <param name="columnwidth">An array of integers specifying the width for each column. The array should contain five values representing the widths for the flight number, origin, destination, number of passengers, and maximum seats, respectively.</param>
        /// <returns>A formatted string containing the flight information, with each value padded to the specified column widths.</returns>
        public string getInfo(int[] columnwidth)
        {
            string s = "";
            s += flightNumber.ToString().PadRight(columnwidth[0] + 5);
            s += origin.PadRight(columnwidth[1] + 5);
            s += destination.PadRight(columnwidth[2] + 5);
            s += numOfPassengers.ToString().PadRight(columnwidth[3] + 5);
            s += maxSeats.ToString().PadRight(columnwidth[4] + 5);

            return s;
        }
        /// <summary>
        /// Adjusts the number of passengers for the flight, ensuring the new number of passengers is valid.
        /// </summary>
        /// <param name="num">The number of passengers to add (positive) or remove (negative) from the current count.</param>
        /// <returns>Returns <c>true</c> if the adjustment is successful  or <c>false</c> if the adjustment is not allowed.</returns>
        public bool setNumOfPassengers(int num)
        {
            if (maxSeats - numOfPassengers > 0 && numOfPassengers + num >= 0)
            {
                numOfPassengers += num;
                return true;
            }
            return false;
        }
    }
}
