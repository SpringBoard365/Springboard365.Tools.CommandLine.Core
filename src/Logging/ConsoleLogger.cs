namespace Springboard365.Tools.CommandLine.Core
{
    using System;

    public class ConsoleLogger
    {
        public static void LogMessage(string value)
        {
            var dateTimeString = DateTime.Now.ToString(ConsoleLoggerOptions.DateTimeFormat);
            Console.WriteLine($"{dateTimeString} {value}");
        }

        public static void LogProgress(string value, ProgressBarOptions progressBarOptions)
        {
            ProgressBar.Draw(value, progressBarOptions);
        }

        public static void LogFatal(string errorMessage)
        {
            var progressBarOptions = new ProgressBarOptions
            {
                Progress = 100,
                Total = 100,
                FilledColour = ConsoleColor.Red,
            };

            ProgressBar.Draw(errorMessage, progressBarOptions);
        }
    }
}