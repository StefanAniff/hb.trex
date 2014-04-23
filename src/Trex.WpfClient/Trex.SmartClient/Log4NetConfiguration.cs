using System.IO;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Trex.SmartClient
{
    public class Log4NetConfiguration
    {
        private readonly string _applicationName;
        private readonly string _filePath;
        private const string LOG_PATTERN = "%d [%t] %-5p %m%n";
        private const string FILE_LOG_PATTERN = "%date %-5level %logger [%property{CorrelationId}] - %message%newline";

        private readonly PatternLayout _layout = new PatternLayout();


        private static string DefaultPattern
        {
            get { return LOG_PATTERN; }
        }

        /// <summary>
        /// Call Configure() when done setting things up
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="filePath"></param>
        public Log4NetConfiguration(string applicationName, string filePath)
        {
            _applicationName = applicationName;
            _filePath = filePath;
            _layout.ConversionPattern = DefaultPattern;
            _layout.ActivateOptions();
        }

        public void Configure()
        {
            var traceLevelFilter = new LevelRangeFilter
                {
                    LevelMax = Level.Error
                };
            var infoLevelFilter = new LevelRangeFilter
                {
                    LevelMax = Level.Info
                };

            var errorLevelFilter = new LevelRangeFilter
                {
                    LevelMin = Level.Warn,
                    LevelMax = Level.Error
                };

            var hierarchy = (Hierarchy) LogManager.GetRepository();

            var traceAppender = CreateTraceAppender(traceLevelFilter);
            var eventLogAppender = CreateEventLogAppender(Level.Warn);
            var infoRollingFileAppender = CreateRollingFileAppender(CreateFileName("Info"), "InfoRollingFileAppender", infoLevelFilter);
            var errorRollingFileAppender = CreateRollingFileAppender(CreateFileName("Error"), "ErrorRollingFileAppender", errorLevelFilter);

            //Configure loggers
            hierarchy.Root.Level = Level.All;
            hierarchy.Root.AddAppender(infoRollingFileAppender); //Debug-Info
            hierarchy.Root.AddAppender(errorRollingFileAppender); //Warn-Error
            hierarchy.Root.AddAppender(eventLogAppender); //Wann
            hierarchy.Root.AddAppender(traceAppender); //Debug only


            hierarchy.Configured = true;
        }


        /// <summary>
        /// Ex. Filepath/[ApplicationName]_type.txt
        /// </summary>
        /// <param name="type">Category such as info. Will be part of filename</param>
        /// <returns>Creates filenanme with full path</returns>
        public string CreateFileName(string type)
        {
            var trimmedApplicationName = _applicationName.Trim(' ');
            var fileName = string.Format("{0}_{1}.txt", trimmedApplicationName, type);
            return Path.Combine(_filePath, fileName);
        }



        #region Internal helpers

        private PatternLayout CreatePatternLayout(string logPattern)
        {
            var patternLayout = new PatternLayout
                {
                    ConversionPattern = logPattern
                };
            patternLayout.ActivateOptions();
            return patternLayout;
        }

        private Logger CreateLoggerConfiguration(string loggerName, Level level)
        {
            var log = LogManager.GetLogger(loggerName);
            var logger = (Logger) log.Logger;
            logger.Level = level;
            return logger;
        }

        #endregion

        #region CreateAppenders

        public EventLogAppender CreateEventLogAppender(Level threshold)
        {
            var tracer = new EventLogAppender();
            var patternLayout = CreatePatternLayout(FILE_LOG_PATTERN);
            tracer.Layout = patternLayout;
            tracer.Name = "EventLogAppender";
            tracer.Threshold = threshold;
            tracer.ApplicationName = _applicationName;
            tracer.ActivateOptions();
            return tracer;
        }



        public TraceAppender CreateTraceAppender(IFilter filter)
        {
            var tracer = new TraceAppender();
            var patternLayout = CreatePatternLayout(LOG_PATTERN);
            tracer.Layout = patternLayout;

            tracer.AddFilter(filter);
            tracer.ActivateOptions();
            return tracer;
        }



        public RollingFileAppender CreateRollingFileAppender(string filenameAndPath, string rollingfileappender, IFilter filter)
        {
            var roller = new RollingFileAppender();
            roller.Name = rollingfileappender;
            roller.AddFilter(filter);
            roller.Layout = CreatePatternLayout(FILE_LOG_PATTERN);
            roller.AppendToFile = true;
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.MaxSizeRollBackups = 4;
            roller.MaximumFileSize = "10MB";
            roller.StaticLogFileName = true;
            roller.File = filenameAndPath;
            roller.ActivateOptions();
            return roller;
        }

        #endregion

    }
}