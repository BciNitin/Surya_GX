using System.Collections.Generic;

namespace MobiVueEVO.CommonUtility.Logging
{
    public class FileLogger
    {
        private string filePath;
        private long logFrequency;
        private LogLevel logLevel;
        private long maxFileSize;
        private bool includeTimestamp;
        private List<string> includeLoggers;
        private string logFormat;
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        public FileLogger(string filePath, long logFrequency, LogLevel logLevel, long maxFileSize, bool includeTimestamp, List<string> includeLoggers, string logFormat)
        {
            this.filePath = filePath;
            this.logFrequency = logFrequency;
            this.logLevel = logLevel;
            this.maxFileSize = maxFileSize;
            this.includeTimestamp = includeTimestamp;
            this.includeLoggers = includeLoggers;
            this.logFormat = logFormat;
        }

        // Rest of the class implementation...
    }
}
