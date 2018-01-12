using System;
using WebTools;
using System.Windows.Forms;
using WebTools.HtmlContent;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace ArbMaker
{
    public partial class ParserFrm : Form
    {
        public ParserFrm()
        {
            InitializeComponent();
        }

        private async void readBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Task<string> tsk = WebPageReader.ReadPage(urlTb.Text);
                webPageContentTb.Text = await tsk;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void footballBetclicBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string content = webPageContentTb.Text;
                string[] days = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
                string[] months = new string[] { "janvier", "février", "mars", "avril", "mai", "juin", "juillet", "août", "septembre", "octobre", "novembre", "décembre" };
                string[] lines = Regex.Split(content, "\r\n");
                //string[] lines = content.Split('\n');
                int startingRowIdx = Array.FindIndex(lines, line => line.Contains("1 Nul 2")), size = lines.Length - startingRowIdx;
                string[] rows = new string[size];
                Array.Copy(lines, startingRowIdx, rows, 0, size);
                rows.Replace("1 Nul 2", string.Empty);
                DateTime date = DateTime.MinValue, currentDate = DateTime.MinValue;
                int nbTeams = 2;
                List<Odd> odds = new List<Odd>();

                for(int i = 0; i < rows.Length; i++)
                {
                    string row = rows[i];
                    row = row.Replace("Plus de stats ", string.Empty);
                    if (row == " " || row == "paris") continue;
                    else if (days.Any(day => row.Contains(day))) // date
                    {
                        string[] dateStr = Regex.Split(row, " ");
                        int startDateFieldIdx = 0;
                        while (dateStr[startDateFieldIdx] == string.Empty) startDateFieldIdx++;
                        string monthStr = dateStr[startDateFieldIdx+2].ToLower();
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
                        for(int k = 0; k < nbTeams; k++)
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
                        odds.Add(new Odd(date, teams[0], teams[1], win, loss, draw));
                    }

                    else // end stream
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class Odd
    {
        private DateTime _timestamp;
        private decimal _win, _loss, _draw;
        private string _teamA, _teamB;

        public Odd(DateTime timestamp, string teamA, string teamB, decimal win, decimal loss, decimal draw)
        {
            _timestamp = timestamp; _teamA = teamA; _teamB = teamB; _win = win; _draw = draw; _loss = loss;
        }
    }

    public static class StringHelper
    {
        public static void Replace(this string[] array, string lookUp, string text)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Replace(lookUp, text);
        }
    }
}
