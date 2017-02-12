namespace Springboard365.Tools.CommandLine.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandLineArgumentAttribute : Attribute
    {
        public CommandLineArgumentAttribute(ArgumentType argType, string name)
        {
            Type = argType;
            Name = name;
            Shortcut = string.Empty;
            Description = string.Empty;
            ParameterDescription = string.Empty;
            SampleUsageValue = string.Empty;
        }

        public ArgumentType Type { get; private set; }

        public string Name { get; set; }

        public string Shortcut { get; set; }

        public string Description { get; set; }

        public string ParameterDescription { get; set; }

        public string SampleUsageValue { get; set; }
    }
}