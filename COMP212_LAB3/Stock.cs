using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace COMP212_LAB3
{
    public class Stock
    {

        public string Symbol { get; set; }
        public string Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public Stock(string symbol, string date, double open, double high, double low, double close) 
        {
            this.Symbol = symbol;
            this.Date = date;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
        }
    }
}
