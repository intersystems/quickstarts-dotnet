/*
* PURPOSE: Makes a connection to an instance of InterSystems IRIS Data Platform using ADO.NET, XEP, and Native API side-by-side
* to store stock company information.
* In short:
*   ADO.NET is used to quickly retrieve all distinct stock names from the Demo.Stock table.
*   Native API is used to call population methods within InterSystems IRIS for founder and mission statement.
*   XEP is used to store these objects directly to the database, avoiding any translation back to tables.
*
*/

using System;
using System.Collections.Generic;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;
using InterSystems.XEP;

namespace myApp
{
    class multiplay
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Initialize dictionary to store connection details from config.txt
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary = generateConfig("../iris-server-config.txt");

            // Retrieve connection information from configuration file
            string ip = dictionary["ip"];
            int port = Convert.ToInt32(dictionary["port"]);
            string Namespace = dictionary["namespace"];
            string username = dictionary["username"];
            string password = dictionary["password"];
            String className = "myApp.StockInfo";

            try
            {
                // Connect to database using EventPersister
                EventPersister xepPersister = PersisterFactory.CreatePersister();
                xepPersister.Connect(ip, port, Namespace, username, password);
                Console.WriteLine("Connected to InterSystems IRIS.");
                xepPersister.DeleteExtent(className);   // Remove old test data
                xepPersister.ImportSchema(className);   // Import flat schema

                // Create Event
                Event xepEvent = xepPersister.GetEvent(className);
                IRISADOConnection connection = (IRISADOConnection)xepPersister.GetAdoNetConnection();
                IRIS native = IRIS.CreateIRIS(connection);

                // Starting interactive prompt
                bool always = true;
                while (always) {
                    Console.WriteLine("1. Retrieve all stock names");
                    Console.WriteLine("2. Create objects");
                    Console.WriteLine("3. Populate properties");
                    Console.WriteLine("4. Quit");
                    Console.WriteLine("What would you like to do? ");

                    String option = Console.ReadLine();
                    switch (option) {
                    // Task 2
                    case "1":
                        Task2(connection);
                        break;

                    // Task 3
                    case "2":
                        Task3(connection, xepEvent);
                        break;

                    // Task 4
                    case "3":
                        Task4(connection, native, xepEvent);
                        break;

                    case "4":
                        Console.WriteLine("Exited.");
                        always = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Try again!");
                        break;
                    }
                }

                xepEvent.Close();
                xepPersister.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Interactive prompt failed:\n" + e);
            }
        }

        // Task 2: Query data using ADO.NET
        public static void Task2(IRISADOConnection connection)
        {
            String sql = "SELECT distinct name FROM demo.stock";
            IRISCommand cmd = new IRISCommand(sql, connection);
            IRISDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[reader.GetOrdinal("Name")]);
            }
        }

        // Task 3: Generate sample stock info objects and stored into database using XEP
        public static void Task3(IRISADOConnection connection, Event xepEvent)
        {
            String sql = "SELECT distinct name FROM demo.stock";
            IRISCommand cmd = new IRISCommand(sql, connection);
            IRISDataReader reader = cmd.ExecuteReader();

            var array = new List<StockInfo>();
            while (reader.Read())
            {
                StockInfo stock = new StockInfo();
                stock.name = (string)reader[reader.GetOrdinal("Name")];
                Console.WriteLine("created stockinfo array.");
                stock.founder = "test founder";
                stock.mission = "some mission statement";
                Console.WriteLine("Adding object with name " + stock.name + " founder " + stock.founder + " and mission " + stock.mission);
                array.Add(stock);
            }
            xepEvent.Store(array.ToArray());
        }

        // Task 4: Use Native API call population methods within InterSystems IRIS for founder and mission statement
        public static void Task4(IRISADOConnection connection, IRIS native, Event xepEvent)
        {
            String sql = "SELECT distinct name FROM demo.stock";
            IRISCommand cmd = new IRISCommand(sql, connection);
            IRISDataReader reader = cmd.ExecuteReader();

            var array = new List<StockInfo>();
            while (reader.Read())
            {
                StockInfo stock = new StockInfo();
                stock.name = (string)reader[reader.GetOrdinal("Name")];
                Console.WriteLine("created stockinfo array.");

                // Generate mission and founder names (Native API)
                stock.founder = native.ClassMethodString("%PopulateUtils", "Name");
                stock.mission = native.ClassMethodString("%PopulateUtils", "Mission");
                Console.WriteLine("Adding object with name " + stock.name + " founder " + stock.founder + " and mission " + stock.mission);
                array.Add(stock);
            }
            xepEvent.Store(array.ToArray());
        }

        // Helper method: Get connection details from config file
        static IDictionary<string, string> generateConfig(string filename)
        {
            // Initial empty dictionary to store connection details
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            // Iterate over all lines in configuration file
            string[] lines = System.IO.File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                string[] info = line.Replace(" ", String.Empty).Split(':');
                // Check if line contains enough information
                if (info.Length >= 2)
                {
                    dictionary[info[0]] = info[1];
                }
                else
                {
                    Console.WriteLine("Ignoring line: " + line);
                }
            }
            return dictionary;
        }
    }
}
