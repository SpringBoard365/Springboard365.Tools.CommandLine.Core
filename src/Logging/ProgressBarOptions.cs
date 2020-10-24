namespace Springboard365.Tools.CommandLine.Core
{
    using System;

    public class ProgressBarOptions
    {
        public int Progress { get; set; } = 0;

        public int Total { get; set; } = 100;

        public ConsoleColor FilledColour { get; set; } = ConsoleColor.Green;

        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Gray;
    }
}