using Core;

namespace Logging
{
    public class NullLogger : ILogger
    {
        public void Log(string message)
        {
        }
    }
}
