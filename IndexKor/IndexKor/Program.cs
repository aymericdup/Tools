using System;

namespace IndexKor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string webPageContent = WebTools.HtmlContent.WebPageContent.GetWebPageContent("https://finance.yahoo.com/q/cp?s=%5EFCHI");
                Console.Write(webPageContent);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
