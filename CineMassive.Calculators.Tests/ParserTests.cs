using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMassive.Calculators.Tests
{
    public class ParserTests
    {
        [Fact(DisplayName = "Verifies a parser can be constructed")]
        public void Verifies_a_parser_can_be_constructed()
        {
            var parser = new Parser();
        }

        [Fact(DisplayName = "Verifies that passing an empty string results in an empty list of numbers to add.")]
        public void Verifies_that_passing_an_empty_string_results_in_an_empty_list_of_numbers_to_add()
        {
            var parser = new Parser();
            var result = parser.Parse(String.Empty);
            Assert.Equal(0, result.NumbersToAdd.Count());
        }

        [Fact(DisplayName = "Verifies that parsing '1, 2' returns two numbers.")]
        public void Verifies_that_parsing_1__2_returns_two_numbers()
        {
            var parser = new Parser();
            var result = parser.Parse("1,2");
            Assert.Equal(2, result.NumbersToAdd.Count());
        }

        [Fact(DisplayName = "Verifies that parsing '1\\n2' returns two numbers.")]
        public void Verifies_that_parsing_1_newline_2_returns_two_numbers()
        {
            var parser = new Parser();
            var result = parser.Parse("1\n2");
            Assert.Equal(2, result.NumbersToAdd.Count());
        }

        [Fact(DisplayName = "Verifies that '1\\n,3' returns 1, 2, and 3")]
        public void Verifies_that__1_newline_3__returns_1__2__and_3()
        {
            var parser = new Parser();
            var result = parser.Parse("1\n,2,3");
            Assert.Equal(3, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(1), "Expected the list of numbers to contain '1'.");
            Assert.True(result.NumbersToAdd.Contains(2), "Expected the list of numbers to contain '2'.");
            Assert.True(result.NumbersToAdd.Contains(3), "Expected the list of numbers to contain '3'.");
        }

        [Fact(DisplayName = "Replacing delimeters should result in the same output as with default deimiters.")]
        public void Replacing_delimeters_should_result_in_the_same_output_as_with_default_deimiters()
        {
            var parser = new Parser();
            var result = parser.Parse("//[;]\n1;2");
            Assert.Equal(2, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(1), "Expected the list of numbers to contain '1'.");
            Assert.True(result.NumbersToAdd.Contains(2), "Expected the list of numbers to contain '2'.");
        }

        [Fact(DisplayName = "Verify that negative numbers are identified by the parser.")]
        public void Verify_that_negative_numbers_are_identified_by_the_parser()
        {
            var parser = new Parser();
            var result = parser.Parse("-1,2");
            Assert.Equal(1, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(2), "Expected the list of numbers to contain '2'.");
            Assert.Equal(1, result.NegativeNumbers.Count());
            Assert.True(result.NegativeNumbers.Contains(-1), "Expected the list of negative numbers to contain '-1'.");
        }

        [Fact(DisplayName = "Verify that multiple negative numbers are identified by the parser.")]
        public void Verify_that_multiple_negative_numbers_are_identified_by_the_parser()
        {
            var parser = new Parser();
            var result = parser.Parse("-1,2,-3");
            Assert.Equal(1, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(2), "Expected the list of numbers to contain '2'.");
            Assert.Equal(2, result.NegativeNumbers.Count());
            Assert.True(result.NegativeNumbers.Contains(-1), "Expected the list of negative numbers to contain '-1'.");
            Assert.True(result.NegativeNumbers.Contains(-3), "Expected the list of negative numbers to contain '-3'.");
        }

        [Fact(DisplayName = "Verifies that parsing '1,1001' returns 1.")]
        public void Verifies_that_parsing__1_1001__returns_1()
        {
            var parser = new Parser();
            var result = parser.Parse("1,1001");
            Assert.Equal(1, result.NumbersToAdd.Count());
            Assert.Equal(1, result.NumbersToAdd.First());
        }

        // Edge case
        [Fact(DisplayName = "Verifies that parsing '1,Int32.Max' returns 1.")]
        public void Verifies_that_parsing__1_Int32_Max__returns_1()
        {
            var parser = new Parser();
            var result = parser.Parse("1," + Int32.MaxValue.ToString());
            Assert.Equal(1, result.NumbersToAdd.Count());
            Assert.Equal(1, result.NumbersToAdd.First());
        }

        // Edge case
        [Fact(DisplayName = "Verifies that parsing '0,1 returns 0,1.")]
        public void Verifies_that_parsing__0_1_returns_0_1()
        {
            var parser = new Parser();
            var result = parser.Parse("0,1");
            Assert.Equal(2, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(0), "Expected the list of numbers to contain '0'.");
            Assert.True(result.NumbersToAdd.Contains(1), "Expected the list of numbers to contain '1'.");
        }

        [Fact(DisplayName = "Delimiters can be of any length with the following format")]
        public void Delimiters_can_be_of_any_length_with_the_following_format()
        {
            var parser = new Parser();
            var result = parser.Parse("//[***]\n1***2***3");
            Assert.Equal(3, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(1), "Expected the list of numbers to contain '1'.");
            Assert.True(result.NumbersToAdd.Contains(2), "Expected the list of numbers to contain '2'.");
            Assert.True(result.NumbersToAdd.Contains(3), "Expected the list of numbers to contain '3'.");
        }

        [Fact(DisplayName = "Allow multiple delimiters")]
        public void Allow_multiple_delimiters()
        {
            var parser = new Parser();
            var result = parser.Parse("//[*][%]\n1*2%3");
            Assert.Equal(3, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(1), "Expected the list of numbers to contain '1'.");
            Assert.True(result.NumbersToAdd.Contains(2), "Expected the list of numbers to contain '2'.");
            Assert.True(result.NumbersToAdd.Contains(3), "Expected the list of numbers to contain '3'.");
        }

        [Fact(DisplayName = "Allow multiple delimiters with length longer than one char")]
        public void Allow_multiple_delimiters_with_length_longer_than_one_char()
        {
            var parser = new Parser();
            var result = parser.Parse("//[***][%%%]\n1***2%%%3");
            Assert.Equal(3, result.NumbersToAdd.Count());
            Assert.True(result.NumbersToAdd.Contains(1), "Expected the list of numbers to contain '1'.");
            Assert.True(result.NumbersToAdd.Contains(2), "Expected the list of numbers to contain '2'.");
            Assert.True(result.NumbersToAdd.Contains(3), "Expected the list of numbers to contain '3'.");
        }
    }
}
