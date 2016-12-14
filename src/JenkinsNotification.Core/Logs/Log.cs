namespace JenkinsNotification.Core.Logs
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.Logs.ILog" />
    [Serializable]
    public class Log : ILog
    {
        public DateTime Issue { get; }

        public LogLevel Level { get; }

        public string Message { get; }

        public string Detail { get; }

        public string FilePath { get; }

        internal Log(DateTime issue, LogLevel level, string message, string detail, string filePath, string memberName, int lineNumber)
        {
            Issue = issue;
            Level = level;
            Message = message;
            Detail = detail;
            FilePath = filePath;
            MemberName = memberName;
            LineNumber = lineNumber;
        }

        internal Log(DateTime issue, LogLevel level, string message, string filePath, string memberName, int lineNumber)
        {
            Issue = issue;
            Level = level;
            Message = message;
            FilePath = filePath;
            MemberName = memberName;
            LineNumber = lineNumber;
        }

        public Log()
        {
            
        }

        public string MemberName { get; }

        public int LineNumber { get; }
    }
}