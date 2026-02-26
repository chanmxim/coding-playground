using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Xml.Serialization;

namespace Flight_Reservation_System
{
    class Program
    {
        private static AirlineCoordinator airCoord;

        public static void Main(string[] args)
        {
            loadData();
            Menu.launch(airCoord);
            saveData();
        } //main
        /// <summary>
        /// Saves the current data (bookings, customers, and flights) to the respective files.
        /// </summary>
        public static void saveData()
        {
            Utility.saveManager(airCoord.getBookingManager());
            Utility.saveManager(airCoord.getCustomerManager());
            Utility.saveManager(airCoord.getFlightManager());
        }
        /// <summary>
        /// Loads the data from the files into the application. If the data cannot be loaded, initializes a new set of managers with default sizes.
        /// </summary>
        public static void loadData()
        {
            airCoord = Utility.loadDataFromFiles();
            if (airCoord == null)
            {
                airCoord = new AirlineCoordinator(100, 100, 100);
            }
        }
    }
}