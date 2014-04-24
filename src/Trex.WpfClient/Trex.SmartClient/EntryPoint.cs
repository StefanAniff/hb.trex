using System;
using System.Deployment.Application;
using System.Globalization;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Microsoft.VisualBasic.ApplicationServices;
using Trex.SmartClient.Infrastructure.Commands;
using log4net;
using UnhandledExceptionEventArgs = System.UnhandledExceptionEventArgs;

namespace Trex.SmartClient
{
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var manager = new SingleInstanceManager();

            manager.Run(args);
        }
    }

    public class SingleInstanceManager : WindowsFormsApplicationBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private SingleInstanceApplication _app;

        public SingleInstanceManager()
        {
            // Developer must be able to run production trex while developing on trex
            #if !DEBUG
            IsSingleInstance = true;
            #endif
        }

        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
        {
            // First time app is launched
            _app = new SingleInstanceApplication();
            _app.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            var dataDirectory = ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.DataDirectory : "logs";

            var log4NetConfiguration = new Log4NetConfiguration("TRex.SmartClient", dataDirectory);
            log4NetConfiguration.Configure();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            _app.Run();

            return false;
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exceptionMessage = CreateExceptionLog(e.Exception);
            Logger.Error(exceptionMessage);
            e.SetObserved();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string version = "Unknown";
            string userName = "Unknown";

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var name = new AssemblyName(assembly.FullName);
                var windowsIdentity = WindowsIdentity.GetCurrent();
                userName = windowsIdentity.Name;
                version = name.Version.ToString();
            }
            catch (Exception)
            {
            }

            var exceptionObject = e.ExceptionObject;
            var exceptionMessage = CreateExceptionLog(exceptionObject);

            Logger.ErrorFormat("Username: {0} - version: {1} \r\n Details:\r\n{2}", userName, version, exceptionMessage);
        }

        private static string CreateExceptionLog(object exceptionObject)
        {
            var sb = new StringBuilder();
            if (exceptionObject is AggregateException)
            {
                var aggregateExeption = exceptionObject as AggregateException;
                foreach (var innerException in aggregateExeption.Flatten().InnerExceptions)
                {
                    sb.Append(innerException);
                }
            }
            else
            {
                sb.Append(exceptionObject);
            }
            return sb.ToString();
        }


        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            // Subsequent launches
            base.OnStartupNextInstance(eventArgs);
            ApplicationCommands.OpenMainWindow.Execute(null);
        }
    }

    public class SingleInstanceApplication : Application
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            var bootStrapper = new BootStrapper();
            bootStrapper.Run();


            ApplicationCommands.BootCompleted.Execute(null);
        }
    }
}