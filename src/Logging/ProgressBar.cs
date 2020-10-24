namespace Springboard365.Tools.CommandLine.Core
{
    using System;

    public static class ProgressBar
    {
        public static void Draw(string currentStageName, ProgressBarOptions progressBarOptions)
        {
            const string Space = " ";

            var progress = progressBarOptions.Progress;
            var total = progressBarOptions.Total;
            var filledColour = progressBarOptions.FilledColour;
            var backgroundColour = progressBarOptions.BackgroundColor;

            var dateTimeString = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ");
            var positionStart = 0 + dateTimeString.Length;
            var positionEnd = 32 + dateTimeString.Length;

            // Draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write(dateTimeString + "[");
            Console.CursorLeft = positionEnd;
            Console.Write("]");
            Console.CursorLeft = positionStart + 1;
            float onechunk = 30.0f / total;

            // Draw filled part
            int position = positionStart + 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = filledColour;
                Console.CursorLeft = position++;
                Console.Write(Space);
            }

            // Draw unfilled part
            for (int i = position; i <= positionEnd - 1; i++)
            {
                Console.BackgroundColor = backgroundColour;
                Console.CursorLeft = position++;
                Console.Write(Space);
            }

            // Draw totals
            Console.CursorLeft = positionEnd + 3;
            Console.BackgroundColor = ConsoleColor.Black;
            var padding = GetPaddingSize(progress, total);
            Console.Write($"{padding}{progress} of {total}  -  ");
            Console.WriteLine(currentStageName);
        }

        private static string GetPaddingSize(int progress, int total)
        {
            int totalLength = total.ToString().Length;
            int progressLength = progress.ToString().Length;

            var difference = totalLength - progressLength;

            var padding = string.Empty;
            for (int i = 0; i < difference; i++)
            {
                padding += " ";
            }

            return padding;
        }
    }
}