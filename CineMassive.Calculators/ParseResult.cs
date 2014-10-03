using System.Collections.Generic;

namespace CineMassive.Calculators
{
    public class ParseResult
    {
        private readonly List<int> _numbersToAdd;
        private readonly List<int> _negativeNumbers;
        public ParseResult()
        {
            _numbersToAdd = new List<int>();
            _negativeNumbers = new List<int>();
        }
        public IEnumerable<int> NumbersToAdd
        {
            get
            {
                return _numbersToAdd.AsReadOnly();
            }
        }

        public void AddNumber(int number)
        {
            _numbersToAdd.Add(number);
        }
        public IEnumerable<int> NegativeNumbers
        {
            get
            {
                return _negativeNumbers.AsReadOnly();
            }
        }

        public void AddNegativeNumber(int number)
        {
            _negativeNumbers.Add(number);
        }
    }
}
