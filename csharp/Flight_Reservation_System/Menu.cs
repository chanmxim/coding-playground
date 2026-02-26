using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Reservation_System
{
    static class Menu
    {
        private static string[] mainMenu = { "Customers", "Flights", "Bookings", "Exit" };
        private static string[] customerSubMenu = { "Add Customer", "View a particular Customer", "View All Customers", "Delete Customer", "Return to main menu" };
        private static string[] flightSubMenu = { "Add Flight", "View a particular Flight", "View All Flights", "Delete Flight", "Return to main menu" };
        private static string[] bookingSubMenu = { "Make Booking", "View a particular Booking", "View All Bookings", "Delete Booking", "Return to main menu" };
        private static int breakCounter = 4;
        private static AirlineCoordinator airCoord;
        /// <summary>
        /// The main entry point for the application, presenting the user with the main menu and handling navigation to submenus (customers, flights, bookings).
        /// </summary>
        public static void launch(AirlineCoordinator airlineCoordinator)
        {
            airCoord = airlineCoordinator;
            int userChoice;
            bool isBreak = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(new string('-', 10) + " Main menu " + new string('-', 10));
                showMenu(mainMenu);
                userChoice = validateUserInput(1, mainMenu.Length, $"Please choose option between 1 and {mainMenu.Length}") - 1;
                switch (userChoice)
                {
                    case 0:
                        customersSubMenu(); //customers sub menu
                        break;
                    case 1:
                        flightsSubMenu(); // flights sub menu
                        break;
                    case 2:
                        bookingsSubMenu(); // booking sub menu
                        break;
                    case 3:
                        isBreak = true; //exit
                        break;
                    default:
                        break;
                }
                if (isBreak) { break; }
            }// while loop
        }
        /// <summary>
        /// Displays the booking menu, allowing the user to make, view, delete, or list bookings.
        /// </summary>
        public static void bookingsSubMenu()
        {
            int userChoice;
            bool isBreak = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(new string('-', 10) + " Booking menu " + new string('-', 10));
                showMenu(bookingSubMenu);
                userChoice = validateUserInput(1, bookingSubMenu.Length, $"Please choose option between 1 and {bookingSubMenu.Length}") - 1;
                switch (userChoice)
                {
                    case 0://Make Booking
                        addBooking();
                        break;
                    case 1:// View a particular Booking
                        viewBooking();
                        break;
                    case 2: // View All Bookings
                        viewAllBookings();
                        break;
                    case 3: // Delete Booking
                        deleteBooking();
                        break;
                    case 4:
                        isBreak = true; //Return to main menu
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        break;
                }
                Console.WriteLine(new string('-', 5) + "Please press any key to continue." + new string('-', 5));
                Console.ReadKey();
                if (isBreak) { break; }
            }// while loop
        }
        /// <summary>
        /// Prompts the user to add a booking by selecting a customer and a flight, then attempts to add the booking.
        /// </summary>
        public static void addBooking()
        {
            string errorMsg;
            int index;

            Console.Clear();
            // show all customers to let user choose from
            viewAllCustomers();
            //find chosen customer
            Customer customer = airCoord.findCustomer(validateUserInput(0, "Please enter Customer id."), out index);
            if (customer != null)
            {
                Console.Clear();
                // show all flights to let user choose from
                viewAllFlights();
                // find chosen flight
                Flight flight = airCoord.findFlight(validateUserInput(0, "Please enter Flight number."), out index);
                if (flight != null)
                {
                    // try to add booking
                    if (airCoord.addBooking(customer, flight, out errorMsg))
                    {
                        Console.WriteLine("Booking successfully added.");
                    }
                    else { Console.WriteLine(errorMsg); } // print error message
                }
                else { Console.WriteLine("Flight is not found."); } // print error message
            }
            else { Console.WriteLine("Customer is not found."); } // print error message
        }
        /// <summary>
        /// Prompts the user to enter a booking number and displays the details of that booking if found.
        /// </summary>
        public static void viewBooking()
        {
            string errorMsg, outputString;
            outputString = (airCoord.viewBooking(validateUserInput(0, "Please enter booking number."), out errorMsg));
            if (outputString != "")
            {
                Console.Clear();
                Console.WriteLine("\n" + new string('-', 30) + " Booking " + new string('-', 30));
                Console.WriteLine(outputString);
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Displays all bookings in the system.
        /// </summary>
        public static void viewAllBookings()
        {

            string errorMsg, outputString;
            outputString = (airCoord.viewAllBookings(out errorMsg));
            if (outputString != "")
            {
                Console.WriteLine("\n" + new string('-', 30) + " All Bookings " + new string('-', 30));
                Console.WriteLine(outputString);
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Prompts the user to enter a booking ID and deletes the corresponding booking if found.
        /// </summary>
        public static void deleteBooking()
        {
            string errorMsg;
            viewAllBookings();
            if (airCoord.deleteBooking(validateUserInput(0, "Please enter Booking id."), out errorMsg))
            {
                Console.WriteLine("Booking successfully deleted.");
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Displays the flight menu, allowing the user to add, view, delete, or list flights.
        /// </summary>
        public static void flightsSubMenu()
        {
            int userChoice;
            bool isBreak = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(new string('-', 10) + " Flight menu " + new string('-', 10));
                showMenu(flightSubMenu);
                userChoice = validateUserInput(1, flightSubMenu.Length, $"Please choose option between 1 and {flightSubMenu.Length}") - 1;
                switch (userChoice)
                {
                    case 0://Add Flight
                        addFlight();
                        break;
                    case 1:// View a particular Flight
                        viewFlight();
                        break;
                    case 2: // View All Flights
                        viewAllFlights();
                        break;
                    case 3: // Delete Flight
                        deleteFlight();
                        break;
                    case 4:
                        isBreak = true; //Return to main menu
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        break;
                }
                Console.WriteLine(new string('-', 5) + "Please press any key to continue." + new string('-', 5));
                Console.ReadKey();
                if (isBreak) { break; }
            }// while loop
        }
        /// <summary>
        /// Prompts the user to input flight details and attempts to add a flight to the system.
        /// </summary>
        public static void addFlight()
        {
            string errorMsg;
            if (airCoord.addFlight(validateUserInput(0, "Please enter Flight number.")
                            , validateUserInput("Please enter flight origin")
                            , validateUserInput("Please enter flight destination")
                            , validateUserInput(0, "Please enter maximum number of seats.")
                            , out errorMsg))
            {
                Console.WriteLine("Flight successfully added.");
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Prompts the user to enter a flight number and displays the details of that flight, including a list of customers who have booked the flight.
        /// </summary>
        public static void viewFlight()
        {
            string errorMsg, outputString;
            viewAllFlights();
            int userInput = validateUserInput(0, "Please enter Flight number.");
            outputString = (airCoord.viewFlight(userInput, out errorMsg));
            if (outputString != "")
            {
                Console.Clear();
                Console.WriteLine("\n" + new string('-', 30) + " Flight " + new string('-', 30));
                Console.WriteLine(outputString);
                outputString = airCoord.viewAllCustomersForBookedFlight(userInput, out errorMsg);
                if (outputString != "")
                {
                    Console.WriteLine("\n" + new string('-', 30) + " Flight Passangers " + new string('-', 30));
                    Console.WriteLine(outputString);
                }
                else { Console.WriteLine(errorMsg); }
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Displays all the flights in the system.
        /// </summary>
        public static void viewAllFlights()
        {
            string errorMsg, outputString;
            outputString = (airCoord.viewAllFlights(out errorMsg));
            if (outputString != "")
            {
                Console.WriteLine("\n" + new string('-', 30) + " All Flights " + new string('-', 30));
                Console.WriteLine(outputString);
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Prompts the user to enter a flight ID and deletes the corresponding flight from the system.
        /// </summary>
        public static void deleteFlight()
        {
            string errorMsg;
            viewAllFlights();
            if (airCoord.deleteFlight(validateUserInput(0, "Please enter Customer id."), out errorMsg))
            {
                Console.WriteLine("Customer successfully deleted.");
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Displays the customer menu and handles the user choice to perform actions such as adding, viewing, deleting, or listing customers.
        /// </summary>
        public static void customersSubMenu()
        {
            int userChoice;
            bool isBreak = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(new string('-', 10) + " Customer menu " + new string('-', 10));
                showMenu(customerSubMenu);
                userChoice = validateUserInput(1, customerSubMenu.Length, $"Please choose option between 1 and {customerSubMenu.Length}") - 1;
                switch (userChoice)
                {
                    case 0: // Add Customer
                        addCustomer();
                        break;
                    case 1:// View a particular Customer
                        viewCustomer();
                        break;
                    case 2: // View All Customers
                        viewAllCustomers();
                        break;
                    case 3: // Delete Customer
                        deleteCustomer();
                        break;
                    case 4:
                        isBreak = true; //Return to main menu
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        break;
                }
                Console.WriteLine(new string('-', 5) + "Please press any key to continue." + new string('-', 5));
                Console.ReadKey();
                if (isBreak) { break; }
            }// while loop
        }
        /// <summary>
        /// Prompts the user to input customer details and adds the customer to the system.
        /// </summary>
        public static void addCustomer()
        {
            string errorMsg;
            if (airCoord.addCustomer(validateUserInput("Please enter first name:")
                            , validateUserInput("Please enter last name:")
                            , validateUserInput("Please enter phone number:")
                            , out errorMsg))
            {
                Console.WriteLine("Customer successfully added.");
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Prompts the user to enter a customer ID and displays the details of that customer if found.
        /// </summary>
        public static void viewCustomer()
        {
            string errorMsg, outputString;
            outputString = (airCoord.viewCustomer(validateUserInput(0, "Please enter Customer id."), out errorMsg));
            if (outputString != "")
            {
                Console.Clear();
                Console.WriteLine("\n" + new string('-', 30) + " Customer " + new string('-', 30));
                Console.WriteLine(outputString);
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Displays all customers in the system.
        /// </summary>
        public static void viewAllCustomers()
        {
            string errorMsg, outputString;
            outputString = (airCoord.viewAllCustomers(out errorMsg));
            if (outputString != "")
            {
                Console.WriteLine("\n" + new string('-', 30) + " All Customers " + new string('-', 30));
                Console.WriteLine(outputString);
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Prompts the user to enter a customer ID and deletes the corresponding customer from the system.
        /// </summary>
        public static void deleteCustomer()
        {
            string errorMsg;
            viewAllCustomers();
            if (airCoord.deleteCustomer(validateUserInput(0, "Please enter Customer id."), out errorMsg))
            {
                Console.WriteLine("Customer successfully deleted.");
            }
            else { Console.WriteLine(errorMsg); }
        }
        /// <summary>
        /// Displays a list of menu options to the user.
        /// </summary>
        /// <param name="menu"></param>
        public static void showMenu(string[] menu)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {menu[i]}");
            }
        }
        /// <summary>
        /// Validates the user input ensuring that it is within the specified range (inclusive).
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int validateUserInput(int min, int max, string msg)
        {
            int count = 0;
            while (++count < breakCounter) //let the user to escape loop when reaching setted attempts number
            {
                Console.WriteLine(msg);
                if (int.TryParse(Console.ReadLine(), out int userInput))
                {
                    if (userInput >= min && userInput <= max)
                    {
                        return userInput;
                    }
                    else { Console.WriteLine($"Input is out of range. Please enter a number between {min} and {max}"); }
                }
                else { Console.WriteLine("Invalid input. Please try again."); }
            }
            Console.WriteLine("Reached maximim attempt number to input. Please try again.");
            return -1;
        }
        /// <summary>
        /// Validates the user input ensuring that it is greater than or equal to the specified minimum value.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int validateUserInput(int min, string msg)
        {
            int count = 0;
            while (++count < breakCounter) //let the user to escape loop when reaching setted attempts number
            {
                Console.WriteLine(msg);
                if (int.TryParse(Console.ReadLine(), out int userInput))
                {
                    if (userInput >= min)
                    {
                        return userInput;
                    }
                    else { Console.WriteLine($"Input is out of range. Please enter a number greater than {min}."); }
                }
                else { Console.WriteLine("Invalid input. Please try again."); }
            }
            Console.WriteLine("Reached maximim attempt number to input. Please try again.");
            return -1;
        }
        /// <summary>
        /// Validates the user input ensuring that it is not null or empty.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string validateUserInput(string msg)
        {
            while (true)
            {
                Console.WriteLine(msg);
                string s = Console.ReadLine();
                if (s != null && s != "")
                {
                    return s;
                }
                else { Console.WriteLine("Invalid input. Please try again."); }
            }
        }
    }
}

