using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Flight_Reservation_System
{
    public class CustomerManager
    {
        private Customer[] customerList;
        private int maxNum;
        private int customerNum;
        private int[] columnWidth = initializeWidth(); // store max width for each column of a Customer objects
        private static int idCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManager"/> class.
        /// This constructor sets the maximum number of customers and initializes the list of customers.
        /// </summary>
        /// <param name="maxNum">The maximum number of customers that can be stored.</param>
        public CustomerManager(int maxNum)
        {
            this.maxNum = maxNum;
            customerList = new Customer[maxNum];
            customerNum = 0;
            idCounter = 1;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManager"/> class with a specified ID counter.
        /// This constructor sets the maximum number of customers, the initial ID, and initializes the customer list.
        /// This constructor is typically used when loading customer data from a file.
        /// </summary>
        /// <param name="maxNum">The maximum number of customers that can be stored.</param>
        /// <param name="id">The starting ID for new customers.</param>
        public CustomerManager(int maxNum, int id)
        {
            this.maxNum = maxNum;
            customerList = new Customer[maxNum];
            customerNum = 0;
            idCounter = id;
        }
        /// <summary>
        /// Adds a new customer to the customer list if they do not already exist.
        /// The method checks for uniqueness based on the customer's first name, last name, and phone number.
        /// </summary>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="phone">The phone number of the customer.</param>
        /// <param name="errorMsg">An output parameter that contains an error message if the addition fails.</param>
        /// <returns>True if the customer was added successfully; otherwise, false.</returns>
        public bool addCustomer(string firstName, string lastName, string phone, out string errorMsg)
        {
            bool isUnique = checkCustomerUniquenss(firstName, lastName, phone);
            errorMsg = "";
            if (isUnique)
            {
                if (customerNum < maxNum)
                {
                    customerList[customerNum] = new Customer(idCounter++,firstName, lastName, phone);
                    checkWidth(customerList[customerNum]);
                    customerNum++;
                    return true;
                }
                else { errorMsg = $"Reached maximum number of customers {customerNum}/{maxNum}"; }
            }
            else { errorMsg = $"The customer {firstName} {lastName} {phone} already excist!"; }
            return false;
        }
        /// <summary>
        /// Adds a new customer with a specified ID to the customer list.
        /// This method is typically used when loading customer data from a file.
        /// </summary>
        /// <param name="id">The unique customer ID.</param>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="phone">The phone number of the customer.</param>
        /// <param name="numOfBookings">The number of bookings associated with the customer.</param>
        public void addCustomer(int id, string firstName, string lastName, string phone, int numOfBookings)
        {
            customerList[customerNum] = new Customer(id, firstName, lastName, phone, numOfBookings);
            checkWidth(customerList[customerNum++]);
        }
        /// <summary>
        /// Deletes a customer from the customer list if they have no active bookings.
        /// </summary>
        /// <param name="customerID">The unique customer ID of the customer to delete.</param>
        /// <param name="errorMsg">An output parameter that contains an error message if the deletion fails.</param>
        /// <returns>True if the customer was deleted successfully; otherwise, false.</returns>
        public bool deleteCustomer(int customerID, out string errorMsg)
        {
            Customer customer = findCustomer(customerID, out int index);
            errorMsg = "";
            if (customer != null)
            {
                if (customer.getNumOfBookings() == 0)
                {
                    customerList[index] = customerList[customerNum];
                    customerNum--;
                    return true;
                }
                else { errorMsg = $"The customer with id - {customerID} has active bookings {customer.getNumOfBookings()}"; }
            }
            else { errorMsg = $"The customer with id - {customerID} is not found!"; }
            return false;
        }
        /// <summary>
        /// Finds a customer by their unique customer ID.
        /// </summary>
        /// <param name="id">The unique customer ID.</param>
        /// <param name="index">An output parameter that holds the index of the found customer in the list, or -1 if not found.</param>
        /// <returns>The customer if found; otherwise, <c>null</c>.</returns>
        public Customer findCustomer(int id, out int index)
        {
            for (int i = 0; i < customerNum; i++)
            {
                if (customerList[i].getCustomerID() == id)
                {
                    index = i;
                    return customerList[i];
                }
            }
            index = -1;
            return null;
        }
        /// <summary>
        /// Views detailed information for a specific customer.
        /// </summary>
        /// <param name="customerID">The unique customer ID of the customer to view.</param>
        /// <param name="errorMsg">An output parameter that contains an error message if the customer is not found.</param>
        /// <returns>A string containing the formatted customer information if found; otherwise, an error message.</returns>
        public string viewCustomer(int customerID, out string errorMsg)
        {
            string s = "";
            errorMsg = "";
            Customer customer = findCustomer(customerID, out int index);
            if (customer != null)
            {
                s += generateHeadersString();
                s += customer.getInfo(columnWidth);
            }
            else { errorMsg = $"The customer with id - {customerID} is not found!"; }
            return s;
        }
        /// <summary>
        /// Views a list of all customers stored in the system.
        /// </summary>
        /// <param name="errorMsg">An output parameter that contains an error message if no customers are found.</param>
        /// <returns>A string containing the list of all customers; otherwise, an error message if no customers exist.</returns>
        public string viewAllCustomers(out string errorMsg)
        {
            string s = "";
            errorMsg = "";
            if (customerNum != 0)
            {
                s += generateHeadersString();
                for (int i = 0; i < customerNum; i++)
                {
                    s += customerList[i].getInfo(columnWidth) + "\n";
                }
            }
            else { errorMsg = "There is no customers to display.\nPlease add customer first."; }
            return s;
        }
        /// <summary>
        /// Checks whether a customer with the same first name, last name, and phone number already exists.
        /// </summary>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="phone">The phone number of the customer.</param>
        /// <returns>True if the customer is unique; otherwise, false.</returns>
        private bool checkCustomerUniquenss(string firstName, string lastName, string phone)
        {
            for (int i = 0; i < customerNum; i++)
            {
                if (customerList[i].getFirstName() == firstName && customerList[i].getLastName() == lastName && customerList[i].getPhone() == phone)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Checks and updates the maximum column widths based on the provided customer data.
        /// This ensures the table displays data correctly with sufficient padding.
        /// </summary>
        /// <param name="customer">The customer whose data is used to update the column widths.</param>
        private void checkWidth(Customer customer)
        {
            int[] currentWidth = customer.getWidth();
            for (int i = 0; i < currentWidth.Length; i++)
            {
                columnWidth[i] = Math.Max(columnWidth[i], currentWidth[i]);
            }
        }
        /// <summary>
        /// Initializes the column widths based on the headers of the customer data table.
        /// </summary>
        /// <returns>An array of integers representing the initial column widths based on the header lengths.</returns>
        private static int[] initializeWidth()
        {
            string[] headers = Customer.getHeaders();
            int[] returnValue = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                returnValue[i] = headers[i].Length;
            }
            return returnValue;
        }
        /// <summary>
        /// Generates a formatted string of column headers for the customer table, aligned based on the maximum column widths.
        /// </summary>
        /// <returns>A formatted string containing the headers for the customer table.</returns>
        private string generateHeadersString()
        {
            string s = "";
            string[] headers = Customer.getHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                s += headers[i].PadRight(columnWidth[i] + 5);
            }
            s += "\n" + new string('-', 100) + "\n";
            return s;
        }
        /// <summary>
        /// Gets the list of all customers.
        /// This method is typically used when save customer data to a file.
        /// </summary>
        /// <returns>An array containing all the customers in the system.</returns>
        public Customer[] getCustomerList() {  return customerList; }
        /// <summary>
        /// Gets the current number of customers stored in the system.
        /// This method is typically used when save customer data to a file.
        /// </summary>
        /// <returns>The number of customers currently stored in the system.</returns>
        public int getCustomerNum() {  return customerNum; }
        /// <summary>
        /// Gets the current customer ID counter.
        /// This method is typically used when save customer data to a file.
        /// </summary>
        /// <returns>The next available customer ID.</returns>
        public int getCounter() { return idCounter; }
    }
}
