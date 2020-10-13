namespace Springboard365.Tools.CommandLine.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;

    internal sealed class CommandLineArgument
    {
        private readonly CommandLineArgumentAttribute argumentAttribute;

        internal CommandLineArgument(PropertyInfo argumentProperty, CommandLineArgumentAttribute argumentAttribute)
        {
            this.argumentAttribute = argumentAttribute;
            ArgumentProperty = argumentProperty;
        }

        internal bool IsSet { get; private set; }

        internal bool IsCollection
        {
            get
            {
                if (!(ArgumentProperty.PropertyType.GetInterface("IList", true) != null))
                {
                    return ArgumentProperty.PropertyType.GetInterface(typeof(IList<>).FullName, true) != null;
                }

                return true;
            }
        }

        internal bool IsHidden
        {
            get
            {
                return (argumentAttribute.Type & ArgumentType.Hidden) == ArgumentType.Hidden;
            }
        }

        internal bool IsRequired
        {
            get
            {
                return (argumentAttribute.Type & ArgumentType.Required) == ArgumentType.Required;
            }
        }

        internal string SampleUsageValue
        {
            get
            {
                return argumentAttribute.SampleUsageValue;
            }
        }

        internal bool HasShortcut
        {
            get
            {
                return !string.IsNullOrEmpty(argumentAttribute.Shortcut);
            }
        }

        internal string Shortcut
        {
            get
            {
                return argumentAttribute.Shortcut;
            }
        }

        internal string Name
        {
            get
            {
                return argumentAttribute.Name;
            }
        }

        internal string ParameterDescription
        {
            get
            {
                return argumentAttribute.ParameterDescription;
            }
        }

        internal bool SupportsMultiple
        {
            get
            {
                return (argumentAttribute.Type & ArgumentType.Multiple) == ArgumentType.Multiple;
            }
        }

        internal bool IsFlag
        {
            get
            {
                return (argumentAttribute.Type & ArgumentType.Binary) == ArgumentType.Binary;
            }
        }

        internal string Description
        {
            get
            {
                return argumentAttribute.Description;
            }
        }

        private static int? WrapLength
        {
            get
            {
                try
                {
                    return Console.WindowWidth;
                }
                catch (IOException)
                {
                    return default;
                }
            }
        }

        private PropertyInfo ArgumentProperty { get; }

        public override string ToString()
        {
            var stringBuilder1 = new StringBuilder();
            stringBuilder1.AppendLine(ToDescriptionString());
            var stringBuilder2 = new StringBuilder("  ");
            stringBuilder2.Append(Description);
            if (HasShortcut)
            {
                var empty = IsFlag ? string.Empty : ':'.ToString(CultureInfo.InvariantCulture);
                stringBuilder2.AppendFormat("  Short form is '{0}{1}{2}'.", '/', Shortcut, empty);
            }

            stringBuilder1.AppendLine(WrapLine(stringBuilder2.ToString()));
            return stringBuilder1.ToString();
        }

        internal static string WrapLine(string text)
        {
            var wrapLength = WrapLength;
            if (!wrapLength.HasValue)
            {
                return text;
            }

            var strArray = text.Split(null);
            var stringBuilder = new StringBuilder();
            var num1 = 0;
            foreach (var str in strArray)
            {
                var length = str.Length;
                var num2 = num1 + length + 1;
                if ((num2 < wrapLength.GetValueOrDefault() ? 0 : 1) != 0)
                {
                    num1 = length + 1;
                    stringBuilder.Append("\n  " + str);
                }
                else
                {
                    num1 += length + 1;
                    stringBuilder.Append(str);
                }

                stringBuilder.Append(' ');
            }

            return stringBuilder.ToString();
        }

        internal void SetValue(object argTarget, string argValue)
        {
            if (IsSet && !SupportsMultiple)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot set command line argument {0} multiple times", ArgumentProperty.Name));
            }

            if (IsCollection)
            {
                PopulateCollectionParameter(argTarget, argValue);
            }
            else if (IsFlag)
            {
                ArgumentProperty.SetValue(argTarget, true, null);
            }
            else
            {
                var obj = Convert.ChangeType(argValue, ArgumentProperty.PropertyType, CultureInfo.InvariantCulture);
                ArgumentProperty.SetValue(argTarget, obj, null);
            }

            IsSet = true;
        }

        internal string ToSampleString()
        {
            return ToSwitchString(SampleUsageValue);
        }

        private string ToDescriptionString()
        {
            if (IsFlag)
            {
                return ToSwitchString(string.Empty);
            }

            return ToSwitchString(ParameterDescription);
        }

        private string ToSwitchString(string value)
        {
            var delimiter = IsFlag ? string.Empty : ':'.ToString(CultureInfo.InvariantCulture);

            return $" /{Name}{delimiter}{value}";
        }

        private void PopulateCollectionParameter(object argTarget, string argValue)
        {
            IList list = ArgumentProperty.GetValue(argTarget, null) as IList;
            if (list == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "ArgumentProperty {0} did not return an IList as expected.", ArgumentProperty));
            }

            var genericArguments = ArgumentProperty.PropertyType.GetGenericArguments();
            if (genericArguments.Length == 0)
            {
                list.Add(argValue);
            }
            else
            {
                list.Add(Convert.ChangeType(argValue, genericArguments[0], CultureInfo.InvariantCulture));
            }
        }
    }
}