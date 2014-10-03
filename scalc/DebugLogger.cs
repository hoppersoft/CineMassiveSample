using CineMassive.Calculators;

namespace scalc
{
    public class DebugLogger : ILogger
    {
        public void Write(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }
    }
}
