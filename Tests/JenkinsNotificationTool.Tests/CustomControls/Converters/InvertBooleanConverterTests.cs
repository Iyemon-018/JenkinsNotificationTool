namespace JenkinsNotificationTool.Tests.CustomControls.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using JenkinsNotification.CustomControls.Converters;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="InvertBooleanConverter"/> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class InvertBooleanConverterTests : TestBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public InvertBooleanConverterTests(ITestOutputHelper output) : base(output)
        {
        }

        #region Convert method test

        /// <summary>
        /// <see cref="Test_Convert_Theory"/> のテストデータです。
        /// </summary>
        public static IEnumerable<object[]> ConvertTheoryTestData
                => new List<object[]>
                       {
                           new object[]{ "true を渡して反転した結果を取得できること。", false, true, typeof(bool), null, CultureInfo.CurrentCulture},
                           new object[]{ "false を渡して反転した結果を取得できること", true, false, typeof(bool), null, CultureInfo.CurrentCulture},
                           new object[]{ "パラメータの値によって動作が変化しないこと。", false, true, typeof(bool), "testValue", CultureInfo.CurrentCulture},
                           new object[]{ "カルチャーの変化によって動作が変化しないこと。", false, true, typeof(bool), null, new CultureInfo("en-US"), },
                           new object[]{ "Null許容型のtrue でも反転した結果を取得できること。", false, (bool?)true, typeof(bool), null, CultureInfo.CurrentCulture, },
                           new object[]{ "Null許容型のfalse でも反転した結果を取得できること。", true, (bool?)false, typeof(bool), null, CultureInfo.CurrentCulture, },
                           new object[]{ "(異常系)Null許容型のnull では値が変わらないこと。", DependencyProperty.UnsetValue, (bool?)null, typeof(bool), null, CultureInfo.CurrentCulture, },
                           new object[]{ "(異常系)bool型以外では値が変わらないこと。(int型)", DependencyProperty.UnsetValue, 0, typeof(bool), null, CultureInfo.CurrentCulture, },
                           new object[]{ "(異常系)bool型以外では値が変わらないこと。(string型)", DependencyProperty.UnsetValue, "string test", typeof(bool), null, CultureInfo.CurrentCulture, },
                           new object[]{ "(異常系)bool型以外では値が変わらないこと。(DateTime型)", DependencyProperty.UnsetValue, DateTime.Now, typeof(bool), null, CultureInfo.CurrentCulture, },
                       };

        /// <summary>
        /// <see cref="InvertBooleanConverter.Convert"/> のテストメソッドです。
        /// </summary>
        /// <param name="caseName">テストケース名</param>
        /// <param name="expected">期待する結果値</param>
        /// <param name="value">判定する値</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        [Theory]
        [MemberData(nameof(ConvertTheoryTestData))]
        public void Test_Convert_Theory(string caseName,
                                        object expected,
                                        object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
        {
            // arrange
            var converter = new InvertBooleanConverter();

            // act
            var realResult = converter.Convert(value, targetType, parameter, culture);

            // assert
            Assert.Equal(expected, realResult);
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{realResult}, 期待値:{expected}");
        }

        #endregion
    }
}