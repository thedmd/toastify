using System;
using Aleab.Common.Logging.Interfaces;

namespace Aleab.Common.Logging
{
    public class LogManager
    {
        #region Static Fields and Properties

        private static ILogFactory LogFactory { get; set; }

        #endregion

        #region Static members

        public static void Init(ILogFactory logFactory)
        {
            LogFactory = logFactory;
        }

        public static ILogger GetLogger(Type type)
        {
            return LogFactory?.GetLogger(type) ?? new NullLogger();
        }

        #endregion
    }
}