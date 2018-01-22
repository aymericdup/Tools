using System;
using ArbCore.Bet;

namespace ArbCore.Parsing
{
    public class BetParsingInformation
    {
        #region vars
        private string[] _participants;
        private DateTime _date;
        private Odds _odds;
        private string _url;
        private BookMaker _bookMaker;
        #endregion

        #region properties
        public string[] Participants { get { return _participants; } set { _participants = value; } }
        public DateTime Date { get { return _date; } set { _date = value; } }
        public Odds Odds { get { return _odds; } set { _odds = value; } }
        public string Url { get { return _url; } set { _url = value; } }
        public BookMaker BookMaker { get { return _bookMaker; } set { _bookMaker = value; } }
        #endregion

        #region constructor(s)
        public BetParsingInformation(BookMaker bookmaker, string[] participants, DateTime date, Odds odds,  string url = null)
        {
            _bookMaker = bookmaker;
            _participants = participants;
            _date = date;
            _odds = odds;
            _url = url;
        }
        #endregion

        #region method(s)
        #endregion
    }
}
