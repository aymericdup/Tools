using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _X2_ = 7;
        #endregion

        #endregion

        #region constructor(s)
        public Odds(BookMaker bookMaker, decimal[] odds)
        {
            _bookmaker = bookMaker; _odds = odds;
        }
        #endregion

        #region method(s)

        #endregion
    }
}
