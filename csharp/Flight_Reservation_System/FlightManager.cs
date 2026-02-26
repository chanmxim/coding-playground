using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Flight_Reservation_System
{
    public class FlightManager
    {
        private Flight[] flightList;
        private int maxNum;
        private int flightNum;
        private int[] columnWidth = initializeWidth(); // store max width for each column of a Flight objects
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightManager"/> class with a specified maximum number of flights.
        /// </summary>
        /// <param name="maxNum">The maximum number of flights that can be managed by this instance of the <see cref="FlightManager"/>.</param>
        public FlightManager(int maxNum)
        {
            this.flightList = new Flight[maxNum];
            this.maxNum = maxNum;
            this.flightNum = 0;
        }
        /// <summary>
        /// Attempts to add a new flight to the flight list.
        /// Checks if the flight number is unique, ensures there is space for the flight, 
        /// and returns a success flag. If the operation fails, an error message is returned.
        /// </summary>
        /// <param name="flightNumber">The flight number of the new flight as an integer.</param>
        /// <param name="origin">The origin of the flight as a string.</param>
        /// <param name="destination">The destination of the flight as a string.</param>
        /// <param name="maxSeats">The maximum number of seats available for the flight as an integer.</param>
        /// <param name="errorMsg">An output parameter that will contain an error message if the operation fails. Otherwise, it will be an empty string.</param>
        /// <returns>Returns <c>true</c> if the flight was added successfully, <c>false</c> if there was an error.</returns>
        public bool addFlight(int flightNumber, string origin, string destination, int maxSeats, out string errorMsg)
        {
            bool isUnique = checkFlightNumberUniqueness(flightNumber);
            errorMsg = "";
            if (isUnique)
            {
                if (flightNum < maxNum)
                {
                    flightList[flightNum] = new Flight(flightNumber, origin, destination, maxSeats);
                    checkWidth(flightList[flightNum]);
                    flightNum++;
                    return true;
                }
                else { errorMsg = $"Reached maximum number of fligts {flightNum}/{maxNum}"; }
            }
            else { errorMsg = $"Flight number - {flightNumber} is already in use!"; }
            return false;
        }
        /// <summary>
        /// Adds a new flight to the flight list with the specified flight details.
        /// This method is typically used when loading flight data from a file.
        /// </summary>
        /// <param name="flightNumber">The flight number as an integer.</param>
        /// <param name="origin">The origin of the flight as a string.</param>
        /// <param name="destination">The destination of the flight as a string.</param>
        /// <param name="numOfPassenger">The current number of passengers on the flight as an integer.</param>
        /// <param name="maxSeats">The maximum number of available seats for the flight as an integer.</param>
        public void addFlight(int flightNumber, string origin, string destination, int numOfPassenger, int maxSeats)
        {
            flightList[flightNum] = new Flight(flightNumber, origin, destination, numOfPassenger, maxSeats);
            checkWidth(flightList[flightNum++]);
        }
        /// <summary>
        /// Attempts to delete a flight from the flight list.
        /// The flight will be deleted only if it has no registered passengers.
        /// If the flight is found and deleted successfully, returns <c>true</c>.
        /// If the flight has passengers or is not found, returns <c>false</c> and sets an error message.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to be deleted.</param>
        /// <param name="errorMsg">An output parameter that will contain an error message if the deletion fails. Otherwise, it will be an empty string.</param>
        /// <returns>Returns <c>true</c> if the flight was deleted successfully or <c>false</c> if the deletion failed.</returns>
        public bool deleteFlight(int flightNumber, out string errorMsg)
        {
            Flight flight = findFlight(flightNumber, out int index);
            errorMsg = "";
            if (flight != null)
            {
                if (flight.getNumOfPassengers() == 0)
                {
                    flightList[index] = flightList[flightNum--];
                    flightNum--;
                    return true;
                }
                else { errorMsg = $"Flight number - {flightNumber} has registered passangers!"; }
            }
            else { errorMsg = $"Flight number - {flightNumber} is not found!"; };
            return false;
        }
        /// <summary>
        /// Searches for a flight by its flight number in the flight list.
        /// If the flight is found, returns the <see cref="Flight"/> object and sets the index to the flight's position in the list.
        /// If the flight is not found, returns <c>null</c> and sets the index to -1.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to search for.</param>
        /// <param name="index">An output parameter that will contain the index of the flight in the flight list if the flight is found; otherwise, it will be set to -1.</param>
        /// <returns>Returns the <see cref="Flight"/> object if the flight is found; otherwise, returns <c>null</c>.</returns>
        public Flight findFlight(int flightNumber, out int index)
        {
            for (int i = 0; i < flightNum; i++)
            {
                if (flightList[i].getFlightNumber() == flightNumber)
                {
                    index = i;
                    return flightList[i];
                }
            }
            index = -1;
            return null;
        }
        /// <summary>
        /// Gets detailed information about a specific flight, including headers and flight details.
        /// If the flight is found, returns a string containing the flight's information.
        /// If the flight is not found, returns an empty string and sets an error message.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to view.</param>
        /// <param name="errorMsg">An output parameter that will contain an error message if the flight is not found. Otherwise, it will be an empty string.</param>
        /// <returns>Returns a string containing the flight's details, including headers, if the flight is found. Otherwise, returns an empty string if the flight is not found.</returns>
        public string viewFlight(int flightNumber, out string errorMsg)
        {
            string s = "";
            errorMsg = "";
            Flight flight = findFlight(flightNumber, out int index);
            if (flight != null)
            {
                s += generateHeadersString();
                s += flight.getInfo(columnWidth);
            }
            else { errorMsg = $"Flight number - {flightNumber} is not found!"; }
            return s;
        }
        /// <summary>
        /// Gets detailed information for all flights in the flight list.
        /// If there are flights available, it returns a string containing information for all flights, including headers.
        /// If there are no flights to display, it returns an empty string and sets an error message.
        /// </summary>
        /// <param name="errorMsg">An output parameter that will contain an error message if no flights are available to display. Otherwise, it will be an empty string.</param>
        /// <returns>Returns a string containing the details of all flights, including headers, if flights are available. Otherwise, returns an empty string if no flights are available.</returns>
        public string viewAllFlights(out string errorMsg)
        {
            string s = "";
            errorMsg = "";
            if (flightNum > 0)
            {
                s += generateHeadersString();
                for (int i = 0; i < flightNum; i++)
                {
                    s += flightList[i].getInfo(columnWidth) + "\n";
                }
            }
            else { errorMsg = "There is no flights to display.\nPlease add flight first."; }
            return s;
        }
        /// <summary>
        /// Checks if a flight number is unique by searching for an existing flight with the same number.
        /// Returns <c>true</c> if the flight number is unique (no such flight exists), and <c>false</c> if a flight with the given number already exists.
        /// </summary>
        /// <param name="flightNumber">The flight number to check for uniqueness.</param>
        /// <returns><c>true</c> if the flight number is unique, <c>false</c> if a flight with the same number already exists.</returns>
        private bool checkFlightNumberUniqueness(int flightNumber)
        {
            if (findFlight(flightNumber, out int index) == null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Compares the current flight's column widths with the stored column widths and updates the stored widths to ensure each column is large enough.
        /// For each column, the method updates the corresponding width in the <c>columnWidth</c> array to the maximum of the current width and the flight's width.
        /// </summary>
        /// <param name="flight">The <see cref="Flight"/> object whose column widths are compared and used to update the <c>columnWidth</c> array.</param>
        private void checkWidth(Flight flight)
        {
            int[] currentWidth = flight.getWidth();
            for (int i = 0; i < currentWidth.Length; i++)
            {
                columnWidth[i] = Math.Max(columnWidth[i], currentWidth[i]);
            }
        }
        /// <summary>
        /// Initializes the column widths based on the lengths of the header strings for the flight information.
        /// This method creates an array where each element represents the width of a column, starting with the length of the corresponding header string.
        /// </summary>
        /// <returns>Returns an array of integers representing the initial column widths, each set to the length of the corresponding header string.</returns>
        private static int[] initializeWidth()
        {
            string[] headers = Flight.getHeaders();
            int[] returnValue = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                returnValue[i] = headers[i].Length;
            }
            return returnValue;
        }
        /// <summary>
        /// Generates a formatted string containing the headers for the flight information.
        /// Each header is padded to ensure that the columns have sufficient width based on the current column widths.
        /// A separator line of dashes is appended after the headers to visually separate them from the data rows.
        /// </summary>
        /// <returns>Returns a formatted string with the headers, each padded to the appropriate width, followed by a separator line.</returns>
        private string generateHeadersString()
        {
            string s = "";
            string[] headers = Flight.getHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                s += headers[i].PadRight(columnWidth[i] + 5);
            }
            s += "\n" + new string('-', 100) + "\n";
            return s;

        }
        /// <summary>
        /// Gets the list of all flights currently stored in the flight manager.
        /// This method returns an array of <see cref="Flight"/> objects, representing all the flights in the system.
        /// This method is typically used when save flight data to a file.
        /// </summary>
        /// <returns>Returns an array of <see cref="Flight"/> objects representing the list of flights.</returns>
        public Flight[] getFlightList() { return flightList; }
        /// <summary>
        /// Gets the current number of flights stored in the flight manager.
        /// This method returns an integer representing the number of flights that have been added to the system.
        /// This method is typically used when save flight data to a file.
        /// </summary>
        /// <returns>Returns the current number of flights as an integer.</returns>
        public int getFlightNum() { return flightNum; }
    }
}