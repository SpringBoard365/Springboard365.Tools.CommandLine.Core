namespace Springboard365.Tools.CommandLine.Core
{
    using System;
    using System.Collections.Generic;

    public class CommandLineParameterBase
    {
        private readonly CommandLineParser parser;
        private readonly Dictionary<string, string> unknownParameters;

        public CommandLineParameterBase()
        {
            parser = new CommandLineParser(this);
            unknownParameters = new Dictionary<string, string>();
        }

        [CommandLineArgument(ArgumentType.Optional | ArgumentType.Binary, "help", Description = "Show this usage message.", Shortcut = "?")]
        public bool ShowHelp { get; set; }

        protected internal CommandLineParser Parser
        {
            get
            {
                return parser;
            }
        }

        protected internal Dictionary<string, string> UnknownParameters
        {
            get
            {
                return unknownParameters;
            }
        }

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
            unknownParameters[argumentName] = argumentValue;
        }

        public void OnInvalidArgument(string argument)
        {
            throw new InvalidOperationException(string.Format("Argument '{0}' could not be parsed.", argument));
        }
    }
}