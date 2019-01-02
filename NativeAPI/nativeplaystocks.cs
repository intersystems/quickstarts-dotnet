/*
* PURPOSE: Makes a connection to an instance of InterSystems IRIS Data Platform with Native API
* to store stock data in a custom data structure.
*
* NOTES: When running,
*      1. Choose option 1 to test the global value
*      2. Choose option 2 to store the stock data in a custom structure.
*      3. Choose option 3 to retrieve and view the stock data from this custom structure.
*      4. Choose option 4 to call population methods within InterSystems IRIS to generate better information for trades
*      5. Choose option 5 to call InterSystems IRIS routine directly
*/

using System;
using System.Collections;
using System.Collections.Generic;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;

namespace myApp
{
    class nativeplaystocks
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Initialize dictionary to store connection details from config.txt
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary = generateConfig("..\\..\\..\\config.txt");

            // Retrieve connection information from configuration file
            string ip = dictionary["ip"];
            int port = Convert.ToInt32(dictionary["port"]);
            string Namespace = dictionary["namespace"];
            string username = dictionary["username"];
            string password = dictionary["password"];

            try
            {
                // Making connection using IRISConnecion
                IRISConnection connection = new IRISConnection();

                // Create connection string
                connection.ConnectionString = "Server = " + ip + "; Port = " + port + "; Namespace = " +
                                        Namespace + "; Password = " + password + "; User ID = " + username;
                connection.Open();
                Console.WriteLine("Connected to InterSystems IRIS.");

                IRIS irisNative = IRIS.CreateIRIS(connection);

                // Starting interactive prompt
                bool always = true;
                while (always)
                {
                    Console.WriteLine("1. Test");
                    Console.WriteLine("2. Store stock data");
                    Console.WriteLine("3. View stock data");
                    Console.WriteLine("4. Generate Trades");
                    Console.WriteLine("5. Call Routines");
                    Console.WriteLine("6. Quit");
                    Console.WriteLine("What would you like to do? ");

                    String option = Console.ReadLine();
                    switch (option)
                    {
                        // Task 1
                        case "1":
                            SetTestGlobal(irisNative);
                            break;

                        // Task 2
                        case "2":
                            StoreStockData(irisNative, connection);
                            break;

                        // Task 3
                        case "3":
                            Console.WriteLine("Printing nyse globals...");
                            long startPrint = DateTime.Now.Ticks; // To calculate execution time

                            // Iterate over all nodes
                            PrintNodes(irisNative, "nyse");
                            long totalPrint = DateTime.Now.Ticks - startPrint;
                            Console.WriteLine("Execution time: " + totalPrint/TimeSpan.TicksPerMillisecond + " ms");
                            break;

                        // Task 4
                        case "4":
                            GenerateData(irisNative, 10);
                            break;

                        // Task 5
                        case "5":
                            Console.WriteLine("on InterSystems IRIS version: " + irisNative.FunctionString("PrintVersion","^StocksUtil"));
                            break;

                        case "6":
                            Console.WriteLine("Exited.");
                            always = false;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Try again!");
                            break;
                    }
                }
                irisNative.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error - Exception thrown: " + e);
            }
        }

        // Task 1: Test Globals
        public static void SetTestGlobal(IRIS irisNative)
        {
            // Write to a test global
            irisNative.Set(8888, "^testglobal", "1");
            int globalValue = (int)irisNative.GetInt32("^testglobal", "1");
            Console.WriteLine("The value of ^testglobal(1) is " + globalValue);
        }

        // Task 2: Store the stock data in a custom structure
        public static void StoreStockData(IRIS irisNative, IRISConnection dbconnection)
        {
            // Clear global from previous runs
            irisNative.Kill("^nyse");
            Console.WriteLine("Storing stock data using Native API...");

            // Get stock data using ADO.NET and write global
            try
            {
                String sql = "select top 1000 TransDate, Name, StockClose, StockOpen, High, Low, Volume from Demo.Stock";
                IRISCommand cmd = new IRISCommand(sql, dbconnection);
                IRISDataReader reader = cmd.ExecuteReader();
                ArrayList list = new ArrayList();
                string result;

                while (reader.Read())
                {
                    DateTime dt = (DateTime)reader[reader.GetOrdinal("TransDate")];
                    result = (string)reader[reader.GetOrdinal("Name")] +
                                dt.ToString("MM/dd/yyyy") +
                                 reader[reader.GetOrdinal("High")] +
                                 reader[reader.GetOrdinal("Low")] +
                                 reader[reader.GetOrdinal("StockOpen")] +
                                reader[reader.GetOrdinal("StockClose")] +
                                (int)reader[reader.GetOrdinal("Volume")];
                    list.Add(result);
                }

                int id = list.Count;
                long startConsume = DateTime.Now.Ticks;

                for (int i = 0; i < id; i++)
                {
                    irisNative.Set(list[i], "^nyse", i + 1);
                }

                long totalConsume = DateTime.Now.Ticks - startConsume;
                Console.WriteLine("Stored natively successfully. Execution time: " + totalConsume / TimeSpan.TicksPerMillisecond + " ms");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error either retrieving data using ADO.NET or storing to globals: " + e);
            }
        }

        // Task 3: Retrieve and view the stock data from this custom structure.
        public static void PrintNodes(IRIS irisNative, String globalName)
        {
            Console.WriteLine("Iterating over " + globalName + " globals");

            // Iterate over all nodes forwards
            IRISIterator iter = irisNative.GetIRISIterator(globalName);
            Console.WriteLine("walk forwards");
            foreach (var v in iter)
            {
                Console.WriteLine("subscript=" + iter.CurrentSubscript + ", value=" + iter.Current);
            }
        }

        // Task 4: Call population methods within InterSystems IRIS to generate better information for trades
        public static Trade[] GenerateData(IRIS irisNative, int objectCount)
        {
            Trade[] data = new Trade[objectCount];
            try
            {

                for (int i = 0; i < objectCount; i++)
                {
                    DateTime tempDate = Convert.ToDateTime("2018-01-01");
                    double tempAmount = (double)irisNative.ClassMethodDouble("%PopulateUtils", "Currency");
                    String tempName = irisNative.ClassMethodString("%PopulateUtils", "String") +
                                            irisNative.ClassMethodString("%PopulateUtils", "String") +
                                            irisNative.ClassMethodString("%PopulateUtils", "String");
                    String tempTrader = irisNative.ClassMethodString("%PopulateUtils", "Name");
                    int tempShares = new Random().Next(1, 20);
                    data[i] = new Trade(tempName, tempDate, tempAmount, tempShares, tempTrader);
                    Console.WriteLine("New trade: " + tempName + " , " + tempDate + " , " + tempAmount + " , " + tempShares + " , " + tempTrader);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return data;
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