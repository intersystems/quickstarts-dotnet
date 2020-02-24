/*
* PURPOSE: This class is being used by multiplay.cs class to generate list of stocks
* This defines information about stock companies 
*/

using System;

namespace myApp
{
    public class StockInfo
    {
        public String name;
        public String mission;
        public String founder;

        // Constructors
        public StockInfo() { }
        public StockInfo(String n, String m, String f)
        {
            name = n;
            mission = m;
            founder = f;
        }
    }
}