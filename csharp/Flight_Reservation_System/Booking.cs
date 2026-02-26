using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Flight_Reservation_System
{
    public class Booking
    {
        private int bookingID;
        private Customer customer;
        private Flight flight;
        private string date;
        private static string[] headers = { "Booking ID", "Customer ID", "Customer full name", "Flight number", "Date" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class with the specified booking ID, customer, and flight.
        /// The booking date is set to the current date and time.
        /// </summary>
        /// <param name="bookingID">The unique booking ID for the booking.</param>
        /// <param name="customer">The <see cref="Customer"/> associated with the booking.</param>
        /// <param name="flight">The <see cref="Flight"/> associated with the booking.</param>
        public Booking(int bookingID, Customer customer, Flight flight)
        {
            this.customer = customer;
            this.flight = flight;
            this.date = DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt");
            this.bookingID = bookingID;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class with the specified booking ID, customer, flight, and date.
        /// This constructor is typically used when loading flight data from a file.
        /// </summary>
        /// <param name="bookingID">The unique booking ID for the booking.</param>
        /// <param name="customer">The <see cref="Customer"/> associated with the booking.</param>
        /// <param name="flight">The <see cref="Flight"/> associated with the booking.</param>
        /// <param name="date">The date of the booking in the format "MM/dd/yyyy h:mm tt".</param>
        public Booking(int bookingID, Customer customer, Flight flight, string date)
        {
            this.customer = customer;
            this.flight = flight;
            this.date = date;
            this.bookingID = bookingID;
        }
        /// <summary>
        /// Gets the unique booking ID for this booking.
        /// </summary>
        /// <returns>The booking ID as int.</returns>
        public int getBookingId() { return bookingID; }
        /// <summary>
        /// Gets the <see cref="Flight"/> associated with this booking.
        /// </summary>
        /// <returns>The <see cref="Flight"/> for this booking.</returns>
        public Flight getFlight() { return flight; }
        /// <summary>
        /// Gets the <see cref="Customer"/> associated with this booking.
        /// </summary>
        /// <returns>The <see cref="Customer"/> for this booking.</returns>
        public Customer getCustomer() { return customer; }
        /// <summary>
        /// Gets the date when the booking was made.
        /// </summary>
        /// <returns>The booking date in the format "MM/dd/yyyy h:mm tt" as string.</returns>
        public string getDate() { return date; }
        /// <summary>
        /// Gets the headers for the booking table.
        /// </summary>
        /// <returns>An array of strings representing the column headers for the booking table.</returns>
        public static string[] getHeaders() { return headers; }
        /// <summary>
        /// Gets the column widths for displaying the booking information in a table.
        /// The widths are based on the length of the booking ID, customer information, flight number, and date.
        /// </summary>
        /// <returns>An array of integers representing the column widths for the booking table.</returns>
        public int[] getWidth()
        {
            int[] width = new int[headers.Length];
            width[0] = bookingID.ToString().Length;
            width[1] = customer.getCustomerID().ToString().Length;
            width[2] = (customer.getFirstName() + " " + customer.getLastName()).Length;
            width[3] = flight.getFlightNumber().ToString().Length;
            width[4] = date.Length;
            return width;
        }
        /// <summary>
        /// Gets a string containing the booking information, formatted with the specified column widths.
        /// </summary>
        /// <param name="columnwidth">An array of integers representing the column widths for each field.</param>
        /// <returns>A formatted string containing the booking details.</returns>
        public string getInfo(int[] columnwidth)
        {
            string s = "";
            s += bookingID.ToString().PadRight(columnwidth[0] + 5);
            s += customer.getCustomerID().ToString().PadRight(columnwidth[1] + 5);
            s += (customer.getFirstName() + " " + customer.getLastName()).PadRight(columnwidth[2] + 5);
            s += flight.getFlightNumber().ToString().PadRight(columnwidth[3] + 5);
            s += date.PadRight(columnwidth[4] + 5);

            return s;
        }
    }
}
