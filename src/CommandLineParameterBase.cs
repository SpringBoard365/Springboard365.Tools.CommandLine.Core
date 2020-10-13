namespace Springboard365.Tools.CommandLine.Core
{
    using System;
    using System.Collections.Generic;

    public class CommandLineParameterBase
    {
        public CommandLineParameterBase()
        {
            Parser = new CommandLineParser(this);
            UnknownParameters = new Dictionary<string, string>();
        }

        [CommandLineArgument(ArgumentType.Optional | ArgumentType.Binary, "help", Description = "Show this usage message.", Shortcut = "?")]
        public bool ShowHelp { get; set; }

        protected internal CommandLineParser Parser { get; }

        protected internal Dictionary<string, string> UnknownParameters { get; }

        public void LoadArguments(string[] args)
        {
            Parser.ParseArguments(args);
        }

        public bool VerifyArguments()
        {
            if (!Parser.VerifyArguments())
            {
                return false;
            }

            return true;
        }

        public void ShowUsage()
        {
            Parser.WriteUsage();
        }

        public void OnUnknownArgument(string argumentName, string argumentValue)
        {
            UnknownParameters[argumentName] = argumentValue;
        }

        public void OnInvalidArgument(string argument)
        {
            throw new InvalidOperationException(string.Format("Argument '{0}' could not be parsed.", argument));
        }
    }
}