using System;
using System.Windows.Forms;
using WebTools.HtmlContent;
using System.Threading.Tasks;
using System.Collections.Generic;
using ArbCore.Parsing.Parsers;
using ArbCore.Parsing;
using ArbCore.Bet;

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
                BetclicParser parser = new BetclicParser();
                List<BetParsingInformation> bets = parser.ReadPageContent(Sport.FOOTBALL, webPageContentTb.Text);
                List<BetParsingInformation> bets2 = parser.ReadSourceCode(Sport.FOOTBALL, WebPageReader.ReadSourceCode(urlTb.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
