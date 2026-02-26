using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Flight_Reservation_System
{
    public class BookingManager
    {
        private Booking[] bookingList;
        private int maxNum;
        private int bookingNum;
        private int[] columnWidth = initializeWidth(); // store max width for each column of a Booking objects
        private static int idCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingManager"/> class with the specified maximum number of bookings.
        /// The booking ID counter is set to 1 and the number of bookings is initially 0.
        /// </summary>
        /// <param name="maxNum">The maximum number of bookings that can be stored.</param>
        public BookingManager(int maxNum)
        {
            this.bookingList = new Booking[maxNum];
            this.maxNum = maxNum;
            this.bookingNum = 0;
            idCounter = 1;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BookingManager"/> class with the specified maximum number of bookings 
        /// and a custom starting booking ID.
        /// This constructor is typically used when loading booking data from a file.
        /// </summary>
        /// <param name="maxNum">The maximum number of bookings that can be stored.</param>
        /// <param name="id">The starting booking ID.</param>
        public BookingManager(int maxNum, int id) 
        {
            this.bookingList = new Booking[maxNum];
            this.maxNum = maxNum;
            this.bookingNum = 0;
            idCounter = id;
        }
        /// <summary>
        /// Adds a new booking for the specified customer and flight. 
        /// If there are no available seats on the flight or the maximum number of bookings is reached, an error message is returned.
        /// </summary>
        /// <param name="customer">The <see cref="Customer"/> making the booking.</param>
        /// <param name="flight">The <see cref="Flight"/> the customer is booking.</param>
        /// <param name="errorMsg">An error message returned if the booking fails.</param>
        /// <returns>True if the booking was successfully added, otherwise false.</returns>
        public bool addBooking(Customer customer, Flight flight, out string errorMsg)
        {
            errorMsg = "";
            if (bookingNum < maxNum)
            {
                if (flight.setNumOfPassengers(1) && customer.setNumOfBookings(1))
                {
                    bookingList[bookingNum] = new Booking(idCounter++,customer, flight);
                    checkWidth(bookingList[bookingNum]);
                    bookingNum++;
                    return true;
                }
                else { errorMsg = $"There is no free seats availibale on flight {flight.getFlightNumber()}"; }
            }
            else { errorMsg = $"Reached maximum number of customers {bookingNum}/{maxNum}"; }
            return false;
        }
        /// <summary>
        /// Adds a new booking with a custom booking ID, customer, flight, and booking date.
        /// This method is typically used when loading flight data from a file.
        /// </summary>
        /// <param name="bookingID">The custom booking ID.</param>
        /// <param name="customer">The <see cref="Customer"/> making the booking.</param>
        /// <param name="flight">The <see cref="Flight"/> the customer is booking.</param>
        /// <param name="date">The booking date.</param>
        public void addBooking(int bookingID, Customer customer, Flight flight, string date)
        {
            bookingList[bookingNum] = new Booking(bookingID, customer, flight, date);
            checkWidth(bookingList[bookingNum++]);
        }
        /// <summary>
        /// Deletes a booking by the specified booking ID. 
        /// If the booking is successfully deleted, the number of passengers for the flight and number of bookings for the customer are decremented.
        /// </summary>
        /// <param name="id">The booking ID of the booking to delete.</param>
        /// <param name="errorMsg">An error message returned if the deletion fails.</param>
        /// <returns>True if the booking was successfully deleted, otherwise false.</returns>
        public bool deleteBooking(int id, out string errorMsg)
        {
            Booking booking = findBooking(id, out int index);
            errorMsg = "";
            if (booking != null)
            {
                if (bookingList[index].getFlight().setNumOfPassengers(-1) && bookingList[index].getCustomer().setNumOfBookings(-1))
                {
                    bookingList[index] = bookingList[bookingNum];
                    bookingNum--;
                    return true;
                }
            }
            else { errorMsg = $"The booking with id - {id} is not found!"; }
            return false;
        }
        /// <summary>
        /// Gets and displays the details of a booking by the specified booking ID.
        /// </summary>
        /// <param name="id">The booking ID of the booking to view.</param>
        /// <param name="errorMsg">An error message returned if the booking is not found.</param>
        /// <returns>A string containing the formatted booking details.</returns>
        public string viewBooking(int id, out string errorMsg)
        {
            string s = "";
            errorMsg = "";
            Booking booking = findBooking(id, out int index);
            if (booking != null)
            {
                s += generateHeadersString();
                s += booking.getInfo(columnWidth);
            }
            else { errorMsg = $"The booking with id - {id} is not found!"; }
            return s;
        }
        /// <summary>
        /// Gets and displays all bookings in the system.
        /// </summary>
        /// <param name="errorMsg">An error message returned if no bookings exist.</param>
        /// <returns>A string containing all the booking details in a formatted table.</returns>
        public string viewAllBookings(out string errorMsg)
        {
            string s = "";
            errorMsg = "";
            if (bookingNum != 0)
            {
                s += generateHeadersString();
                for (int i = 0; i < bookingNum; i++)
                {
                    s += bookingList[i].getInfo(columnWidth) + "\n";
                }
            }
            else { errorMsg = "There is no bookings to display.\nPlease add booking first."; }
            return s;
        }
        /// <summary>
        /// Gets and displays all customers who have booked a specific flight, based on the flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to check for bookings.</param>
        /// <param name="errorMsg">An error message returned if no bookings for the flight are found.</param>
        /// <returns>A string containing a list of all customers who booked the specified flight.</returns>
        public string viewAllCustomersForBookedFlight(int flightNumber, out string errorMsg)
        {
            Booking[] bookings = findAllBookingsForFlight(flightNumber);
            string s = "";
            errorMsg = "";
            if (bookings.Length != 0)
            {
                int[] currentWidth = customerColumnWidth(bookings);
                s += generateHeadersString(currentWidth);
                for (int i = 0; i < bookings.Length; i++)
                {
                    s += bookings[i].getCustomer().getInfoFlight(currentWidth);
                }
            }
            else { errorMsg = $"The bookings with flight number - {flightNumber} is not found!"; }
            return s;
        }
        /// <summary>
        /// Finds all bookings associated with a given flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to search for bookings.</param>
        /// <returns>An array of <see cref="Booking"/> objects for the specified flight number.</returns>
        private Booking[] findAllBookingsForFlight(int flightNumber)
        {
            Booking[] bookings = new Booking[0];
            for (int i = 0; i < bookingNum; i++)
            {
                if (bookingList[i].getFlight().getFlightNumber() == flightNumber)
                {
                    bookings = bookings.Concat(new Booking[] { bookingList[i] }).ToArray();
                }
            }
            return bookings;
        }
        /// <summary>
        /// Finds a booking by its ID.
        /// </summary>
        /// <param name="id">The booking ID to search for.</param>
        /// <param name="index">The index of the found booking in the list.</param>
        /// <returns>The <see cref="Booking"/> object if found, otherwise <c>null</c>.</returns>
        private Booking findBooking(int id, out int index)
        {
            for (int i = 0; i < bookingNum; i++)
            {
                if (bookingList[i].getBookingId() == id)
                {
                    index = i;
                    return bookingList[i];
                }
            }
            index = -1;
            return null;
        }
        /// <summary>
        /// Checks and updates the column widths for displaying booking information based on the given booking.
        /// </summary>
        /// <param name="booking">The <see cref="Booking"/> object to use for width calculations.</param>
        private void checkWidth(Booking booking)
        {
            int[] currentWidth = booking.getWidth();
            for (int i = 0; i < currentWidth.Length; i++)
            {
                columnWidth[i] = Math.Max(columnWidth[i], currentWidth[i]);
            }
        }
        /// <summary>
        /// Initializes the column widths based on the headers for the booking table.
        /// </summary>
        /// <returns>An array of integers representing the initial column widths based on the headers.</returns>
        private static int[] initializeWidth()
        {
            string[] headers = Booking.getHeaders();
            int[] returnValue = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                returnValue[i] = headers[i].Length;
            }
            return returnValue;
        }
        /// <summary>
        /// Calculates the column widths for customer information based on the given bookings.
        /// </summary>
        /// <param name="bookings">An array of <see cref="Booking"/> objects to calculate column widths for.</param>
        /// <returns>An array of integers representing the column widths for customer information.</returns>
        private int[] customerColumnWidth(Booking[] bookings)
        {
            string[] headers = Customer.getHeadersFlight();
            int[] returnValue = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                returnValue[i] = headers[i].Length;
            }

            for (int i = 0; i < bookings.Length; i++)
            {
                int[] currentWidth = bookings[i].getCustomer().getWidthFlight();
                for (int j = 0; j < currentWidth.Length; j++)
                {
                    returnValue[i] = Math.Max(returnValue[i], currentWidth[i]);
                }
            }

            return returnValue;
        }
        /// <summary>
        /// Generates a string representation of the headers for the booking table.
        /// </summary>
        /// <returns>A formatted string containing the column headers for the booking table.</returns>
        private string generateHeadersString()
        {
            string s = "";
            string[] headers = Booking.getHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                s += headers[i].PadRight(columnWidth[i] + 5);
            }
            s += "\n" + new string('-', 100) + "\n";
            return s;
        }
        /// <summary>
        /// Generates a string representation of the headers for the customer booking table with the specified column widths.
        /// </summary>
        /// <param name="currentWidth">An array of integers representing the column widths for the customer table.</param>
        /// <returns>A formatted string containing the column headers for the customer table.</returns>
        private string generateHeadersString(int[] currentWidth)
        {
            string s = "";
            string[] headers = Customer.getHeadersFlight();
            for (int i = 0; i < headers.Length; i++)
            {
                s += headers[i].PadRight(currentWidth[i] + 5);
            }
            s += "\n" + new string('-', 100) + "\n";

            return s;
        }
        /// <summary>
        /// Gets the list of all bookings.
        /// This method is typically used when save Booking data to a file.
        /// </summary>
        /// <returns>An array of <see cref="Booking"/> objects representing all bookings.</returns>
        public Booking[] getBookingList() { return bookingList; }
        /// <summary>
        /// Gets the number of bookings currently stored in the system.
        /// This method is typically used when save Booking data to a file.
        /// </summary>
        /// <returns>The number of bookings as int.</returns>
        public int getBookingNum() {  return bookingNum; }
        /// <summary>
        /// Gets the current booking ID counter value.
        /// This method is typically used when save Booking data to a file.
        /// </summary>
        /// <returns>The current booking ID counter value as int.</returns>
        public int getCounter() { return idCounter; }
        
    }
}
