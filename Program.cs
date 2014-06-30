using CrashReporterDotNET;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SweetFX_Configurator
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.ThreadException += ApplicationThreadException;
            //
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            //
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            ReportCrash((Exception)unhandledExceptionEventArgs.ExceptionObject);
            Environment.Exit(0);
        }

        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ReportCrash(e.Exception);
        }

        private static void ReportCrash(Exception exception)
        {
            var reportCrash = new ReportCrash
            {
                FromEmail = "TylerVigario90@gmail.com",
                ToEmail = "logicpwn.exceptions@gmail.com",
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                UserName = "logicpwn.exceptions@gmail.com",
                Password = "Px4XbJrPkssN",
                EnableSSL = true
            };
            reportCrash.Send(exception);
        }
    }
}