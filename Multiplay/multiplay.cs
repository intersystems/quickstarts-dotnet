using System;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;
using InterSystems.XEP;
using System.Collections.Generic;

namespace myApp
{
    class multiplay
    {
        static void Main(string[] args)
        {
            String host = "localhost";
            int port = 51777;
            String username = "SuperUser";
            String password = "SYS";
            String Namespace = "USER";
            String className = "myApp.StockInfo";

            try
            {
                // Connect to database using EventPersister
                EventPersister xepPersister = PersisterFactory.CreatePersister();
                xepPersister.Connect(host, port, Namespace, username, password);
                Console.WriteLine("Connected to InterSystems IRIS.");
                xepPersister.DeleteExtent(className);   // remove old test data
                xepPersister.ImportSchema(className);   // import flat schema

                // Create Event
                Event xepEvent = xepPersister.GetEvent(className);
                IRISADOConnection connection = (IRISADOConnection)xepPersister.GetAdoNetConnection();
                IRIS native = IRIS.CreateIRIS(connection);

                // Task 2
                // Uncomment the line below to run task 2
                // Task2(connection);

                // Task 3
                // Uncomment the line below to run task 3
                // Task3(connection, xepEvent);

                // Task 4
                // Comment out Task 2, Task 3 and uncomment the line below to run task 4
                Task4(connection, native, xepEvent);

                xepEvent.Close();
                xepPersister.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Interactive prompt failed:\n" + e);
            }
        }

        public static void Task2(IRISADOConnection connection)
        {
            String sql = "SELECT distinct name FROM demo.stock";
            IRISCommand cmd = new IRISCommand(sql, connection);
            IRISDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[reader.GetOrdinal("Name")]);
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
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

                //generate mission and founder names (Native API)
                stock.founder = "test founder";
                stock.mission = "some mission statement";
                Console.WriteLine("Adding object with name " + stock.name + " founder " + stock.founder + " and mission " + stock.mission);
                array.Add(stock);
            }
            xepEvent.Store(array.ToArray());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

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

                //generate mission and founder names (Native API)
                stock.founder = native.ClassMethodString("%PopulateUtils", "Name");
                stock.mission = native.ClassMethodString("%PopulateUtils", "Mission");
                Console.WriteLine("Adding object with name " + stock.name + " founder " + stock.founder + " and mission " + stock.mission);
                array.Add(stock);
            }
            xepEvent.Store(array.ToArray());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}