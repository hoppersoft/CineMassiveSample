using System;
using Xunit;
using Moq;

namespace CineMassive.Calculators.Tests
{
    public class StringCalculatorTests
    {
        [Fact(DisplayName = "Verifies that the StringCalculator requires a parser")]
        public void Verifies_that_the_StringCalculator_requires_a_parser()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new StringCalculator(null, new Mock<ILogger>().Object, new Mock<IWebService>().Object)
            );
        }

        [Fact(DisplayName = "Verifies that the StringCalculator requires a logger")]
        public void Verifies_that_the_StringCalculator_requires_a_logger()
        {
            Assert.Throws<ArgumentNullException>( () => 
                new StringCalculator(new Mock<IParser>().Object, null, new Mock<IWebService>().Object));
        }

        [Fact(DisplayName = "Verifies that the StringCalculator requires a webservice")]
        public void Verifies_that_the_StringCalculator_requires_a_webservice()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new StringCalculator(new Mock<IParser>().Object, new Mock<ILogger>().Object, null));
        }

        [Fact(DisplayName = "Verifies a StringCalculator can be constructed")
        ]
        public void Verifies_a_StringCalculator_can_be_constructed()
        {
            var calculator = new StringCalculator(new Mock<IParser>().Object, new Mock<ILogger>().Object, new Mock<IWebService>().Object);
        }

        [Fact(DisplayName = "Verifies the StringCalculator returns the sum of the parsed numbers")
        ]
        public void Verifies_the_StringCalculator_returns_the_sum_of_the_parsed_numbers()
        {
            var parser = new Mock<IParser>();
            var result = new ParseResult();
            result.AddNumber(1);
            result.AddNumber(2);
            parser.Setup(p => p.Parse(It.IsAny<String>())).Returns(result);
            var calculator = new StringCalculator(parser.Object, new Mock<ILogger>().Object, new Mock<IWebService>().Object);
            Assert.Equal(3, calculator.Add("1,2")); // The actual input here is not important
        }

        [Fact(DisplayName = "Verifies the StringCalculator throws when a negative number is passed")]
        public void Verifies_the_StringCalculator_throws_when_a_negative_number_is_passed()
        {
            var parser = new Mock<IParser>();
            var result = new ParseResult();
            result.AddNegativeNumber(-3);
            result.AddNegativeNumber(-4);
            parser.Setup(p => p.Parse(It.IsAny<String>())).Returns(result);
            var calculator = new StringCalculator(parser.Object, new Mock<ILogger>().Object, new Mock<IWebService>().Object);
            Assert.Throws<InvalidOperationException>( () => 
                calculator.Add("-1, -2")); // The actual input here is not important
        }

        [Fact(DisplayName = "Verifies the message the StringCalculator throws when negative numbers are passed contain the numbers")
        ]
        public void Verifies_the_message_the_StringCalculator_throws_when_negative_numbers_are_passed_contain_the_numbers()
        {
            var parser = new Mock<IParser>();
            var result = new ParseResult();
            result.AddNumber(2);
            result.AddNegativeNumber(-3);
            result.AddNegativeNumber(-4);
            parser.Setup(p => p.Parse(It.IsAny<String>())).Returns(result);
            var calculator = new StringCalculator(parser.Object, new Mock<ILogger>().Object, new Mock<IWebService>().Object);

            Exception exception = null;
            try
            {
                calculator.Add("-1, -2"); // The actual input here is not important
            }
            catch (InvalidOperationException e)
            {
                exception = e;
            }
            Assert.True(!exception.Message.Contains("2"), "The exception message should no contain the positive numbers passed to the parser.");
            Assert.True(exception.Message.Contains("-3"), "The exception message should contain the negative numbers passed to the parser.");
            Assert.True(exception.Message.Contains("-4"), "The exception message should contain the negative numbers passed to the parser.");
        }

        [Fact(DisplayName = "Verify Logging Abilities")]
        public void Verify_Logging_Abilities()
        {
            var parser = new Mock<IParser>();
            var result = new ParseResult();
            result.AddNumber(1);
            result.AddNumber(2);
            parser.Setup(p => p.Parse(It.IsAny<String>())).Returns(result);
            var logger = new Mock<ILogger>();
            var calculator = new StringCalculator(parser.Object, logger.Object, new Mock<IWebService>().Object);
            calculator.Add("1,2");
            logger.Verify(l => l.Write("The result is 3"));
        }

        [Fact(DisplayName = "Verify that, if the logger throws an exception, the string calculator should notify an IWebservice")]
        public void Verify_that_if_the_logger_throws_an_exception_the_string_calculator_should_notify_an_IWebservice()
        {
            var parser = new Mock<IParser>();
            var result = new ParseResult();
            result.AddNumber(2);
            result.AddNegativeNumber(-3);
            result.AddNegativeNumber(-4);
            parser.Setup(p => p.Parse(It.IsAny<String>())).Returns(result);

            var webService = new Mock<IWebService>();

            var calculator = new StringCalculator(parser.Object, new Mock<ILogger>().Object, webService.Object);

            Exception exception = null;
            try
            {
                calculator.Add("1,2");
            }
            catch (InvalidOperationException e)
            {
                exception = e;
            }

            Assert.NotNull(exception);
            webService.Verify(s => s.ReportError(exception.Message));
        }

        [Fact(DisplayName = "Verifies that the StringCalculator requires a TextWriter")]
        public void Verifies_that_the_StringCalculator_requires_a_TextWriter()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new StringCalculator(new Mock<IParser>().Object, new Mock<ILogger>().Object, new Mock<IWebService>().Object, null)
            );
        }

        [Fact(DisplayName = "Verify that everytime you call Add(string) it also outputs the number result of the calculation in a new line to the terminal or console")]
        public void Verify_that_everytime_you_call_Add_string__it_also_outputs_the_number_result_of_the_calculation_in_a_new_line_to_the_terminal_or_console()
        {
            var parser = new Mock<IParser>();
            var result = new ParseResult();
            result.AddNumber(1);
            result.AddNumber(2);
            parser.Setup(p => p.Parse(It.IsAny<String>())).Returns(result);

            var webService = new Mock<IWebService>();

            var console = new Mock<System.IO.TextWriter>();

            var calculator = new StringCalculator(parser.Object, new Mock<ILogger>().Object, webService.Object, console.Object);

            Exception exception = null;
            calculator.Add("1,2");

            console.Verify(tw => tw.WriteLine("The result is 3"));
        }
    }
}
