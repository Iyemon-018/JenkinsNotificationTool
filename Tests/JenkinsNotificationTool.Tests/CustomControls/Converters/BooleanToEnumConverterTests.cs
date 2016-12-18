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
    /// <see cref="BooleanToEnumConverter" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class BooleanToEnumConverterTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public BooleanToEnumConverterTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Converter method test

        /// <summary>
        /// <see cref="Test_Converter_Theory"/> で使用するテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> ConvertTheoryTestData
            => new List<object[]>
               {
                   new object[]
                   {
                       "(正常系) 指定した値とパラメータが一致している場合、true を返すこと。",
                       true,
                       DayOfWeek.Sunday,
                       typeof(bool),
                       DayOfWeek.Sunday.ToString(),
                       CultureInfo.CurrentCulture
                   },
                   new object[]
                   {
                       "(正常系) 指定した値とパラメータが一致している場合、false を返すこと。",
                       false,
                       DayOfWeek.Monday,
                       typeof(bool),
                       DayOfWeek.Sunday.ToString(),
                       CultureInfo.CurrentCulture
                   },
                   new[]
                   {
                       "(異常系) パラメータを指定していない場合、DependencyProperty.UnsetValue を返すこと。",
                       DependencyProperty.UnsetValue,
                       DayOfWeek.Sunday,
                       typeof(bool),
                       null,
                       CultureInfo.CurrentCulture
                   },
                   new[]
                   {
                       "(異常系) バインディングソースの値が列挙体でない場合、DependencyProperty.UnsetValue を返すこと。",
                       DependencyProperty.UnsetValue,
                       10,
                       typeof(bool),
                       DayOfWeek.Sunday.ToString(),
                       CultureInfo.CurrentCulture
                   }
               };

        /// <summary>
        /// <see cref="BooleanToEnumConverter.Convert" /> をテストします。
        /// </summary>
        /// <param name="caseName">テストケースの内容</param>
        /// <param name="expected">戻り値の期待値</param>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        [Theory]
        [MemberData(nameof(ConvertTheoryTestData))]
        public void Test_Converter_Theory(string caseName, object expected, object value, Type targetType, object parameter, CultureInfo culture)
        {
            // arrange
            var converter = new BooleanToEnumConverter();
            object result = null;

            // act
            var ex = Record.Exception(() => result = converter.Convert(value, targetType, parameter, culture));

            // assert
            Assert.Null(ex);
            Assert.Equal(expected, result);
            WriteResult(caseName, result, expected);
        }

        #endregion

        #region ConvertBack method test

        /// <summary>
        /// <see cref="Test_ConvertBack_Theory"/> で使用するテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> ConvertBackTheoryTestData
            => new List<object[]>
               {
                   new object[]
                   {
                       "(正常系) true を渡すとパラメータに指定した値を返すこと。",
                       DayOfWeek.Sunday,
                       true,
                       typeof(DayOfWeek),
                       DayOfWeek.Sunday.ToString(),
                       CultureInfo.CurrentCulture
                   },
                   new[]
                   {
                       "(正常系) false を渡すとDependencyProperty.UnsetValue を返すこと。",
                       DependencyProperty.UnsetValue,
                       false,
                       typeof(DayOfWeek),
                       DayOfWeek.Sunday.ToString(),
                       CultureInfo.CurrentCulture
                   },
                   new[]
                   {
                       "(異常系) パラメータを指定していない場合、DependencyProperty.UnsetValue を返すこと。",
                       DependencyProperty.UnsetValue,
                       true,
                       typeof(DayOfWeek),
                       null,
                       CultureInfo.CurrentCulture
                   },
                   new[]
                   {
                       "(異常系) バインディングターゲットの値がbool型でない場合、DependencyProperty.UnsetValue を返すこと。",
                       DependencyProperty.UnsetValue,
                       10,
                       typeof(DayOfWeek),
                       DayOfWeek.Sunday.ToString(),
                       CultureInfo.CurrentCulture
                   }
               };

        /// <summary>
        /// <see cref="BooleanToEnumConverter.ConvertBack" /> をテストします。
        /// </summary>
        /// <param name="caseName">テストケースの内容</param>
        /// <param name="expected">期待される戻り値</param>
        /// <param name="value">バインディング ターゲットによって生成される値。</param>
        /// <param name="targetType">変換後の型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        [Theory]
        [MemberData(nameof(ConvertBackTheoryTestData))]
        public void Test_ConvertBack_Theory(string caseName, object expected, object value, Type targetType, object parameter, CultureInfo culture)
        {
            // arrange
            var convert = new BooleanToEnumConverter();
            object result = null;
            
            // act
            var ex = Record.Exception(() => result = convert.ConvertBack(value, targetType, parameter, culture));

            // assert
            Assert.Null(ex);
            Assert.Equal(expected, result);
            WriteResult(caseName, result, expected);
        }

        #endregion
    }
}