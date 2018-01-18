using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArbTools;
using HtmlAgilityPack;

namespace ArbCore.Parsing.Parsers
{
    public class BetclicParser : Parser
    {
        #region vars
        #endregion

        #region constructor(s)
        #endregion

        #region method(s)

        #region Parser implementation
        protected override List<BetParsingInformation> ReadFootballPageContent(string content)
        {
            string[] days = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
            string[] months = new string[] { "janvier", "février", "mars", "avril", "mai", "juin", "juillet", "août", "septembre", "octobre", "novembre", "décembre" };
            DateTime date = DateTime.MinValue, currentDate = DateTime.MinValue;
            int nbTeams = 2;
            List<BetParsingInformation> bets = new List<BetParsingInformation>();

            try
            {
                
                string[] lines = Regex.Split(content, "\r\n");
                int startingRowIdx = Array.FindIndex(lines, line => line.Contains("1 Nul 2")), size = lines.Length - startingRowIdx;
                string[] rows = new string[size];
                Array.Copy(lines, startingRowIdx, rows, 0, size);
                rows.Replace("1 Nul 2", string.Empty);                

                for (int i = 0; i < rows.Length; i++)
                {
                    string row = rows[i];
                    row = row.Replace("Plus de stats ", string.Empty);
                    if (row == " " || row == "paris") continue;
                    else if (days.Any(day => row.Contains(day))) // date
                    {
                        string[] dateStr = Regex.Split(row, " ");
                        int startDateFieldIdx = 0;
                        while (dateStr[startDateFieldIdx] == string.Empty) startDateFieldIdx++;
                        string monthStr = dateStr[startDateFieldIdx + 2].ToLower();
                        currentDate = date = new DateTime(DateTime.Today.Year, Array.FindIndex(months, month => month == monthStr) + 1, Convert.ToInt32(dateStr[startDateFieldIdx + 1]));
                    }

                    else if (row.Contains(":")) // time
                    {
                        date = currentDate.Add(TimeSpan.Parse(row));
                    }

                    else if (row.Contains("-")) // odds
                    {
                        string[] data = Regex.Split(row, " ");
                        int startTeamFieldIdx = 0;
                        while (data[startTeamFieldIdx] == string.Empty) startTeamFieldIdx++;

                        string[] teams = new string[nbTeams];
                        for (int k = 0; k < nbTeams; k++)
                        {
                            teams[k] = data[startTeamFieldIdx];
                            int endTeamFieldIdx = startTeamFieldIdx + 1;
                            while (data[endTeamFieldIdx] != "-" && !data[endTeamFieldIdx].Contains(","))
                            {
                                teams[k] = string.Format("{0} {1}", teams[k], data[endTeamFieldIdx]);
                                endTeamFieldIdx++;
                            }
                            startTeamFieldIdx = endTeamFieldIdx + 1;
                        }
                        startTeamFieldIdx--;
                        decimal win = Convert.ToDecimal(data[startTeamFieldIdx].Replace(",", ".")), draw = Convert.ToDecimal(data[startTeamFieldIdx + 1].Replace(",", ".")), loss = Convert.ToDecimal(data[startTeamFieldIdx + 2].Replace(",", "."));
                        bets.Add(new BetParsingInformation(Bet.BookMaker.BETCLIC, teams, date, new Bet.Odds(Bet.BookMaker.BETCLIC, new decimal[] { win, draw, loss })));
                    }

                    else // end stream
                        break;
                }

                return bets;
            }
            catch (Exception ex)
            {
                throw ex;
                //return bets;
            }
        }

        protected override List<BetParsingInformation> ReadFootballSourceCode(Stream stream)
        {
            List<BetParsingInformation> bets = new List<BetParsingInformation>();
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo() { NumberDecimalSeparator = "," };

            try
            {
                HtmlDocument document = new HtmlDocument();
                document.Load(stream);

                HtmlNode eventWrapperNode = document.GetElementbyId("event-wrapper");
                IEnumerable<HtmlNode> timeNodes = eventWrapperNode.Elements("entry day-entry grid-9 nm");
                for (int i = 0; i < eventWrapperNode.ChildNodes.Count; i++)
                {
                    HtmlNode dayNode = eventWrapperNode.ChildNodes[i];
                    if (!dayNode.HasAttributes) continue;
                    HtmlAttribute classAttribute = dayNode.Attributes["class"];
                    if (classAttribute == null) continue;

                    if(classAttribute.Value == "entry day-entry grid-9 nm")
                    {
                        string dateStr = dayNode.Attributes["data-date"].Value;
                        DateTime date = DateTime.Parse(dateStr);

                        foreach(HtmlNode timeNode in dayNode.ChildNodes)
                        {
                            if (!timeNode.HasAttributes) continue;
                            HtmlAttribute nodeClassAttribute = timeNode.Attributes["class"];
                            if (nodeClassAttribute == null) continue;
                            if(nodeClassAttribute.Value == "schedule clearfix")
                            {
                                TimeSpan time = TimeSpan.Parse(timeNode.ChildNodes[1].InnerText);
                                DateTime datetime = date.Add(time);

                                foreach(HtmlNode matchNode in timeNode.ChildNodes)
                                {
                                    if (!matchNode.HasAttributes) continue;
                                    if(!matchNode.Id.Contains("match_")) continue;

                                    string evt = matchNode.Attributes["data-track-event-name"].Value;
                                    string[] participants = Regex.Split(evt, "-");
                                    participants.Trim();

                                    string url = string.Empty;
                                    foreach (HtmlNode fieldMatchNode in matchNode.ChildNodes)
                                    {
                                        if (!fieldMatchNode.HasAttributes) continue;
                                        HtmlAttribute fieldMatchNodeAttribute = fieldMatchNode.Attributes["class"];
                                        if (fieldMatchNodeAttribute == null) continue;
                                        
                                        if (fieldMatchNodeAttribute.Value == "match-name")
                                        {
                                            url = fieldMatchNode.ChildNodes["a"].Attributes["href"].Value;
                                        }
                                        else if(fieldMatchNodeAttribute.Value == "match-odds")
                                        {
                                            List<decimal> odds = new List<decimal>();
                                            foreach(HtmlNode oddNode in fieldMatchNode.ChildNodes)
                                            {
                                                if (!oddNode.HasAttributes) continue;
                                                HtmlAttribute oddNodeAttribute = oddNode.Attributes["class"];
                                                if (oddNodeAttribute == null) continue;
                                                if (oddNodeAttribute.Value == "match-odd")
                                                    odds.Add(decimal.Parse(oddNode.ChildNodes["span"].InnerText, numberFormatInfo));
                                            }

                                            bets.Add(new BetParsingInformation(Bet.BookMaker.BETCLIC, participants, datetime, new Bet.Odds(Bet.BookMaker.BETCLIC, odds.ToArray()), url));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    
                }


                //IEnumerable <HtmlNode> nodes = document.DocumentNode.Descendants("div").Where(div => div.Id.Contains("match_") || div.Id.Contains("entry day-entry grid-9 nm"));

                //foreach (HtmlNode node in nodes)
                //{
                //    if (node.Id.Contains("match_"))
                //    {
                //        string evt = node.Attributes["data-track-event-name"].Value;
                //        string link = node.ChildNodes[1].ChildNodes[1].Attributes["href"].Value;
                //        HtmlNode oddNodes = node.ChildNodes[3];
                //        List<string> odds = new List<string>();
                //        for (int i = 1; i < oddNodes.ChildNodes.Count; i = i + 2)
                //            odds.Add(oddNodes.ChildNodes[i].ChildNodes[1].InnerText);
                //    }
                //    else
                //    {
                //        string sport = node.Attributes["data_track_sport_name"].Value;
                //        string competition = node.Attributes["data_track_competition_name"].Value;

                //        string date = node.ChildNodes[3].Attributes["data-date"].Value;
                //    }
                //}
                return bets;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion
    }
}
