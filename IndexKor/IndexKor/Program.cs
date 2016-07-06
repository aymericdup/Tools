using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IndexKor
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch(); sw.Start();
            IList<Stock> stocks = NasaqIndex.GetStockComponents();
            stocks.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "nasdaq-stocks.csv"));
            sw.Stop();
            Console.WriteLine(string.Format("Operation done in {0}", sw.Elapsed.ToString("hh':'mm':'ss'.'ffff")));
            Console.ReadLine();
        }
    }    
}

