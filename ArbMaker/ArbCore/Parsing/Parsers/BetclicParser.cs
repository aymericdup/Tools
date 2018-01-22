using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArbCore.Bet;
using ArbTools;
using HtmlAgilityPack;

namespace ArbCore.Parsing.Parsers
{
    public class BetclicParser : Parser
    {
        #region vars
        private Uri _BASE_URL_ = new Uri("https://www.betclic.fr");
        private NumberFormatInfo _numberFormatInfo  = new NumberFormatInfo() { NumberDecimalSeparator = "," };
        #endregion

        #region constructor(s)
        #endregion

        #region method(s)

        #region Parser implementation
        protected override Odds ReadDetailedFootballPageContent(Stream stream, Odds oldOdd)
        {
            Odds odd = new Odds();
            decimal[] newOdds = new decimal[Odds._NB_ODDS_];
            try
            {
                HtmlDocument document = new HtmlDocument();
                document.Load(stream);

                #region result
                HtmlNode matchResult = document.GetElementbyId("market_marketTypeCode_Ftb_Mr3");
                for (int i = 0; i < matchResult.ChildNodes.Count; i++)
                {
                    HtmlNode oddsMultiNode = matchResult.ChildNodes[i];
                    if (!oddsMultiNode.HasAttributes) continue;
                    HtmlAttribute classAttribute = oddsMultiNode.Attributes["class"];
                    if (oddsMultiNode == null) continue;

                    if(classAttribute.Value == "odds multi")
                    {
                        HtmlNode trChild = oddsMultiNode.ChildNodes["table"].ChildNodes["tbody"].ChildNodes["tr"];

                        decimal _1 = decimal.Parse(trChild.ChildNodes[1].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._1_] != _1) newOdds[Odds._1_] = oldOdd[Odds._1_] = _1;

                        decimal _X = decimal.Parse(trChild.ChildNodes[2].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._X_] != _X) newOdds[Odds._X_] = oldOdd[Odds._X_] = _X;

                        decimal _2 = decimal.Parse(trChild.ChildNodes[3].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._2_] != _2) newOdds[Odds._2_] = oldOdd[Odds._2_] = _2;
                        break;
                    }
                }
                #endregion

                #region double chance
                HtmlNode doucheChanceNode = document.GetElementbyId("market_marketTypeCode_68");
                for (int i = 0; i < doucheChanceNode.ChildNodes.Count; i++)
                {
                    HtmlNode oddsMultiNode = doucheChanceNode.ChildNodes[i];
                    if (!oddsMultiNode.HasAttributes) continue;
                    HtmlAttribute classAttribute = oddsMultiNode.Attributes["class"];
                    if (oddsMultiNode == null) continue;

                    if (classAttribute.Value == "odds multi")
                    {
                        #region 1X
                        HtmlNode trChild = oddsMultiNode.ChildNodes[1].ChildNodes["table"].ChildNodes["tbody"].ChildNodes["tr"];

                        decimal _1X = decimal.Parse(trChild.ChildNodes[3].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._1X_] != _1X) newOdds[Odds._1X_] = oldOdd[Odds._1X_] = _1X;

                        decimal _1X_HALF = decimal.Parse(trChild.ChildNodes[4].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._1X_Half_] != _1X_HALF) newOdds[Odds._1X_Half_] = oldOdd[Odds._1X_Half_] = _1X_HALF;
                        #endregion

                        #region 12
                        trChild = oddsMultiNode.ChildNodes[3].ChildNodes["table"].ChildNodes["tbody"].ChildNodes["tr"];

                        decimal _12 = decimal.Parse(trChild.ChildNodes[3].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._12_] != _12) newOdds[Odds._12_] = oldOdd[Odds._12_] = _12;

                        decimal _12_HALF = decimal.Parse(trChild.ChildNodes[4].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._12_Half_] != _12_HALF) newOdds[Odds._12_Half_] = oldOdd[Odds._12_Half_] = _12_HALF;
                        #endregion

                        #region X2
                        trChild = oddsMultiNode.ChildNodes[5].ChildNodes["table"].ChildNodes["tbody"].ChildNodes["tr"];

                        decimal _X2 = decimal.Parse(trChild.ChildNodes[3].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._X2_] != _X2) newOdds[Odds._X2_] = oldOdd[Odds._X2_] = _X2;

                        decimal _X2_HALF = decimal.Parse(trChild.ChildNodes[4].ChildNodes[3].InnerText, _numberFormatInfo);
                        if (oldOdd[Odds._X2_Half_] != _X2_HALF) newOdds[Odds._X2_Half_] = oldOdd[Odds._X2_Half_] = _X2_HALF;
                        #endregion

                        break;
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return odd;
        }

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
                                            decimal[] odds = new decimal[Odds._NB_ODDS_]; int index = 0;
                                            foreach(HtmlNode oddNode in fieldMatchNode.ChildNodes)
                                            {
                                                if (!oddNode.HasAttributes) continue;
                                                HtmlAttribute oddNodeAttribute = oddNode.Attributes["class"];
                                                if (oddNodeAttribute == null) continue;
                                                if (oddNodeAttribute.Value == "match-odd")
                                                { 
                                                    odds[index] = decimal.Parse(oddNode.ChildNodes["span"].InnerText, _numberFormatInfo);
                                                    index++;
                                                }
                                            }

                                            bets.Add(new BetParsingInformation(BookMaker.BETCLIC, participants, datetime, new Odds(BookMaker.BETCLIC, odds), (new Uri(_BASE_URL_, url)).AbsoluteUri));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }   
                }
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
