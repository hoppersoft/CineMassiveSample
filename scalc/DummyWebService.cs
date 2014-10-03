using CineMassive.Calculators;
using System;

namespace scalc
{
    public class DummyWebService : IWebService
    {
        public void ReportError(string message)
        {
            System.Diagnostics.Trace.WriteLine("WebService call: " + message);
        }
    }
}
