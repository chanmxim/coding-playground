using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Reservation_System
{
    public static class Utility
    {
        private static string flightFilePath = "flights.txt";
        private static string customerFilePath = "customers.txt";
        private static string bookingFilePath = "bookings.txt";
        private static string delimiter = "|%|%|";

        /// <summary>
        /// Joins an array of strings into a single string, separated by a specified delimiter.
        /// </summary>
        /// <param name="values">The array of strings to join.</param>
        /// <param name="delimiter">The delimiter used to separate the strings.</param>
        /// <returns>A single string containing all the values, separated by the delimiter.</returns>
        private static string stringJoinWithDelimiter(string[] values, string delimiter)
        {
            return string.Join(delimiter, values);
        }
        /// <summary>
        /// Saves the state of the FlightManager to a file.
        /// </summary>
        /// <param name="flightManager">The FlightManager instance containing flight data.</param>
        public static void saveManager(FlightManager flightManager)
        {
            try
            {
                using StreamWriter sw = new StreamWriter(flightFilePath);
                Flight[] flightList = flightManager.getFlightList();
                int flightNum = flightManager.getFlightNum(); // first line is number of objects stored in a file
                sw.WriteLine(flightNum);
                sw.WriteLine(0); // just to follow same logic as for other managers
                for (int i = 0; i < flightNum; i++)
                {
                    // sequence flightNumber, origin, destination, numOfPassengers, maxSeats;
                    string[] values = { flightList[i].getFlightNumber().ToString(),
                                flightList[i].getOrigin(),
                                flightList[i].getDestination(),
                                flightList[i].getNumOfPassengers().ToString(),
                                flightList[i].getmaxSeats().ToString() };
                    sw.WriteLine(stringJoinWithDelimiter(values, delimiter)); // concatenate string using delimiter
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// Saves the state of the CustomerManager to a file.
        /// </summary>
        /// <param name="customerManager">The CustomerManager instance containing customer data.</param>
        public static void saveManager(CustomerManager customerManager)
        {
            try
            {
                using StreamWriter sw = new StreamWriter(customerFilePath);
                Customer[] customerList = customerManager.getCustomerList();
                int customerNum = customerManager.getCustomerNum();
                sw.WriteLine(customerNum.ToString()); // first line is number of objects stored in a file
                sw.WriteLine(customerManager.getCounter().ToString()); // second line is id
                for (int i = 0; i < customerNum; i++)
                {
                    // sequence customerID, firstName, lastName, phone, numOfBookings
                    string[] values = { customerList[i].getCustomerID().ToString(),
                                customerList[i].getFirstName(),
                                customerList[i].getLastName(),
                                customerList[i].getPhone(),
                                customerList[i].getNumOfBookings().ToString() };
                    sw.WriteLine(stringJoinWithDelimiter(values, delimiter)); // concatenate string using delimiter
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// Saves the state of the BookingManager to a file.
        /// </summary>
        /// <param name="bookingManager">The BookingManager instance containing booking data.</param>
        public static void saveManager(BookingManager bookingManager)
        {
            try
            {
                using StreamWriter sw = new StreamWriter(bookingFilePath);
                Booking[] bookingList = bookingManager.getBookingList();
                int customerNum = bookingManager.getBookingNum();
                sw.WriteLine(customerNum.ToString()); // first line is number of objects stored in a file
                sw.WriteLine(bookingManager.getCounter().ToString()); // second line is id
                for (int i = 0; i < customerNum; i++)
                {
                    // sequence bookingID, customerID, flightNumber, date;
                    string[] values = { bookingList[i].getBookingId().ToString(),
                                bookingList[i].getCustomer().getCustomerID().ToString(),
                                bookingList[i].getFlight().getFlightNumber().ToString(),
                                bookingList[i].getDate() };
                    sw.WriteLine(stringJoinWithDelimiter(values, delimiter)); // concatenate string using delimiter
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// Loads data from the flight, customer, and booking files and initializes the system managers.
        /// </summary>
        /// <returns>An instance of <see cref="AirlineCoordinator"/> initialized with the data from the files, or <c>null</c> if data loading fails.</returns>
        public static AirlineCoordinator loadDataFromFiles()
        {
            // load fligts
            string[] flightData = readFile(flightFilePath);
            FlightManager fm = createFlightManager(flightData); //check null

            // load customers
            string[] customersData = readFile(customerFilePath);
            CustomerManager cm = createCustomerManager(customersData); //check null

            // load bookings
            if (fm != null && cm != null)
            {
                string[] BookingData = readFile(bookingFilePath);
                BookingManager bm = createBookingManager(BookingData, fm, cm); //check null
                if (bm != null)
                {
                    // create coordinator
                    return new AirlineCoordinator(fm,cm,bm);
                    
                }
            }
            return null;
        }
        /// <summary>
        /// Reads the content of a file and returns it as an array of strings (one string per line).
        /// </summary>
        /// <param name="filePath">The file path to read from.</param>
        /// <returns>An array of strings representing the lines of the file, or an empty array if the file could not be read.</returns>
        private static string[] readFile(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return [];
        }
        /// <summary>
        /// Creates a <see cref="FlightManager"/> instance from the provided flight data.
        /// </summary>
        /// <param name="data">The array of strings representing flight information from the file.</param>
        /// <returns>A <see cref="FlightManager"/> initialized with the loaded flight data, or <c>null</c> if data is invalid.</returns>
        private static FlightManager createFlightManager(string[] data)
        {
            if (data.Length > 2) // check that we have at least 3 rows in file
            {
                int num = int.Parse(data[0]);
                int id = int.Parse(data[1]);
                FlightManager manager = new FlightManager(num + 500);
                for (int i = 2; i < data.Length; i++)
                {
                    string[] splittedString = data[i].Split(delimiter);
                    // sequence flightNumber, origin, destination, numOfPassengers, maxSeats;
                    manager.addFlight(int.Parse(splittedString[0]), splittedString[1],
                        splittedString[2], int.Parse(splittedString[3]), int.Parse(splittedString[4]));
                }
                return manager;
            }
            return null;
        }
        /// <summary>
        /// Creates a <see cref="CustomerManager"/> instance from the provided customer data.
        /// </summary>
        /// <param name="data">The array of strings representing customer information from the file.</param>
        /// <returns>A <see cref="CustomerManager"/> initialized with the loaded customer data, or <c>null</c> if data is invalid.</returns>
        private static CustomerManager createCustomerManager(string[] data)
        {
            if (data.Length > 2) // check that we have at least 3 rows in file
            {
                int num = int.Parse(data[0]);
                int id = int.Parse(data[1]);
                CustomerManager manager = new CustomerManager(num + 500, id);
                for (int i = 2; i < data.Length; i++)
                {
                    string[] splittedString = data[i].Split(delimiter);
                    // sequence customerID, firstName, lastName, phone, numOfBookings
                    manager.addCustomer(int.Parse(splittedString[0]), splittedString[1],
                        splittedString[2], splittedString[3], int.Parse(splittedString[4]));
                }
                return manager;
            }
            return null;
        }
        /// <summary>
        /// Creates a <see cref="BookingManager"/> instance from the provided booking data, using the given <see cref="FlightManager"/> and <see cref="CustomerManager"/> instances.
        /// </summary>
        /// <param name="data">The array of strings representing booking information from the file.</param>
        /// <param name="fm">The <see cref="FlightManager"/> used to find flight details.</param>
        /// <param name="cm">The <see cref="CustomerManager"/> used to find customer details.</param>
        /// <returns>A <see cref="BookingManager"/> initialized with the loaded booking data, or <c>null</c> if data is invalid.</returns>
        private static BookingManager createBookingManager(string[] data, FlightManager fm, CustomerManager cm)
        {
            if (data.Length > 2) // check that we have at least 3 rows in file
            {
                int index;
                int num = int.Parse(data[0]);
                int id = int.Parse(data[1]);
                BookingManager manager = new BookingManager(num + 500, id);
                for (int i = 2; i < data.Length; i++)
                {
                    string[] splittedString = data[i].Split(delimiter);
                    // sequence bookingID, customerID, flightNumber, date;
                    manager.addBooking(int.Parse(splittedString[0]),
                        cm.findCustomer(int.Parse(splittedString[1]), out index),
                        fm.findFlight(int.Parse(splittedString[2]), out index),
                        splittedString[3]);
                }
                return manager;
            }
            return null;
        }
    }//class end
}
