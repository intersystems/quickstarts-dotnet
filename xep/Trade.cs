using System;

namespace myApp
{
    class Trade
    {
        public String stockName;
        public DateTime purchaseDate;
        public double purchasePrice;
        public int shares;
        public String traderName;
        public Trade()
        {

        }

        public Trade(String stockName, DateTime purchaseDate, double purchasePrice, int shares, String traderName)
        {
            //super();
            this.stockName = stockName;
            this.purchaseDate = purchaseDate;
            this.purchasePrice = purchasePrice;
            this.shares = shares;
            this.traderName = traderName;
        }

        public static Trade[] generateSampleData(int objectCount)
        {
            Trade[] data = new Trade[objectCount];
            try
            {

                for (int i = 0; i < objectCount; i++)
                {
                    DateTime tempDate = Convert.ToDateTime("2018/01/01");
                    double tempPrice = 25;

                    data[i] = new Trade("XXX", tempDate, tempPrice, 5, "TestTrader");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Interactive prompt failed:\n" + e);
            }
            return data;
        }
    }
}