namespace JenkinsNotificationTool.Tests.Core.Communicators
{
    using System;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Communicators;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="WebSocketCommunicator" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class WebSocketCommunicatorTests : TestBase
    {
        private const string ServerAddress = "http://127.0.0.1:33456/";

        private const string ClientUri = "ws://127.0.0.1:33456/";

        private MockWebSocketServer _server;

        public WebSocketCommunicatorTests(ITestOutputHelper output) : base(output)
        {
            _server = new MockWebSocketServer();
            _server.ConnectionClient += Server_OnConnectionClient;
            _server.ReceivedRequest += Server_OnReceivedRequest;
        }

        private void Server_OnReceivedRequest(object sender, WebSocketReceivedRequestEventArgs e)
        {
            
        }

        private void Server_OnConnectionClient(object sender, WebSocketClientEventArgs e)
        {
            
        }

        private async void ConnectionServer()
        {
            await _server.Connection(ServerAddress);
        }

        [Fact]
        public async void Test_Connected_Success()
        {
            // arrange
            var connectedFlag = false;
            var webSocket = new WebSocketCommunicator();
            webSocket.Connected += (sender, e) => { connectedFlag = true; };

            // act
            var ex = Record.Exception(() => webSocket.Connection(ClientUri, 100));
            ConnectionServer();
            await Task.Delay(TimeSpan.FromSeconds(3));

            // assert
            try
            {
                Assert.Null(ex);
                Assert.True(connectedFlag);
            }
            finally
            {
                webSocket.Dispose();
                _server.Disconnection();
            }
        }
    }
}