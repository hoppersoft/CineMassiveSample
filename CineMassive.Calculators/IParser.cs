using System;

namespace CineMassive.Calculators
{
    public interface IParser
    {
        ParseResult Parse(String input);
    }
}
