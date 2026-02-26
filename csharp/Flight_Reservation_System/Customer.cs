using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Flight_Reservation_System
{
    public class Customer
    {
        private int customerID;
        private string firstName;
        private string lastName;
        private string phone;
        private int numOfBookings;
        private static string[] headers = { "Customer ID", "First name", "Last name", "Phone number", "Bookings made" };
        private static string[] headersFlight = { "Customer ID", "Customer full name" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class with the specified customer details.
        /// This constructor sets the <c>customerID</c>, <c>firstName</c>, <c>lastName</c>, and <c>phone</c> properties,
        /// and initializes <c>numOfBookings</c> to 0, indicating that no bookings have been made for the customer yet.
        /// </summary>
        /// <param name="customerID">The unique identifier for the customer.</param>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="phone">The phone number of the customer.</param>
        public Customer(int customerID, string firstName, string lastName, string phone)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.phone = phone;
            numOfBookings = 0;
            this.customerID = customerID;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class with the specified customer details.
        /// This constructor is typically used when loading flight data from a file.
        /// </summary>
        /// <param name="customerID">The unique identifier for the customer.</param>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="phone">The phone number of the customer.</param>
        /// <param name="numOfBookings">The number of bookings made by the customer.</param>
        public Customer(int customerID, string firstName, string lastName, string phone, int numOfBookings)
        {
            this.firstName=firstName;
            this.lastName=lastName;
            this.phone = phone;
            this.customerID = customerID;
            this.numOfBookings = numOfBookings;
        }
        /// <summary>
        /// Gets the number of bookings that the customer has made.
        /// This method returns an integer representing the total number of bookings associated with the customer.
        /// </summary>
        /// <returns>Returns the total number of bookings as an integer.</returns>
        public int getNumOfBookings() { return numOfBookings; }
        /// <summary>
        /// Updates the number of bookings for the customer by adding the specified value.
        /// This method ensures that the total number of bookings cannot be negative. 
        /// If the new number of bookings would be negative, the method returns <c>false</c>, 
        /// otherwise it updates the booking count and returns <c>true</c>.
        /// </summary>
        /// <param name="num">The number of bookings to add (can be negative to decrease bookings).</param>
        /// <returns><c>true</c> if the number of bookings was successfully updated; 
        /// <c>false</c> if the resulting number of bookings would be negative.</returns>
        public bool setNumOfBookings(int num)
        {
            if (numOfBookings + num >= 0)
            {
                numOfBookings += num;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Gets the headers for the customer data.
        /// </summary>
        /// <returns>An array of strings representing the column headers for customer data.</returns>
        public static string[] getHeaders() { return headers; }
        /// <summary>
        /// Gets the headers for the flight data.
        /// </summary>
        /// <returns>An array of strings representing the column headers for flight data.</returns>
        public static string[] getHeadersFlight() { return headersFlight; }
        /// <summary>
        /// Gets the unique identifier for the customer.
        /// </summary>
        /// <returns>The customer's unique identifier as an integer.</returns>
        public int getCustomerID() { return customerID; }
        /// <summary>
        /// Gets the first name of the customer.
        /// </summary>
        /// <returns>The customer's first name as a string.</returns>
        public string getFirstName() { return firstName; }
        /// <summary>
        /// Gets the last name of the customer.
        /// </summary>
        /// <returns>The customer's last name as a string.</returns>
        public string getLastName() { return lastName; }
        /// <summary>
        /// Gets the phone number of the customer.
        /// </summary>
        /// <returns>The customer's phone number as a string.</returns>
        public string getPhone() { return phone; }
        /// <summary>
        /// Calculates the width of each column based on the customer data (customerID, firstName, lastName, phone, numOfBookings).
        /// This is useful for dynamically adjusting column widths in displays or tables.
        /// </summary>
        /// <returns>An array of integers representing the width of each column in the customer data table.</returns>
        public int[] getWidth()
        {
            int[] width = new int[headers.Length];
            width[0] = customerID.ToString().Length;
            width[1] = firstName.Length;
            width[2] = lastName.Length;
            width[3] = phone.Length;
            width[4] = numOfBookings.ToString().Length;
            return width;
        }
        /// <summary>
        /// Calculates the width of each column based on the flight-related customer data (customerID, firstName, and lastName).
        /// This is used for formatting the customer’s flight information display.
        /// </summary>
        /// <returns>An array of integers representing the width of each column for the flight display table.</returns>
        public int[] getWidthFlight()
        {
            int[] width = new int[headersFlight.Length];
            width[0] = customerID.ToString().Length;
            width[1] = firstName.Length + lastName.Length+1;
            return width;
        }
        /// <summary>
        /// Gets a formatted string of customer information (customerID, firstName, lastName, phone, numOfBookings) 
        /// aligned with the provided column widths.
        /// </summary>
        /// <param name="columnwidth">An array of integers representing the column widths for each customer data field.</param>
        /// <returns>A string containing the formatted customer information with proper padding for each field.</returns>
        public string getInfo(int[] columnwidth)
        {
            string s = "";
            s += customerID.ToString().PadRight(columnwidth[0] + 5);
            s += firstName.PadRight(columnwidth[1] + 5);
            s += lastName.PadRight(columnwidth[2] + 5);
            s += phone.PadRight(columnwidth[3] + 5);
            s += numOfBookings.ToString().PadRight(columnwidth[4] + 5);

            return s;
        }
        /// <summary>
        /// Gets a formatted string of flight-related customer information (customerID, firstName, and lastName) 
        /// aligned with the provided column widths.
        /// </summary>
        /// <param name="columnwidth">An array of integers representing the column widths for each flight-related data field.</param>
        /// <returns>A string containing the formatted flight
        public string getInfoFlight(int[] columnwidth)
        {
            string s = "";
            s += customerID.ToString().PadRight(columnwidth[0] + 5);
            s += firstName+" "+lastName.PadRight(columnwidth[1] + 5);
            return s;
        }

    }
}
