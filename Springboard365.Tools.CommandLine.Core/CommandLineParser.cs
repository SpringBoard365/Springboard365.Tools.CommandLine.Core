namespace Springboard365.Tools.CommandLine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public sealed class CommandLineParser
    {
        private readonly List<CommandLineArgument> arguments;
        private readonly Dictionary<string, CommandLineArgument> argumentsMap;
        private readonly CommandLineParameterBase argumentSource;

        public CommandLineParser(CommandLineParameterBase argumentSource)
        {
            arguments = new List<CommandLineArgument>();
            this.argumentSource = argumentSource;
            argumentsMap = GetPropertyMap();
        }

        private List<CommandLineArgument> Arguments
        {
            get
            {
                return arguments;
            }
        }

        private Dictionary<string, CommandLineArgument> ArgumentsMap
        {
            get
            {
                return argumentsMap;
            }
        }

        private CommandLineParameterBase ArgumentsSource
        {
            get
            {
                return argumentSource;
            }
        }

        public void ParseArguments(string[] args)
        {
            if (args != null)
            {
                foreach (string str in args)
                {
                    if (IsArgument(str))
                    {
                        string argumentValue;
                        var argumentName = GetArgumentName(str, out argumentValue);
                        if (!string.IsNullOrEmpty(argumentName) && ArgumentsMap.ContainsKey(argumentName))
                        {
                            ArgumentsMap[argumentName].SetValue(ArgumentsSource, argumentValue);
                        }
                        else
                        {
                            ArgumentsSource.OnUnknownArgument(argumentName, argumentValue);
                        }
                    }
                    else
                    {
                        ArgumentsSource.OnInvalidArgument(str);
                    }
                }
            }

            ParseConfigArguments();
        }

        public bool VerifyArguments()
        {
            foreach (var commandLineArgument in ArgumentsMap.Values)
            {
                if (commandLineArgument.IsRequired && !commandLineArgument.IsSet)
                {
                    return false;
                }
            }

            return true;
        }

        public void WriteUsage()
        {
            Console.Out.WriteLine();
            Console.Out.WriteLine("Options:");
            foreach (var commandLineArgument in Arguments)
            {
                if (!commandLineArgument.IsHidden)
                {
                    Console.Out.WriteLine(commandLineArgument.ToString());
                }
            }

            Console.Out.WriteLine();
            Console.Out.WriteLine("Example:");
            Console.Out.WriteLine(GetSampleUsage());
            Console.Out.WriteLine();
        }

        private static bool IsArgument(string argument)
        {
            return argument[0] == 47;
        }

        private static string GetArgumentName(string argument, out string argumentValue)
        {
            argumentValue = null;
            string str;
            if (argument[0] != 47)
            {
                return null;
            }

            var num = argument.IndexOf(':');
            if (num != -1)
            {
                str = argument.Substring(1, num - 1);
                argumentValue = argument.Substring(num + 1);
            }
            else
            {
                str = argument.Substring(1);
            }

            return str.ToUpperInvariant();
        }

        private static CommandLineArgumentAttribute GetCommandLineAttribute(PropertyInfo property)
        {
            var customAttributes = property.GetCustomAttributes(typeof(CommandLineArgumentAttribute), false);
            if (customAttributes.Length == 0)
            {
                return null;
            }

            return (CommandLineArgumentAttribute)customAttributes[0];
        }

        private static void CreateMapEntry(Dictionary<string, CommandLineArgument> propertyMap, CommandLineArgument argument, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                propertyMap.Add(value.ToUpperInvariant(), argument);
            }
        }

        private void ParseConfigArguments()
        {
            foreach (var allKey in ConfigurationManager.AppSettings.AllKeys)
            {
                var upperInvariant = allKey.ToUpperInvariant();
                var appSetting = ConfigurationManager.AppSettings[allKey];
                if (ArgumentsMap.ContainsKey(upperInvariant) && !ArgumentsMap[upperInvariant].IsSet)
                {
                    ArgumentsMap[upperInvariant].SetValue(ArgumentsSource, appSetting);
                }
            }
        }

        private Dictionary<string, CommandLineArgument> GetPropertyMap()
        {
            var propertyMap = new Dictionary<string, CommandLineArgument>();
            foreach (var property in ArgumentsSource.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                var commandLineAttribute = GetCommandLineAttribute(property);
                if (commandLineAttribute == null)
                {
                    continue;
                }

                var commandLineArgument = new CommandLineArgument(property, commandLineAttribute);
                Arguments.Add(commandLineArgument);
                CreateMapEntry(propertyMap, commandLineArgument, commandLineArgument.Shortcut);
                CreateMapEntry(propertyMap, commandLineArgument, commandLineArgument.Name);
            }

            return propertyMap;
        }

        private string GetSampleUsage()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Path.GetFileName(Assembly.GetExecutingAssembly().Location));
            foreach (var commandLineArgument in Arguments)
            {
                if (!commandLineArgument.IsHidden && commandLineArgument.IsRequired && !string.IsNullOrEmpty(commandLineArgument.SampleUsageValue))
                {
                    stringBuilder.Append(commandLineArgument.ToSampleString());
                }
            }

            return CommandLineArgument.WrapLine(stringBuilder.ToString());
        }
    }
}