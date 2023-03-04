using System;

namespace Aleab.Common.Logging.Interfaces
{
    public interface ILogFactory
    {
        ILogger GetLogger(Type type);
    }
}