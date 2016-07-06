using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using IQFeedWrapper;
using IQFeedWrapper.SymbolLookup;

namespace IndexKor
{
    public struct Stock
    {
        public string Mnemo { get; set; }
        public string Name { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3}", Mnemo, Name, Sector, Industry);
        }
    }

    public static class NasaqIndex
    {
        public static void Save(this IList<Stock> stocks, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Mnemo;Name;Sector;Industry");
                foreach (Stock stock in stocks)
                    writer.WriteLine(stock.ToString());
            }
        }
        public static List<Stock> GetStockComponents()
        {
            IQFeedService.Service.InitializeSocket();
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
            List<Stock> stocks = new List<Stock>();
            string[] markets = new string[] { "1", "2", "21" };
            // retrieve all stock from the NGM, Nasdaq Global Market, code: 1
            stocks.AddRange(GetStockComponentsFromMarket(alphabet, markets[0]));
            // retrieve all stock from the NGM, Nasdaq Global Market, code: 2
            stocks.AddRange(GetStockComponentsFromMarket(alphabet, markets[1]));
            // retrieve all stock from the NGM, Nasdaq Global Market, code: 21
            stocks.AddRange(GetStockComponentsFromMarket(alphabet, markets[2]));
            stocks = stocks.OrderBy(x => x.Mnemo).ToList();
            // retrieve all sector and Industry from yahoo finance
            //for(int i = 0; i < stocks.Count; i++)
            //{
            //    Stock stock = stocks[i];
            //    Tuple<string, string> result = GetStockSectorAndIndustry(stock.Mnemo);
            //    if (result == null) continue;
            //    stock.Sector = result.Item1; stock.Industry = result.Item2;
            //    stocks[i] = stock;
            //}
            stocks.FillSectorAndIndustryFromNasdaqWebsite();
            return stocks;
        }

        private static IList<Stock> GetStockComponentsFromMarket(char[] alphabet, string market)
        {
            List<Stock> stocks = new List<Stock>();
            for (int i = 0; i < alphabet.Length; i++)
            {
                string[] results = IQFeedService.Service.GetData(new SymbolByFilterLookupRequest(alphabet[i].ToString(), FieldToSeachSymbolLookup.DESCRIPTION, FilterTypeSymbolLookup.LISTED_MARKET, market));
                foreach (string result in results)
                {
                    if (string.IsNullOrEmpty(result) || result.Contains("ENDMSG"))
                        continue;

                    string[] fields = result.Split(',');
                    if (fields[2] != "1") continue;
                    string mnemo = fields[0], name = fields[3].Trim();
                    if (!stocks.Any(x => x.Mnemo == mnemo))
                        stocks.Add(new Stock() { Name = name, Mnemo = mnemo});
                }
            }
            return stocks;
        }

        private static Tuple<string, string> GetStockSectorAndIndustry(string mnemo)
        {
            var task = WebTools.HtmlContent.MessageLoopWorker.Run(WebTools.HtmlContent.WebPageContent.GetWebPageContent,
                    string.Format("https://finance.yahoo.com/q/in?s={0}+Industry", mnemo));
            task.Wait();
            string webPageContent = task.Result.ToString();
            if (string.IsNullOrEmpty(webPageContent))
                return null;
            string[] rows = Regex.Split(webPageContent, "\n");
            bool hasSector = false, hasIndustry = false;
            string sector = string.Empty, industry = string.Empty;

            for(int i = 0; i < rows.Length && !(hasSector && hasIndustry); i++)
            {
                if (rows[i].StartsWith("Sector:"))
                { 
                    sector = rows[i].Replace("Sector:","").Replace("\t", "").Replace("\r", "").Trim(); hasSector = true;
                }
                else if(rows[i].StartsWith("Industry:") && !rows[i].Contains("Get Industry"))
                {
                    industry = rows[i].Replace("Industry:", "").Replace("\t","").Replace("\r","").Trim(); hasIndustry = true;
                }
            }

            return new Tuple<string, string>(sector, industry);
        }

        public static void FillSectorAndIndustryFromNasdaqWebsite(this List<Stock> stocks)
        {
            string nasdaqFile = WebTools.HtmlContent.WebPageContent.GetFileContentFromWeb("http://www.nasdaq.com/screening/companies-by-industry.aspx?exchange=NASDAQ&render=download");
            if (string.IsNullOrEmpty(nasdaqFile)) return;

            string[] rows = Regex.Split(nasdaqFile, Environment.NewLine);
            rows = rows.Skip(1).Take(rows.Length - 1).ToArray();
            Dictionary<string, Stock> stocksNasdaq = new Dictionary<string, Stock>();
            foreach(string row in rows)
            {
                if (string.IsNullOrEmpty(row))
                    continue;

                string[] fields = Regex.Split(row, "\",\"");
                Stock stock = new Stock() { Mnemo = fields[0].Trim().Replace("\"",""), Name = fields[1].Trim().Replace("\"", ""), Sector = fields[6].Trim().Replace("\"", ""), Industry = fields[7].Trim().Replace("\"", "") };
                if (stocksNasdaq.ContainsKey(stock.Mnemo))
                    continue;
                stocksNasdaq.Add(stock.Mnemo, stock);
            }

            if (stocksNasdaq.Count == 0) return;

            for (int i = 0; i < stocks.Count; i++)
            {
                Stock stock = stocks[i];
                if (!stocksNasdaq.ContainsKey(stock.Mnemo))
                    continue;
                Stock stockNasdaq = stocksNasdaq[stock.Mnemo];
                if (stockNasdaq.Sector == "n/a" && stockNasdaq.Industry == "n/a")
                    continue;
                stock.Sector = stockNasdaq.Sector; stock.Industry = stockNasdaq.Industry;
                stocks[i] = stock;
            }
        }
    }
}
