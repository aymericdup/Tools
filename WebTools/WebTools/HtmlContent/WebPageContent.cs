using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebTools.HtmlContent
{
    public static class WebPageContent
    {
        static private string GetPlainText(string url)
        {
            StringBuilder sb = new StringBuilder();
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Url = new Uri(url);
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            webBrowser.Document.ExecCommand("SelectAll", false, null);
            webBrowser.Document.ExecCommand("Copy", false, null);
            webBrowser.Dispose();
            return Clipboard.GetText();
        }

        static public string GetWebPageContent(string url, bool isThreadSafeMethodNeeded = true)
        {
            string content = string.Empty;
            Thread thread = new Thread(() =>
            {
                content = GetPlainText(url);
            });
            if (isThreadSafeMethodNeeded)
                thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return content;
        }
    }
}
