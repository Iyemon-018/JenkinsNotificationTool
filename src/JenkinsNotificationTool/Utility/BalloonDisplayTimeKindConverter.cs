namespace JenkinsNotificationTool.Utility
{
    using System;

    /// <summary>
    /// <see cref="BalloonDisplayTimeKind"/> のコンバーター機能クラスです。
    /// </summary>
    public static class BalloonDisplayTimeKindConverter
    {
        /// <summary>
        /// <see cref="Nullable{TimeSpan}"/> 型から<see cref="BalloonDisplayTimeKind"/> への変換を行います。
        /// </summary>
        /// <param name="value">変換元の値</param>
        /// <returns>変換結果</returns>
        public static BalloonDisplayTimeKind Convert(TimeSpan? value)
        {
            BalloonDisplayTimeKind result;
            if (value.HasValue)
            {
                if (value.Value.TotalSeconds == 5.0d)
                {
                    result = BalloonDisplayTimeKind.Seconds5;
                }
                else if (value.Value.TotalSeconds == 15.0d)
                {
                    result = BalloonDisplayTimeKind.Seconds15;
                }
                else if (value.Value.TotalSeconds == 30.0d)
                {
                    result = BalloonDisplayTimeKind.Seconds30;
                }
                else
                {
                    result = BalloonDisplayTimeKind.Manual;
                }
            }
            else
            {
                result = BalloonDisplayTimeKind.Manual;
            }

            return result;
        }

        /// <summary>
        /// <see cref="BalloonDisplayTimeKind"/> 型から<see cref="Nullable{TimeSpan}"/> への変換を行います。
        /// </summary>
        /// <param name="value">変換元の値</param>
        /// <returns>変換結果</returns>
        public static TimeSpan? ConvertBack(BalloonDisplayTimeKind value)
        {
            TimeSpan? result;
            switch (value)
            {
                case BalloonDisplayTimeKind.Seconds5:
                    result = TimeSpan.FromSeconds(5);
                    break;
                case BalloonDisplayTimeKind.Seconds15:
                    result = TimeSpan.FromSeconds(15);
                    break;
                case BalloonDisplayTimeKind.Seconds30:
                    result = TimeSpan.FromSeconds(30);
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}