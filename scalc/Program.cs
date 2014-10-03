using CineMassive.Calculators;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scalc
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IParser, Parser>();
            container.RegisterType<ILogger, DebugLogger>();
            container.RegisterType<IWebService, DummyWebService>();
            container.RegisterInstance<System.IO.TextWriter>(Console.Out);
            container.RegisterType<StringCalculator>();

            var calculator = container.Resolve<StringCalculator>();

            if (args.Length == 0)
            {
                Environment.Exit(0);
            }

            for (var input = args[0]; !String.IsNullOrEmpty(input); input = Console.ReadLine())
            {
                // The functional requirements didn't explicitly state that we needed
                // to handle delimiter substitution, but since it's a relatively inexpensive
                // feature, we'll include it
                // TODO: Clarify with PM whether we want to keep this.
                // TODO: Rather than us knowing about the parser, can we have the StringCalculator
                //       check to see if we need another line of input? Maybe extend the API?
                if (input.StartsWith(Parser.DelimiterAttentionSequence))
                {
                    input += "\n" + Console.ReadLine();
                }

                try
                {
                    calculator.Add(input);
                }
                catch (InvalidOperationException e)
                {
                    // The FR didn't mention whether we should
                    Console.WriteLine("Error: " + e.Message);
                }
                Console.WriteLine("another input please");
            }

        }
    }
}
