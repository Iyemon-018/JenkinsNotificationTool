namespace JenkinsNotification.Core.Communicator
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml.Serialization;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Logs;

    public class UdpLogCommunicator : ILogCommunicator
    {
        private static readonly string TargetIPAddress = "127.0.0.1";

        public static readonly int TargetPort = 35263;

        private static readonly int LocalPort = TargetPort - 1;

        public static Encoding Encode = Encoding.UTF8;

        private UdpClient _client;

        public void Connection()
        {
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(TargetIPAddress), TargetPort);
            _client = new UdpClient(LocalPort);
            _client.Connect(serverEndPoint);
        }

        public void Disconnection()
        {
            if (_client != null)
            {
                _client.Close();
                _client.Dispose();
                _client = null;
            }
        }

        public void Send(ILog log)
        {
            if (_client != null)
            {
                byte[] sendMessage;
                var sendLog = log as Log;
                using (var ms = new MemoryStream())
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Log));
                    serializer.Serialize(ms, sendLog);
                    sendMessage = ms.ToArray();
                }
                _client.Send(sendMessage, sendMessage.Length);
            }
        }

        #region IDisposable interface 

        private bool _disposed;

        ~UdpLogCommunicator()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // マネージドリソースを解放する。
            }

            // アンマネージドリソースを開放する。
            Disconnection();

            _disposed = true;
        }

        #endregion
    }
}