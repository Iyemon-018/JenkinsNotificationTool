namespace JenkinsNotification.Core.Communicator
{
    using System;
    using JenkinsNotification.Core.Logs;

    public interface ILogCommunicator : IDisposable
    {
        void Connection();

        void Disconnection();

        void Send(ILog log);
    }
}