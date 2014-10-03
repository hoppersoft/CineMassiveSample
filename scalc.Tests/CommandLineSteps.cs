using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace scalc.Tests
{
    [Binding]
    public class CommandLineSteps
    {
        [Given(@"I have launched scalc with the command line ""(.*)""")]
        public void GivenIHaveLaunchedScalcWithTheCommandLine(String input)
        {
            var configurationPath = new DirectoryInfo(Environment.CurrentDirectory).Name;
            var scalcFolder = new DirectoryInfo(System.IO.Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\scalc\\bin\\" + configurationPath)).FullName;
            var pathToScalc = Path.Combine(scalcFolder, "scalc.exe");
            var scalc = new Process();
            scalc.StartInfo = new ProcessStartInfo(pathToScalc);
            scalc.StartInfo.Arguments = "\"" + input + "\"";
            scalc.StartInfo.UseShellExecute = false;
            scalc.StartInfo.RedirectStandardOutput = true;
            scalc.StartInfo.RedirectStandardInput = true;
            scalc.Start();
            ScenarioContext.Current.Set<Process>(scalc);
        }

        [When(@"I enter ""(.*)""")]
        public void WhenIEnter(String input)
        {
            var scalc = ScenarioContext.Current.Get<Process>();
            scalc.StandardInput.WriteLine(input);
        }

        [When(@"I wait for output")]
        public void WhenIWaitForOutput()
        {
            var scalc = ScenarioContext.Current.Get<Process>();
            StringBuilder scalcOutput = new StringBuilder();

            Type taskType = typeof(System.Threading.Tasks.Task<String>);
            while (true)
            {
                System.Threading.Tasks.Task<String> nextLine = null;
                if (!ScenarioContext.Current.TryGetValue<System.Threading.Tasks.Task<String>>(out nextLine))
                {
                    nextLine = scalc.StandardOutput.ReadLineAsync();
                    ScenarioContext.Current.Set<System.Threading.Tasks.Task<String>>(nextLine);
                }
                if (!nextLine.IsCompleted) nextLine.Wait(500);
                if (!nextLine.IsCompleted)
                {
                    break;
                }
                ScenarioContext.Current.Remove(taskType.FullName);
                if (String.IsNullOrEmpty(nextLine.Result)) break;
                scalcOutput.AppendLine(nextLine.Result);
            }
            ScenarioContext.Current.Set<String>(scalcOutput.ToString(), "Output");
        }

        [Then(@"the screen should display ""(.*)""")]
        public void ThenTheScreenShouldDisplay(string output)
        {
            var scalcOutput = ScenarioContext.Current.Get<String>("Output");

            Assert.Contains(output, scalcOutput);
        }

        [Then(@"the application should end\.")]
        public void ThenTheApplicationShouldEnd_()
        {
            var scalc = ScenarioContext.Current.Get<Process>();
            Assert.True(scalc.HasExited);
        }
    }
}
