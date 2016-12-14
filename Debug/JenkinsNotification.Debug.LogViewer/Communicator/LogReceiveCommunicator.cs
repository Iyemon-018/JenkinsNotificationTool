namespace JenkinsNotification.Debug.LogViewer.Communicator
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Communicator;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Logs;

    public class LogReceiveCommunicator
    {
        private readonly UdpClient _client;

        private readonly string TargetIPAddress = "127.0.0.1";

        private Action<ILog> _receivedAction;

        public event LogReceivedEventHandler Received;

        public static LogReceiveCommunicator Current { get; } = new LogReceiveCommunicator();

        public static void SetReceivedHandler(Action<ILog> action)
        {
            Current._receivedAction = action;
        }
        
        protected void OnReceived(ILog log)
        {
            Received?.Invoke(this, new LogReceivedEventArgs(log));
        }

        public LogReceiveCommunicator()
        {
            var clientIpAddress = new IPEndPoint(IPAddress.Parse(TargetIPAddress), UdpLogCommunicator.TargetPort);
            _client = new UdpClient(clientIpAddress);
        }

        public async Task BeginReceive()
        {
            do
            {
                var data = await _client.ReceiveAsync();

                var log = data.Buffer.Deserialize<Log>();
                _receivedAction?.Invoke(log);
                OnReceived(log);
            }
            while (true);
        }

        public void Close()
        {
            if (_client!= null)
            {
                _client.Close();
                _client.Dispose();
            }
        }
    }
}