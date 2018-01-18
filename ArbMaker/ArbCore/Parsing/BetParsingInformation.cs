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
