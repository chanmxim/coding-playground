using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Flight_Reservation_System
{
    public class AirlineCoordinator
    {
        private FlightManager flightManager;
        private CustomerManager customerManager;
        private BookingManager bookingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AirlineCoordinator"/> class with the specified maximum numbers of flights, customers, and bookings.
        /// The class manages the creation of <see cref="FlightManager"/>, <see cref="CustomerManager"/>, and <see cref="BookingManager"/>.
        /// </summary>
        /// <param name="maxFlight">The maximum number of flights that can be managed by the <see cref="FlightManager"/>.</param>
        /// <param name="maxCustomer">The maximum number of customers that can be managed by the <see cref="CustomerManager"/>.</param>
        /// <param name="maxBooking">The maximum number of bookings that can be managed by the <see cref="BookingManager"/>.</param>
        public AirlineCoordinator(int maxFlight, int maxCustomer, int maxBooking)
        {
            this.flightManager = new FlightManager(maxFlight);
            this.customerManager = new CustomerManager(maxCustomer);
            this.bookingManager = new BookingManager(maxBooking);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AirlineCoordinator"/> class using existing instances of <see cref="FlightManager"/>, 
        /// <see cref="CustomerManager"/>, and <see cref="BookingManager"/>.
        /// This constructor is typically used when loading  data from a file.
        /// </summary>
        /// <param name="flightManager">An existing instance of the <see cref="FlightManager"/> to be used by the coordinator.</param>
        /// <param name="customerManager">An existing instance of the <see cref="CustomerManager"/> to be used by the coordinator.</param>
        /// <param name="bookingManager">An existing instance of the <see cref="BookingManager"/> to be used by the coordinator.</param>
        public AirlineCoordinator(FlightManager flightManager, CustomerManager customerManager, BookingManager bookingManager)
        {
            this.flightManager = flightManager;
            this.customerManager = customerManager;
            this.bookingManager = bookingManager;
        }
        /// <summary>
        /// Gets the <see cref="FlightManager"/> instance managed by the coordinator.
        /// This method is typically used when save data to a file.
        /// </summary>
        /// <returns>The <see cref="FlightManager"/> instance.</returns>
        public FlightManager getFlightManager() { return flightManager; }
        /// <summary>
        /// Gets the <see cref="CustomerManager"/> instance managed by the coordinator.
        /// This method is typically used when save data to a file.
        /// </summary>
        /// <returns>The <see cref="CustomerManager"/> instance.</returns>
        public CustomerManager getCustomerManager() { return customerManager; }
        /// <summary>
        /// Gets the <see cref="BookingManager"/> instance managed by the coordinator.
        /// This method is typically used when save data to a file.
        /// </summary>
        /// <returns>The <see cref="BookingManager"/> instance.</returns>
        public BookingManager getBookingManager() { return bookingManager; }

        //Flight

        /// <summary>
        /// Adds a new flight to the system using the specified flight details.
        /// </summary>
        /// <param name="flightNumber">The unique flight number.</param>
        /// <param name="origin">The origin location of the flight.</param>
        /// <param name="destination">The destination location of the flight.</param>
        /// <param name="maxSeats">The maximum number of seats available on the flight.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns <c>true</c> if the flight was successfully added; otherwise, <c>false</c>.</returns>
        public bool addFlight(int flightNumber, string origin, string destination, int maxSeats, out string errorMsg)
        {
            if (flightManager.addFlight(flightNumber, origin, destination, maxSeats, out errorMsg))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Deletes an existing flight based on the flight number.
        /// </summary>
        /// <param name="flightNumber">The unique flight number of the flight to be deleted.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns <c>true</c> if the flight was successfully deleted; otherwise, <c>false</c>.</returns>
        public bool deleteFlight(int flightNumber, out string errorMsg)
        {
            if (flightManager.deleteFlight(flightNumber, out errorMsg))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Gets detailed information about a specific flight based on the flight number.
        /// </summary>
        /// <param name="flightNumber">The unique flight number of the flight to be viewed.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns a string representing the flight's details if found; otherwise, an error message.</returns>
        public string viewFlight(int flightNumber, out string errorMsg)
        {
            return flightManager.viewFlight(flightNumber, out errorMsg);
        }
        /// <summary>
        /// Gets detailed information about all flights in the system.
        /// </summary>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns a string representing the details of all flights if available; otherwise, an error message.</returns>
        public string viewAllFlights(out string errorMsg)
        {
            return flightManager.viewAllFlights(out errorMsg);
        }
        /// <summary>
        /// Finds a specific flight based on the provided flight ID and returns its index in the flight list.
        /// </summary>
        /// <param name="id">The unique ID of the flight to find.</param>
        /// <param name="index">An output parameter that returns the index of the flight in the flight list.</param>
        /// <returns>Returns the <see cref="Flight"/> object if found; otherwise, <c>null</c>.</returns>
        public Flight findFlight(int id, out int index)
        {
            return flightManager.findFlight(id, out index);
        }

        //Customer

        /// <summary>
        /// Adds a new customer to the system using the specified customer details.
        /// </summary>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="phone">The phone number of the customer.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns <c>true</c> if the customer was successfully added; otherwise, <c>false</c>.</returns>
        /// <returns></returns>
        public bool addCustomer(string firstName, string lastName, string phone, out string errorMsg)
        {
            if (customerManager.addCustomer(firstName, lastName, phone, out errorMsg))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Deletes an existing customer from the system based on their customer ID.
        /// </summary>
        /// <param name="customerID">The unique ID of the customer to be deleted.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns <c>true</c> if the customer was successfully deleted; otherwise, <c>false</c>.</returns>
        public bool deleteCustomer(int customerID, out string errorMsg)
        {
            if (customerManager.deleteCustomer(customerID, out errorMsg))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Gets detailed information about a specific customer based on their customer ID.
        /// </summary>
        /// <param name="customerID">The unique ID of the customer to be viewed.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns a string representing the customer's details if found; otherwise, an error message.</returns>
        public string viewCustomer(int customerID, out string errorMsg)
        {
            return customerManager.viewCustomer(customerID, out errorMsg);
        }
        /// <summary>
        /// Gets detailed information about all customers in the system.
        /// </summary>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns a string representing the details of all customers if available; otherwise, an error message.</returns>
        public string viewAllCustomers(out string errorMsg)
        {
            return customerManager.viewAllCustomers(out errorMsg);
        }
        /// <summary>
        /// Finds a specific customer based on the provided customer ID and returns their index in the customer list.
        /// </summary>
        /// <param name="id">The unique ID of the customer to find.</param>
        /// <param name="index">An output parameter that returns the index of the customer in the customer list.</param>
        /// <returns>Returns the <see cref="Customer"/> object if found; otherwise, <c>null</c>.</returns>
        public Customer findCustomer(int id, out int index)
        {
            return customerManager.findCustomer(id, out index);
        }


        //Booking

        /// <summary>
        /// Adds a new booking for a customer on a specific flight.
        /// </summary>
        /// <param name="customer">The customer who is making the booking.</param>
        /// <param name="flight">The flight on which the customer is booking a seat.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns <c>true</c> if the booking was successfully added; otherwise, <c>false</c>.</returns>
        public bool addBooking(Customer customer, Flight flight, out string errorMsg)
        {
            if (bookingManager.addBooking(customer, flight, out errorMsg))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Deletes an existing booking based on the booking ID.
        /// </summary>
        /// <param name="id">The unique ID of the booking to be deleted.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns <c>true</c> if the booking was successfully deleted; otherwise, <c>false</c>.</returns>
        public bool deleteBooking(int id, out string errorMsg)
        {
            if (bookingManager.deleteBooking(id, out errorMsg))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Gets the details of a specific booking based on the booking ID.
        /// </summary>
        /// <param name="id">The unique ID of the booking to view.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns a string representing the booking details if found; otherwise, an error message.</returns>
        public string viewBooking(int id, out string errorMsg)
        {
            return bookingManager.viewBooking(id, out errorMsg);
        }
        /// <summary>
        /// Gets the details of all bookings in the system.
        /// </summary>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns a string representing the details of all bookings if available; otherwise, an error message.</returns>
        public string viewAllBookings(out string errorMsg)
        {
            return bookingManager.viewAllBookings(out errorMsg);
        }
        /// <summary>
        /// Gets a list of all customers who have booked a seat on a specific flight.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight whose customers are to be retrieved.</param>
        /// <param name="errorMsg">An output parameter that returns an error message, if any.</param>
        /// <returns>Returns a string representing the list of customers for the given flight if available; otherwise, an error message.</returns>
        public string viewAllCustomersForBookedFlight(int flightNumber, out string errorMsg)
        {
            return bookingManager.viewAllCustomersForBookedFlight(flightNumber, out errorMsg);
        }
    }
}
