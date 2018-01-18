using System;

namespace ArbCore.Bet
{

    public enum Country
    {
        FRANCE = 0,
        UK = 1,
        GERMANY = 2,
        SPAIN = 3,
        ITALY = 4,
        SWIITZERLAND = 5,
        NETHERLAND = 6,
        BELGIUM = 7,
        SCOTHLAND = 8,
        EUROPEAN_CUP = 9,
        AUSTRALIA = 10,
        DENMARK = 11,
        INTERNATIONAL = 12
    }

    public enum Competition
    {
        LIGUE_1 = 0,
        LIGUE_2 = 1,
        COUPE_DE_FRANCE = 2,
        COUPE_DE_LA_LIGUE = 3,
        UEFA_CHAMPIONS_LEAGUE = 4,
        PRO_LEAGUE = 5,
        PREMIER_LEAGUE = 6,
        EERSTE_DIVISIE = 7,
        SERIE_A = 8,
        SERIE_B = 9,
        FA_CUP = 10,
        EFL_CUP = 11,
        LIGUA_PRIMERA = 12,
        LIGUA_SEGUNDA = 13,
        COPA_DEL_REY = 14,
        BUNDESLIGA = 15,
        SUPER_LEAGUE = 16,
        EKSTRAKLASA = 17,
        SCOTTISH_CUP = 18,
        NATIONAL = 19,
        COPPA_ITALIA = 20,
        SECOND_BUNDESLIGA = 21,
        GERMAIN_CUP = 22,
        DIVISION_1A = 23,
        PRIMEIRA_LIGA = 24,
        SEGUNDA_LIGA = 25,
        PORTUGAL_CUP = 26,
        EUROPA_LEAGUE = 27
    }

    public enum Sport
    {
        FOOTBALL = 0,
        TENNIS = 1,
        HANDBALL = 2,
        BASKETBALL = 3,
        RUGBY = 4,
        BASEBALL = 5,
        HOCKEY = 6,
        MOTO = 7
    }

    public enum BookMaker
    {
        FDJ = 0,
        WINAMAX = 1,
        BWIN = 2,
        GENYBET = 3,
        UNIBET = 4,
        BETCLIC = 5,
        PMU = 6,
        BETSTARS = 7,
        NETBET = 8,
        ZEBET = 9,
        JOA = 10,
        FEELING_BET = 11,
        BETURF = 12,
        VIVARO = 13
    }

    public class Bet
    {
        #region vars
        private Country _country;
        private Competition _competition;
        private string[] _participants;
        private DateTime _date;
        private Sport _sport;
        #endregion

        #region constructor(s)
        public Bet(Country country, Competition competition, string[] participants, DateTime date, Sport sport)
        {
            _country = country;
            _competition = competition;
            _participants = participants;
            _date = date;
            _sport = sport;
        }
        #endregion

        #region method(s)

        #endregion
    }
}
