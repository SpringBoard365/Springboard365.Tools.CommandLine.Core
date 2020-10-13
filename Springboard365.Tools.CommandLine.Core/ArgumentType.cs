namespace Springboard365.Tools.CommandLine.Core
{
    using System;

    [Flags]
    public enum ArgumentType
    {
        Optional = 1,

        Required = 2,

        Multiple = 4,

        Binary = 8,

        Hidden = 16,
    }
}