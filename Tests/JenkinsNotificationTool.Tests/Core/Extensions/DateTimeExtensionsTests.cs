namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="DateTimeExtensions"/> 拡張メソッドのユニットテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class DateTimeExtensionsTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public DateTimeExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region RoundUp method test

        /// <summary>
        /// <see cref="Test_RoundUp_Theory"/> で使用するテストデータです。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> RoundUpTestData()
        {
            var testValue = new DateTime(2016, 12, 31, 15, 34, 22, 123);

            // ミリ秒を切り上げた結果、１ミリ秒カウントアップすること。
            yield return new object[]
                             {
                                 "ミリ秒の切り上げると1ミリ秒切り上がるであること。",
                                 testValue,
                                 TimeUnitKind.Milliseconds,
                                 new DateTime(2016, 12, 31, 15, 34, 22, 124)
                             };
            // 秒を切り上げた結果、ミリ秒以下は 0 になり１秒カウントアップすること。
            yield return new object[]
                             {
                                 "秒を切り上げると１秒切り上がり、それ以下の単位は 0 となること。",
                                 testValue,
                                 TimeUnitKind.Seconds,
                                 new DateTime(2016, 12, 31, 15, 34, 23, 0)
                             };
            // 分を切り上げた結果、秒以下は 0 になり１分カウントアップすること。
            yield return new object[]
                             {
                                 "分を切り上げると１分切り上がり、それ以下の単位は 0 となること。",
                                 testValue,
                                 TimeUnitKind.Minutes,
                                 new DateTime(2016, 12, 31, 15, 35, 0, 0)
                             };
            // 時間を切り上げた結果、分以下は 0 になり１時間カウントアップすること。
            yield return new object[]
                             {
                                 "時間を切り上げると１時間切り上がり、それ以下の単位は 0 となること。",
                                 testValue,
                                 TimeUnitKind.Hours,
                                 new DateTime(2016, 12, 31, 16, 0, 0, 0)
                             };
        }

        /// <summary>
        /// <see cref="DateTimeExtensions.RoundUp"/> のテストメソッドです。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="testValue">テスト対象の日時</param>
        /// <param name="kind">変換対象の時間種別</param>
        /// <param name="expected">期待値</param>
        [Theory]
        [MemberData(nameof(RoundUpTestData))]
        public void Test_RoundUp_Theory(string caseName, DateTime testValue, TimeUnitKind kind, DateTime expected)
        {
            // arrange
            // act
            var result = testValue.RoundUp(kind);

            // assert
            Assert.Equal(expected.Ticks, result.Ticks);
            WriteResult(caseName, result.ToString("O"), expected.ToString("O"));
        }

        #endregion

        #region Truncate method test

        /// <summary>
        /// <see cref="Test_Truncate_Theory"/> で使用するテストデータです。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> TruncateTestData()
        {
            var testValue = new DateTime(2016, 12, 31, 15, 34, 22, 123);

            // ミリ秒を切り捨てた結果、ミリ秒以下の単位は扱えないためテスト対象の日時と一致すること。
            yield return new object[]
                             {
                                 "ミリ秒以下は切り捨てる単位がないので期待値と結果値が一致すること。",
                                 testValue,
                                 TimeUnitKind.Milliseconds,
                                 new DateTime(2016, 12, 31, 15, 34, 22, 123)
                             };
            // 秒を切り捨てた結果、ミリ秒以下の値は 0 となり秒に変化は無いこと。
            yield return new object[]
                             {
                                 "秒を切り捨てると、それ以下の単位だけが切り捨てられて 0 になること。",
                                 testValue,
                                 TimeUnitKind.Seconds,
                                 new DateTime(2016, 12, 31, 15, 34, 22, 0)
                             };
            // 分を切り捨てた結果、秒以下の値は 0 となり分に変化は無いこと。
            yield return new object[]
                             {
                                 "分を切り捨てると、それ以下の単位だけが切り捨てられて 0 になること。",
                                 testValue,
                                 TimeUnitKind.Minutes,
                                 new DateTime(2016, 12, 31, 15, 34, 0, 0)
                             };
            // 時間を切り捨てた結果、分以下の値は 0 となり時間に変化は無いこと。
            yield return new object[]
                             {
                                 "時間を切り捨てると、それ以下の単位だけが切り捨てられて 0 になること。",
                                 testValue,
                                 TimeUnitKind.Hours,
                                 new DateTime(2016, 12, 31, 15, 0, 0, 0)
                             };
        }

        /// <summary>
        /// <see cref="DateTimeExtensions.Truncate"/> のテストメソッドです。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="testValue">テスト対象の日時</param>
        /// <param name="kind">変換対象の時間種別</param>
        /// <param name="expected">期待値</param>
        [Theory]
        [MemberData(nameof(TruncateTestData))]
        public void Test_Truncate_Theory(string caseName, DateTime testValue, TimeUnitKind kind, DateTime expected)
        {
            // arrange
            // act
            var result = testValue.Truncate(kind);

            // assert
            Assert.Equal(expected.Ticks, result.Ticks);
            WriteResult(caseName, result.ToString("O"), expected.ToString("O"));
        }

        #endregion
    }
}