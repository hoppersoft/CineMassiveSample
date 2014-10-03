using System;
using System.Collections.Generic;
using System.Linq;

namespace CineMassive.Calculators
{
    public class Parser : IParser
    {
        public const String LineFeed = "\n";
        public const string DelimiterAttentionSequence = "//";
        public const String DefaultDelimiter = ",";
        public readonly System.Text.RegularExpressions.Regex inputRegEx =
            new System.Text.RegularExpressions.Regex("^(?:" + DelimiterAttentionSequence + "(?:\\[(?<delimiter>[^\\]]+)\\])+\\n)?(?<input>.*)", System.Text.RegularExpressions.RegexOptions.Singleline);

        public ParseResult Parse(string input)
        {
            var result = new ParseResult();
            if (!String.IsNullOrEmpty(input))
            {
                // Ordinarily, I'd be doing a TON of string verification here,
                // but since we were assured the inputs will always be valid...

                var match = inputRegEx.Match(input);
                var delimiters = from System.Text.RegularExpressions.Capture c in match.Groups["delimiter"].Captures select c.Value;
                if (!delimiters.Any()) delimiters = new [] { DefaultDelimiter };

                var remainder = match.Groups["input"].Value;
                var numbers = remainder.Split(
                    delimiters.Concat(new [] { LineFeed } ).ToArray(), 
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (var number in numbers.Select(n => Int32.Parse(n)))
                {
                    if (number >= 0)
                    {
                        if (number <= 1000) result.AddNumber(number);
                    }
                    else
                    {
                        result.AddNegativeNumber(number);
                    }
                }
            }
            return result;
        }
    }
}
