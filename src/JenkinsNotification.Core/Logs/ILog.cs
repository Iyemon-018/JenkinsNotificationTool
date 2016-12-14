namespace JenkinsNotification.Core.Logs
{
    using System;
    
    public interface ILog
    {
        DateTime Issue { get; }

        LogLevel Level { get; }

        string Message { get; }

        string Detail { get; }

        string FilePath { get; }

        string MemberName { get; }

        int LineNumber { get; }
    }
}