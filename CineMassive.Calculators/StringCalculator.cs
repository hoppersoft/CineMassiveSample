using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMassive.Calculators
{
    public class StringCalculator
    {
        private readonly IParser _parser;
        private readonly ILogger _logger;
        private readonly IWebService _reportingService;
        private readonly TextWriter _writer;

        public StringCalculator(IParser parser, ILogger logger, IWebService reportingService) : 
            this(parser, logger, reportingService, Console.Out)
        {

        }

        public StringCalculator(IParser parser, ILogger logger, IWebService reportingService, TextWriter writer)
        {
            if (parser == default(IParser)) throw new ArgumentNullException("parser");
            if (logger == default(ILogger)) throw new ArgumentNullException("logger");
            if (reportingService == default(IWebService)) throw new ArgumentNullException("reportingService");
            if (writer == default(TextWriter)) throw new ArgumentNullException("writer");

            _parser = parser;
            _logger = logger;
            _reportingService = reportingService;
            _writer = writer;
        }

        public int Add(String numbers)
        {
            int result = 0;

            if (!String.IsNullOrEmpty(numbers))
            {
                var parseResult = _parser.Parse(numbers);
                if (parseResult.NegativeNumbers.Any())
                {
                    var errorMessage = String.Format(
                        "negatives not allowed. Invalid numbers: \n{0}", 
                        String.Join(",", parseResult.NegativeNumbers));
                    _reportingService.ReportError(errorMessage);
                    throw new InvalidOperationException(errorMessage.ToString());
                }
                result = parseResult.NumbersToAdd.Sum();
            }

            var message = String.Format("The result is {0}", result);
            _logger.Write(message);
            _writer.WriteLine(message);
            return result;
        }
    }
}
