namespace JenkinsNotificationTool.Utility
{
    /// <summary>
    /// <see cref="NotifyHistoryCountKind"/> のコンバーター機能クラスです。
    /// </summary>
    public static class NotifyHistoryCountKindConverter
    {
        /// <summary>
        /// <see cref="T:int"/> 型から<see cref="NotifyHistoryCountKind"/> への変換を行います。
        /// </summary>
        /// <param name="value">変換元の値</param>
        /// <returns>変換結果</returns>
        public static NotifyHistoryCountKind Convert(int value)
        {
            NotifyHistoryCountKind result;
            switch (value)
            {
                case 50:
                    result = NotifyHistoryCountKind.Count50;
                    break;
                case 100:
                    result = NotifyHistoryCountKind.Count100;
                    break;
                case 200:
                    result = NotifyHistoryCountKind.Count200;
                    break;
                default:
                    result = NotifyHistoryCountKind.Count100;
                    break;
            }
            return result;
        }

        /// <summary>
        /// <see cref="NotifyHistoryCountKind"/> 型から<see cref="T:int"/> への変換を行います。
        /// </summary>
        /// <param name="value">変換元の値</param>
        /// <returns>変換結果</returns>
        public static int ConvertBack(NotifyHistoryCountKind value)
        {
            int result;
            switch (value)
            {
                case NotifyHistoryCountKind.Count50:
                    result = 50;
                    break;
                case NotifyHistoryCountKind.Count100:
                    result = 100;
                    break;
                case NotifyHistoryCountKind.Count200:
                    result = 200;
                    break;
                default:
                    result = 100;
                    break;
            }
            return result;
        }
    }
}