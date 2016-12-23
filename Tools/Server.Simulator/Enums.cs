namespace Server.Simulator
{
    /// <summary>
    /// WebSocket 通信中のクライアントの状態を定義します。
    /// </summary>
    public enum WebSocketConnectState
    {
        /// <summary>
        /// 切断されています。
        /// </summary>
        Disconnect,
        
        /// <summary>
        /// 接続しています。
        /// </summary>
        Connect,

        /// <summary>
        /// メッセージを受信しました。
        /// </summary>
        Received,

        /// <summary>
        /// メッセージを送信しました。
        /// </summary>
        Send,
    }
}