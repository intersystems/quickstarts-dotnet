/*
* PURPOSE: Makes a connection to an instance of InterSystems IRIS Data Platform with Entity Framework
* to simulates real-time stock trades:
*
* NOTES: When running,
*      1. Choose option 1 to create a sample trade.
*      2. Choose option 2 to save this trade
*      3. Choose option 3 to generate 1000 generic trades using XEP
*      4. Choose option 4 to view all trades
*      5. Choose option 5 to generate 1000 generic trades using ADO.NET. Notes that it much slower compare to XEP
*      6. Choose option 6 to update all trades
*/

using System;
using System.Data;
using System.Collections.Generic;
using InterSystems.Data.IRISClient;
using InterSystems.XEP;

namespace myApp
{
    class xepplaystocks
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Trade[] sampleArray = null;

            // Initialize dictionary to store connection details from config.txt
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary = getConnectionsDetails();

            // Retrieve connection information from configuration file
            string ip = dictionary["ip"];
            int port = Convert.ToInt32(dictionary["port"]);
            string Namespace = dictionary["Namespace"];
            string username = dictionary["username"];
            string password = dictionary["password"];
            string className = "myApp.Trade";

            try
            {
                // Connect to database using EventPersister
                EventPersister xepPersister = PersisterFactory.CreatePersister();
                xepPersister.Connect(ip, port, Namespace, username, password);
                Console.WriteLine("Connected to InterSystems IRIS.");
                xepPersister.DeleteExtent(className);   // remove old test data
                xepPersister.ImportSchema(className);   // import flat schema

                // Create Event
                Event xepEvent = xepPersister.GetEvent(className);

                // Starting interactive prompt
                bool always = true;
                while (always)
                {
                    Console.WriteLine("1. Make a trade (do not save)");
                    Console.WriteLine("2. Confirm all trades");
                    Console.WriteLine("3. Generate and save multiple trades");
                    Console.WriteLine("4. ADO.NET Comparison - Create and save multiple trades");
                    Console.WriteLine("5. Retrieve all trades; show execution statistics");
                    Console.WriteLine("6. Update all trades; show execution statistics");
                    Console.WriteLine("7. Quit");
                    Console.WriteLine("What would you like to do? ");

                    String option = Console.ReadLine();
                    switch (option)
                    {

                        // Task 2
                        case "1":
                            sampleArray = Task2CreateTrade(sampleArray);
                            break;
                        case "2":
                            Task2SaveTrade(sampleArray, xepEvent);
                            sampleArray = null;
                            break;

                        // Task 3
                        case "3":
                            Task3(sampleArray, xepEvent);
                            break;

                        // Task 4
                        case "4":
                            Task4(sampleArray, xepPersister);
                            break;

                        // Task 5
                        case "5":
                            Task5(xepEvent);
                            break;

                        // Task 6
                        case "6":
                            Task6(xepEvent);
                            break;

                        case "7":
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

        // Task 2: Create a sample trade
        // Note: Make a trade on 2016-08-12 for $300/share, 2 shares, and your own name as the trader
        public static Trade[] Task2CreateTrade(Trade[] sampleArray)
        {
            //Create trade object
            Console.WriteLine("Stock name: ");
            String name = Console.ReadLine();

            Console.WriteLine("Date (YYYY-MM-DD): ");
            DateTime date;
            if (DateTime.TryParse(Console.ReadLine(), out date))
            {

            }
            else
            {
                Console.WriteLine("Invalid date format!");
            }

            Console.WriteLine("Price: ");
            String inputPrice = Console.ReadLine();
            double price;
            double.TryParse(inputPrice, out price);
            if (price <= 0)
            {
                Console.WriteLine("Price has to bigger than 0");
            }

            Console.WriteLine("Number of Shares: ");
            String inputShare = Console.ReadLine();
            int shares;
            Int32.TryParse(inputShare, out shares);
            if (shares <= 0)
            {
                Console.WriteLine("Number of Shares has to bigger than 0");
            }
            Console.WriteLine("Trader name: ");
            String traderName = Console.ReadLine();

            Trade[] tradeArray = CreateTrade(name, date, price, shares, traderName, sampleArray);
            return tradeArray;
        }

        // Task 2: Saving trade.
        public static void Task2SaveTrade(Trade[] sampleArray, Event xepEvent)
        {
            //Save trades
            Console.WriteLine("Saving trades...");
            if (sampleArray != null)
            {
                XEPSaveTrades(sampleArray, xepEvent);
            }
            else
            {
                Console.WriteLine("There is no new trade to save");
            }
        }

        // Task 3: Generate trades and save it to database using XEP.
        public static void Task3(Trade[] sampleArray, Event xepEvent)
        {
            Console.WriteLine("How many items do you want to generate? ");
            String inputNumber = Console.ReadLine();
            int number;
            Int32.TryParse(inputNumber, out number);
            if (number <= 0)
            {
                Console.WriteLine("Number of items has to bigger than 0");
            }
            // Get sample generated array to store
            sampleArray = Trade.generateSampleData(number);

            // Save generated trades
            long totalStore = XEPSaveTrades(sampleArray, xepEvent);
            Console.WriteLine("Execution time: " + totalStore + "ms");
        }

        // Task 4: Generate trades and save it to database using ADO.NET.
        public static void Task4(Trade[] sampleArray, EventPersister xepPersister)
        {
            Console.WriteLine("How many items to generate using ADO.NET? ");
            String inputNum = Console.ReadLine();
            int numberADO;
            Int32.TryParse(inputNum, out numberADO);
            if (numberADO <= 0)
            {
                Console.WriteLine("Number of items has to bigger than 0");
            }
            //Get sample generated array to store
            sampleArray = Trade.generateSampleData(numberADO);

            //Save generated trades using ADO
            long totalADOStore = StoreUsingADO(xepPersister, sampleArray);
            Console.WriteLine("Execution time: " + totalADOStore + " ms");
        }

        // Task 5: View all trades
        public static void Task5(Event xepEvent)
        {
            Console.WriteLine("Fetching all. Please wait...");
            long totalFetch = ViewAll(xepEvent);
            Console.WriteLine("Execution time: " + totalFetch + "ms");
        }

        // Task 6: Update all trades and view them
        public static void Task6(Event xepEvent)
        {
            Console.WriteLine("Fetching all. Please wait...");
            long totalFetch = ViewAllAfterUpdate(xepEvent);
            Console.WriteLine("Execution time: " + totalFetch + "ms");
        }

        // Create a sample trade and add it to the array
        public static Trade[] CreateTrade(String stockName, DateTime tDate, double price, int shares, String trader, Trade[] sampleArray)
        {
            Trade sampleObject = new Trade(stockName, tDate, price, shares, trader);
            Console.WriteLine("New Trade: " + shares + " shares of " + stockName + " purchased on date " + tDate.ToString() + " at price " + price + " by " + trader + ".");

            int currentSize = 0;
            int newSize = 1;
            if (sampleArray != null)
            {
                currentSize = sampleArray.Length;
                newSize = currentSize + 1;
            }

            Trade[] newArray = new Trade[newSize];
            for (int i = 0; i < currentSize; i++)
            {
                newArray[i] = sampleArray[i];
            }
            newArray[newSize - 1] = sampleObject;
            Console.WriteLine("Added " + stockName + " to the array. Contains " + newSize + " trade(s).");
            return newArray;
        }

        // Save trade into database using XEP
        public static long XEPSaveTrades(Trade[] sampleArray, Event xepEvent)
        {
            long startTime = DateTime.Now.Ticks; //To calculate execution time
            xepEvent.Store(sampleArray);
            long endtime = DateTime.Now.Ticks;
            Console.WriteLine("Saved " + sampleArray.Length + " trade(s).");
            return endtime - startTime;
        }

        // Save trade into database using ADO.NET - which is slower than using XEP
        public static long StoreUsingADO( Trade[] sampleArray, EventPersister persist)
        {
            long totalTime = new long();
            long startTime = DateTime.Now.Ticks;
            // Loop through objects to insert
            try
            {
                IRISDataAdapter da = new IRISDataAdapter();
                String ClassName = "myApp.Trade";

                IRISADOConnection con = (IRISADOConnection)persist.GetAdoNetConnection();

                String SQL = "select purchaseDate, purchasePrice, stockName, shares, traderName from " + ClassName;
                da.SelectCommand = con.CreateCommand();
                da.SelectCommand.CommandText = SQL;

                SQL = "INSERT INTO myApp.Trade (purchaseDate, purchasePrice, stockName, shares, traderName) VALUES (?,?,?,?,?)";

                IRISCommand cmd = con.CreateCommand();
                cmd.CommandText = SQL;
                da.InsertCommand = cmd;

                IRISParameter date_param = new IRISParameter("purchaseDate", IRISDbType.DateTime);
                cmd.Parameters.Add(date_param);
                date_param.SourceColumn = "purchaseDate";

                IRISParameter price_param = new IRISParameter("purchasePrice", IRISDbType.Double);
                cmd.Parameters.Add(price_param);
                price_param.SourceColumn = "purchasePrice";

                IRISParameter name_param = new IRISParameter("stockName", IRISDbType.NVarChar);
                cmd.Parameters.Add(name_param);
                name_param.SourceColumn = "stockName";

                IRISParameter shares_param = new IRISParameter("shares", IRISDbType.Int);
                cmd.Parameters.Add(shares_param);
                shares_param.SourceColumn = "shares";

                IRISParameter trader_param = new IRISParameter("traderName", IRISDbType.NVarChar);
                cmd.Parameters.Add(trader_param);
                trader_param.SourceColumn = "traderName";

                da.TableMappings.Add("Table", ClassName);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < sampleArray.Length; i++)
                {
                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["purchaseDate"] = sampleArray[i].purchaseDate;
                    newRow["purchasePrice"] = sampleArray[i].purchasePrice;
                    newRow["stockName"] = sampleArray[i].stockName;
                    newRow["shares"] = sampleArray[i].shares;
                    newRow["traderName"] = sampleArray[i].traderName;
                    ds.Tables[0].Rows.Add(newRow);
                }


                da.Update(ds);
                Console.WriteLine("Inserted " + sampleArray.Length + " item(s) via ADO.NET successfully.");
                totalTime = DateTime.Now.Ticks - startTime;
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem storing items using ADO.NET.\n" + e);
            }
            return totalTime / TimeSpan.TicksPerMillisecond;
        }

        // Iterate over all trades
        public static long ViewAll(Event xepEvent)
        {
            // Create and execute query using EventQuery
            String sqlQuery = "SELECT * FROM MyApp.Trade WHERE purchaseprice > ? ORDER BY stockname, purchaseDate";
            EventQuery<Trade> xepQuery = xepEvent.CreateQuery<Trade>(sqlQuery);
            xepQuery.AddParameter(0);    // find stocks purchased > $0/share (all)
            long startTime = DateTime.Now.Ticks;
            xepQuery.Execute();

            // Iterate through and write names of stocks using EventQueryIterator
            Trade trade = xepQuery.GetNext();
            while (trade != null)
            {
                Console.WriteLine(trade.stockName + "\t" + trade.purchasePrice + "\t" + trade.purchaseDate);
                trade = xepQuery.GetNext();
            }
            long totalTime = DateTime.Now.Ticks - startTime;
            xepQuery.Close();
            return totalTime / TimeSpan.TicksPerMillisecond;
        }

        // Update all trade and iterate over them.
        public static long ViewAllAfterUpdate(Event xepEvent)
        {
            // Create and execute query using EventQuery
            String sqlQuery = "SELECT * FROM myApp.Trade WHERE purchaseprice > ? ORDER BY stockname, purchaseDate";
            EventQuery<Trade> xepQuery = xepEvent.CreateQuery<Trade>(sqlQuery);
            xepQuery.AddParameter("0");    // find stocks purchased > $0/share (all)
            long startTime = DateTime.Now.Ticks;
            xepQuery.Execute();

            // Iterate through and write names of stocks using EventQueryIterator
            Trade trade = xepQuery.GetNext();
            while (trade != null)
            {
                trade.stockName = "NYSE-" + trade.stockName;
                xepQuery.UpdateCurrent(trade);
                Console.WriteLine(trade.stockName + "\t" + trade.purchasePrice + "\t" + trade.purchaseDate);
                trade = xepQuery.GetNext();
            }
            long totalTime = DateTime.Now.Ticks - startTime;
            xepQuery.Close();
            return totalTime / TimeSpan.TicksPerMillisecond;
        }

        static IDictionary<string, string> getConnectionsDetails()
        {
            return generateConfig("../connections.config");
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
                string[] info = line.Replace(" ", String.Empty).Replace(";", String.Empty).Split(':');
                // Check if line contains enough information
                if (info.Length >= 2)
                {
                    dictionary[info[0]] = info[1];
                }
            }
            return dictionary;
        }
    }
}