namespace JenkinsNotification.Core.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="T:DateTime"/> 型の拡張メソッドを定義します。
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Const

        /// <summary>
        /// 切り上げデータのマッピング情報
        /// </summary>
        private static readonly Dictionary<TimeUnitKind, TimeSpan> IntervalForKindMapping
            = new Dictionary<TimeUnitKind, TimeSpan>
                  {
                    {TimeUnitKind.Milliseconds, TimeSpan.FromMilliseconds(1)},
                    {TimeUnitKind.Seconds, TimeSpan.FromSeconds(1)},
                    {TimeUnitKind.Minutes, TimeSpan.FromMinutes(1)},
                    {TimeUnitKind.Hours, TimeSpan.FromHours(1)}
                  };

        #endregion

        #region Methods

        /// <summary>
        /// 現在の日時を指定した時間で切り上げます。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="kind">切り上げる時間の単位</param>
        /// <returns>切り上げ結果</returns>
        /// <example>
        /// それぞれの時間種別ごとの戻り値をコンソールに出力する例を以下に示します。
        /// <code><![CDATA[
        /// public class App : Application
        /// {
        ///     protected override void OnStartup(StartupEventArgs e)
        ///     {
        ///         base.OnStartup(e);
        /// 
        ///         var dt = new DateTime(2016, 12, 31, 15, 34, 22, 123);
        /// 
        ///         // -- 2016-12-31T15:34:22.1240000
        ///         // -- 2016-12-31T15:34:23.0000000
        ///         // -- 2016-12-31T15:35:00.0000000
        ///         // -- 2016-12-31T16:00:00.0000000       
        ///         Console.WriteLine($"-- {dt.RoundUp(TimeUnitKind.Milliseconds):O}");
        ///         Console.WriteLine($"-- {dt.RoundUp(TimeUnitKind.Seconds):O}");
        ///         Console.WriteLine($"-- {dt.RoundUp(TimeUnitKind.Minutes):O}");
        ///         Console.WriteLine($"-- {dt.RoundUp(TimeUnitKind.Hours):O}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public static DateTime RoundUp(this DateTime self, TimeUnitKind kind)
        {
            var interval = IntervalForKindMapping[kind];
            return new DateTime((self.Ticks + interval.Ticks + 1) / interval.Ticks * interval.Ticks, self.Kind);
        }

        /// <summary>
        /// 現在の日時を指定した時間以下の単位を切り捨てます。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="kind">切り捨て対象の時間単位</param>
        /// <returns>切り捨て結果</returns>
        /// <example>
        /// それぞれの時間種別ごとの戻り値をコンソールに出力する例を以下に示します。
        /// <code><![CDATA[
        /// public partial class App : Application
        /// {
        ///     protected override void OnStartup(StartupEventArgs e)
        ///     {
        ///         base.OnStartup(e);
        ///
        ///         var dt = new DateTime(2016, 12, 31, 15, 34, 22, 123);
        ///
        ///         // -- 2016-12-31T15:34:22.1230000
        ///         // -- 2016-12-31T15:34:22.0000000
        ///         // -- 2016-12-31T15:34:00.0000000
        ///         // -- 2016-12-31T15:00:00.0000000
        ///         Console.WriteLine($"-- {dt.Truncate(TimeUnitKind.Milliseconds):O}");
        ///         Console.WriteLine($"-- {dt.Truncate(TimeUnitKind.Seconds):O}");
        ///         Console.WriteLine($"-- {dt.Truncate(TimeUnitKind.Minutes):O}");
        ///         Console.WriteLine($"-- {dt.Truncate(TimeUnitKind.Hours):O}");
        ///     }
        /// }
        /// ]]></code></example>
        public static DateTime Truncate(this DateTime self, TimeUnitKind kind)
        {
            var interval = IntervalForKindMapping[kind];
            return new DateTime(((self.Ticks + interval.Ticks) / interval.Ticks - 1) * interval.Ticks, self.Kind);
        }

        #endregion
    }
}