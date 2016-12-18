namespace JenkinsNotificationTool
{
    /// <summary>
    /// バルーンの表示時間種別です。
    /// </summary>
    public enum BalloonDisplayTimeKind
    {
        /// <summary>
        /// 手動で閉じます。
        /// </summary>
        Manual,

        /// <summary>
        /// 5秒間表示します。
        /// </summary>
        Seconds5,

        /// <summary>
        /// 15秒間表示します。
        /// </summary>
        Seconds15,

        /// <summary>
        /// 30秒間表示します。
        /// </summary>
        Seconds30,
    }

    /// <summary>
    /// 通知履歴の表示数を表す種別です。
    /// </summary>
    public enum NotifyHistoryCountKind
    {
        /// <summary>
        /// 50
        /// </summary>
        Count50,

        /// <summary>
        /// 100
        /// </summary>
        Count100,

        /// <summary>
        /// 200
        /// </summary>
        Count200,
    }
}