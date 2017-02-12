namespace Springboard365.Tools.CommandLine.Core
{
    using System;
    using System.Reflection;

    public abstract class CommandLineProgramBase
    {
        private readonly CommandLineParameterBase commandLineParameterBase;

        protected CommandLineProgramBase(CommandLineParameterBase commandLineParameterBase, string[] args)
        {
            this.commandLineParameterBase = commandLineParameterBase;
            CommandLineParameterBase.LoadArguments(args);
            VerifyThenRun();
        }

        protected internal CommandLineParameterBase CommandLineParameterBase
        {
            get
            {
                return commandLineParameterBase;
            }
        }

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

        public abstract void Run();

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
                            Run();
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