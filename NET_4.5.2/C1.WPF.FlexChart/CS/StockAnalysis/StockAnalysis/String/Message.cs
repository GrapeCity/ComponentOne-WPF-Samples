using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.String
{
    class Message
    {
        public const string MaximumIndicatorMessage = "You can apply maximum 3 indicators at a time.";
        public const string DisableIndicatorMessage = "Indicators are not applicable in Kagi, Renko or Point & Figure charts, which are time-independent.";
        public const string DisableOverlayMessage = "Overlays are not applicable in Kagi, Renko or Point & Figure charts,. which are time-independent.";
    }
}
