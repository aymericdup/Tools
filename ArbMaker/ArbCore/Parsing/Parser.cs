using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbCore.Bet;

namespace ArbCore.Parsing
{
    public abstract class Parser : IParsing
    {
        #region methods
        #region IParsing
        public List<BetParsingInformation> ReadPageContent(Sport sport, string content)
        {
            switch (sport)
            {
                case Sport.FOOTBALL: return ReadFootballPageContent(content);
                default: return ReadFootballPageContent(content);
            }
        }

        public List<BetParsingInformation> ReadSourceCode(Sport sport, Stream stream)
        {
            switch (sport)
            {
                case Sport.FOOTBALL: return ReadFootballSourceCode(stream);
                default: return ReadFootballSourceCode(stream);
            }
        }
        #endregion

        #region sport parsing
        protected abstract List<BetParsingInformation> ReadFootballPageContent(string content);
        protected abstract List<BetParsingInformation> ReadFootballSourceCode(Stream stream);
        #endregion
        #endregion
    }
}
