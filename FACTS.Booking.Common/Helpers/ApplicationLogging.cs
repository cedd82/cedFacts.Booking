using Microsoft.Extensions.Logging;

using NLog;
using NLog.Extensions.Logging;

namespace FACTS.GenericBooking.Common.Helpers
{
    public static class ApplicationLogging
    {
        private static ILoggerFactory ClassLoggerFactory { get; set; }

        public static ILoggerFactory LoggerFactory
        {
            get => ClassLoggerFactory;
            set => ClassLoggerFactory = value;
        }

        public static void SetupLoggerFactory()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            LogFactory logFactory = new LogFactory();
            logFactory.LoadConfiguration("nlog.config");
            loggerFactory.AddProvider(new NLogLoggerProvider(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true }, logFactory));
            LoggerFactory = loggerFactory;
        }
    }
}
