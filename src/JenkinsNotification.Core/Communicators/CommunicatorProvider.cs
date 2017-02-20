namespace JenkinsNotification.Core.Communicators
{
    public class CommunicatorProvider : ICommunicatorProvider
    {
        private readonly IWebSocketCommunicator _webSocketCommunicator;

        private readonly IWebApiCommunicator _webApiCommunicator;

        public CommunicatorProvider(IWebSocketCommunicator webSocketCommunicator, IWebApiCommunicator webApiCommunicator)
        {
            _webSocketCommunicator = webSocketCommunicator;
            _webApiCommunicator = webApiCommunicator;
        }

        public IWebSocketCommunicator WebSocketCommunicator => _webSocketCommunicator;

        public IWebApiCommunicator WebApiCommunicator => _webApiCommunicator;
    }
}