using System.Collections.Generic;
using System.IO;
using ArbCore.Bet;

namespace ArbCore.Parsing
{
    public interface IParsing
    {
        List<BetParsingInformation> ReadPageContent(Sport sport, string content);
        List<BetParsingInformation> ReadSourceCode(Sport sport, Stream stream);
    }
}
