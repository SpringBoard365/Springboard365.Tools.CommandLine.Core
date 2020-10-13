namespace Springboard365.Tools.CommandLine.Core
{
    using System;
    using System.Reflection;

    public abstract class CommandLineProgramBase
    {
        protected CommandLineProgramBase(CommandLineParameterBase commandLineParameterBase, string[] args)
        {
            CommandLineParameterBase = commandLineParameterBase;
            CommandLineParameterBase.LoadArguments(args);
            VerifyThenRun();
        }

        protected internal CommandLineParameterBase CommandLineParameterBase { get; }

        private static string ApplicationName
        {
            get
            {
                return ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            }
        }

        private static string ApplicationVersion
        {
            get
            {
                return ((AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true)[0]).Version;
            }
        }

        private static string ApplicationDescription
        {
            get
            {
                return ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
            }
        }

        private static string ApplicationCopyright
        {
            get
            {
                return ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            }
        }

        public abstract void RunBase();

        public void ShowHelp()
        {
            ShowLogo();
            CommandLineParameterBase.ShowUsage();
        }

        private static void ShowLogo()
        {
            Console.Out.WriteLine("{0} : {1} [Version {2}]", ApplicationName, ApplicationDescription, ApplicationVersion);
            Console.Out.WriteLine(ApplicationCopyright);
            Console.Out.WriteLine();
        }

        private void VerifyThenRun()
        {
            try
            {
                if (!CommandLineParameterBase.ShowHelp)
                {
                    if (CommandLineParameterBase.VerifyArguments())
                    {
                        try
                        {
                            RunBase();
                            return;
                        }
                        catch (Exception exception)
                        {
                            Console.Error.WriteLine();
                            Console.Error.WriteLine("Exiting program with exception: {0}", exception.Message);
                            Console.Out.WriteLine("===== DETAIL ======");
                            Console.Out.WriteLine(exception);
                        }
                    }
                }

                ShowHelp();
            }
            catch (InvalidOperationException)
            {
                ShowHelp();
            }
        }
    }
}