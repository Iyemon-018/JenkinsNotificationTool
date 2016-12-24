namespace JenkinsNotification.Core
{
    using JenkinsNotification.Core.Jenkins.Api;

    /// <summary>
    /// 時間単位の種別を定義します。
    /// </summary>
    public enum TimeUnitKind
    {
        /// <summary>
        /// ミリ秒
        /// </summary>
        Milliseconds,

        /// <summary>
        /// 秒
        /// </summary>
        Seconds,

        /// <summary>
        /// 分
        /// </summary>
        Minutes,

        /// <summary>
        /// 時間
        /// </summary>
        Hours,
    }

    /// <summary>
    /// ジョブの実行結果種別を定義します。
    /// </summary>
    /// <remarks>
    /// <see cref="JobExecuteResult"/> によって取得できる、ジョブのビルド結果です。
    /// </remarks>
    public enum JobResultType
    {
        /// <summary>
        /// 実行結果が不明な値であることを表します。
        /// </summary>
        None = 0,

        /// <summary>
        /// 正常であることを表します。
        /// </summary>
        Success,

        /// <summary>
        /// 警告であることを表します。
        /// </summary>
        Warning,

        /// <summary>
        /// 失敗であることを表します。
        /// </summary>
        Failed,
    }

    /// <summary>
    /// ジョブの状態種別を定義します。
    /// </summary>
    /// <remarks><see cref="JobExecuteResult" /> によって取得できる、ジョブの状態です。</remarks>
    public enum JobStatus
    {
        /// <summary>
        /// 状態が不明な値であることを表します。
        /// </summary>
        None = 0,
        
        /// <summary>
        /// ジョブが開始されたことを表します。
        /// </summary>
        Start,
        
        /// <summary>
        /// ジョブが完了したことを表します。
        /// </summary>
        Success,
    }

    /// <summary>
    /// ログの出力レベルを定義します。
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// トレースログを表します。
        /// </summary>
        Trace,

        /// <summary>
        /// デバッグログを表します。
        /// </summary>
        Debug,

        /// <summary>
        /// 情報ログを表します。
        /// </summary>
        Information,

        /// <summary>
        /// 警告ログを表します。
        /// </summary>
        Warning,

        /// <summary>
        /// エラーログを表します。
        /// </summary>
        Error,

        /// <summary>
        /// 深刻なエラーログを表します。
        /// </summary>
        Fatal,
    }

    /// <summary>
    /// 表示可能な画面識別子です。
    /// </summary>
    public enum ScreenKey
    {
        /// <summary>
        /// タスクトレイに常駐している状態です。
        /// </summary>
        TaskTray,

        /// <summary>
        /// 構成情報の設定画面です。
        /// </summary>
        Configuration,

        /// <summary>
        /// 通知履歴表示画面です。
        /// </summary>
        NotificationHistory,

        /// <summary>
        /// ジョブ詳細情報表示画面です。
        /// </summary>
        Details,
    }

    /// <summary>
    /// WebSocket 通信で受信したデータの種別を定義します。
    /// </summary>
    public enum ReceivedType
    {
        /// <summary>
        /// メッセージ
        /// </summary>
        Message,

        /// <summary>
        /// バイナリ
        /// </summary>
        Binary,
    }
}