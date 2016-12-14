namespace JenkinsNotification.Debug.LogViewer.Communicator
{
    using System;
    using JenkinsNotification.Core.Logs;

    public delegate void LogReceivedEventHandler(object sender, LogReceivedEventArgs e);

    public class LogReceivedEventArgs : EventArgs
    {
        public LogReceivedEventArgs(ILog log)
        {
            Log = log;
        }

        public ILog Log { get; private set; }
    }
}