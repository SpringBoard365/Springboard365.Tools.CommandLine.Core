namespace Springboard365.Tools.CommandLine.Core
{
    using System;

    public class ProgressBarOptions
    {
        public int Progress { get; set; } = 0;

        public int Total { get; set; } = 100;

        public ConsoleColor FilledColour { get; set; } = ConsoleColor.Gray;

        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Green;
    }
}