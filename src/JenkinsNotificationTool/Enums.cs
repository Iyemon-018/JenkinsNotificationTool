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
}