namespace ArbCore.Bet
{
    
    public class Odds
    {
        #region vars
        private BookMaker _bookmaker;
        private decimal[] _odds;

        #region const vars
        public const int
            _1_ = 0,
            _X_ = 1,
            _2_ = 2,
            _DNB1_ = 3,
            _DNB2_ = 4,
            _12_ = 5,
            _1X_ = 6,
            _X2_ = 7,
            _12_Half_ = 8,
            _1X_Half_ = 9,
            _X2_Half_ = 10,
            _NB_ODDS_ = 11;
        #endregion

        #endregion

        #region properties
        public decimal this[int i] { get { return _odds[i]; } set { _odds[i] = value; } }
        #endregion

        #region constructor(s)
        public Odds() { }
        public Odds(BookMaker bookMaker, decimal[] odds)
        {
            _bookmaker = bookMaker; _odds = odds;
        }
        #endregion

        #region method(s)

        #endregion
    }
}
