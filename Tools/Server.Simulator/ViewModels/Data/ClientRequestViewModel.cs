namespace Server.Simulator.ViewModels.Data
{
    using System;
    using System.Net;
    using JenkinsNotification.Core.ComponentModels;

    /// <summary>
    /// クライアントからのリクエスト情報を持つViewModel クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.ComponentModels.ViewModelBase" />
    public class ClientRequestViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// クライアントのエンドポイント
        /// </summary>
        private readonly IPEndPoint _clientEndPoint;

        /// <summary>
        /// 接続状態
        /// </summary>
        private readonly WebSocketConnectState _state;

        /// <summary>
        /// リクエスト情報を受け取った日時
        /// </summary>
        private readonly DateTime _dateTime;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="clientEndPoint">クライアントのエンドポイント</param>
        /// <param name="state">クライアントの状態</param>
        public ClientRequestViewModel(IPEndPoint clientEndPoint, WebSocketConnectState state)
        {
            _dateTime       = DateTime.Now;
            _clientEndPoint = clientEndPoint;
            _state          = state;

        }
        
        #endregion

        #region Properties

        /// <summary>
        /// クライアントのエンドポイントを設定、または取得します。
        /// </summary>
        public IPEndPoint ClientEndPoint => _clientEndPoint;

        /// <summary>
        /// 接続状態を設定、または取得します。
        /// </summary>
        public WebSocketConnectState State => _state;

        /// <summary>
        /// リクエスト情報を受け取った日時を取得します。
        /// </summary>
        public DateTime DateTime => _dateTime;

        #endregion
    }
}