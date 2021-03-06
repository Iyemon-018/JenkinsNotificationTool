﻿namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using JenkinsNotification.Core.Extensions;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Extensions;

    /// <summary>
    /// <see cref="StringExtensions" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class StringExtensionsTests : TestBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public StringExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        #region IsEmpty Method test

        /// <summary>
        /// <see cref="Test_IsEmpty_Theory"/> に使用するテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> IsEmptySuccessTestData
            => new List<object[]>
               {
                   new object[] {"string.Empty を判定する。", true, string.Empty},
                   new object[] {"null を判定する。", true, null},
                   new object[] {"\"\"を判定する。", true, ""},
                   new object[] {"\"\" + \"\"を判定する。", true, "" + ""},
                   new object[] {"null文字0個を判定する。", true, new string('\0', 0)},
                   new object[] {"通常の文字列を判定する。", false, "ABC"},
                   new object[] {"null文字1個を判定する。", false, new string('\0', 1)},
               };

        /// <summary>
        /// <see cref="StringExtensions.IsEmpty" /> をテストします。(成功パターン)
        /// </summary>
        /// <param name="caseName">テストしたい内容、ケースの名称</param>
        /// <param name="expected">期待される結果</param>
        /// <param name="target">判定する値</param>
        [Theory]
        [MemberData(nameof(IsEmptySuccessTestData))]
        public void Test_IsEmpty_Theory(string caseName, bool expected, string target)
        {
            // arrange
            // act
            var realResult = target.IsEmpty();

            // assert
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{realResult} 期待値:{expected}");
            Assert.Equal(expected, realResult);
        }

        #endregion

        #region HasText Method test

        /// <summary>
        /// <see cref="Test_HasText_Theory"/> のテスト用データを取得します。
        /// </summary>
        public static IEnumerable<object[]> HasTextTheoryTestData
            => new List<object[]>
               {
                   new object[] {"string.Empty を判定する。", false, string.Empty},
                   new object[] {"null を判定する。", false, null},
                   new object[] {"\"\"を判定する。", false, ""},
                   new object[] {"\"\" + \"\"を判定する。", false, "" + ""},
                   new object[] {"null文字0個を判定する。", false, new string('\0', 0),},
                   new object[] {"null文字1個を判定する。", true, new string('\0', 1),},
                   new object[] {"通常の文字列を判定する。", true, "ABC",},
               };

        /// <summary>
        /// <see cref="StringExtensions.HasText"/> メソッドのユニットテストメソッドです。
        /// </summary>
        /// <param name="caseName">テストしたい内容、ケースの名称</param>
        /// <param name="expected">期待される結果</param>
        /// <param name="target">判定する値</param>
        [Theory]
        [MemberData(nameof(HasTextTheoryTestData))]
        public void Test_HasText_Theory(string caseName, bool expected, string target)
        {
            // arrange
            // act
            var realResult = target.HasText();

            // assert
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{realResult}, 期待値:{expected}");
            Assert.Equal(realResult, expected);
        }

        #endregion

        #region IsDefined<TEnum> Method test

        /// <summary>
        /// <see cref="Test_IsDefined_Theory{TEnum}"/> メソッドのテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> IsDefinedTheoryTestData
            => new List<object[]>
               {
                   new object[] {"値の名称のみ判定する。", true, "Sunday", DayOfWeek.Sunday},
                   new object[] {"型名も含めた名称を判定する。", false, "DayOfWeek.Sunday", DayOfWeek.Sunday},
                   new object[] {"定義されていない値の名称を判定する。", false, "Nichiyobi", DayOfWeek.Sunday},
                   new object[] {"定義されていない方で定義されている値の名称を判定する。", false, "Youbi.Sunday", DayOfWeek.Sunday},
                   new object[] {"空文字を判定する。", false, "", DayOfWeek.Sunday},
                   new object[] {"nullを判定する。", false, null, DayOfWeek.Sunday},
                   new object[] {"string.Emptyを判定する。", false, string.Empty, DayOfWeek.Sunday},
               };

        /// <summary>
        /// <see cref="StringExtensions.IsDefined{TEnum}" /> をテストします。
        /// </summary>
        /// <typeparam name="TEnum">判定したい列挙体の型</typeparam>
        /// <param name="caseName">テストしたい内容、ケースの名称</param>
        /// <param name="expected">期待される結果</param>
        /// <param name="target">判定する値</param>
        /// <param name="originalValue">
        /// 実際に検証対象となっている値
        /// (実際は使わないけどGenericなので含めるしかない。なんとかうまいことできないものか…)
        /// </param>
        [Theory]
        [MemberData(nameof(IsDefinedTheoryTestData))]
        public void Test_IsDefined_Theory<TEnum>(string caseName, bool expected, string target, TEnum originalValue)
            where TEnum : struct
        {
            // arrange
            // act
            var realResult = target.IsDefined<TEnum>();

            // assert
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{realResult}, 期待値:{expected}");
            Assert.Equal(realResult, expected);
        }

        #endregion

        #region ToEnum Method test

        /// <summary>
        /// <see cref="Test_ToEnum_Theory{TEnum}"/> のテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> ToEnumTheoryTestData
            => new List<object[]>
               {
                   new object[] {"通常の成功パターン", DayOfWeek.Sunday, "Sunday", DayOfWeek.Saturday},
                   new object[] {"変換に失敗してdefaultValue を取得する。defaultValue を指定する。", DayOfWeek.Saturday, "NoneDay", DayOfWeek.Saturday},
                   new object[] {"変換に失敗してdefaultValue を取得する。defaultValue 未指定", default(DayOfWeek), "NoneDay", null},
               };

        /// <summary>
        /// <see cref="StringExtensions.ToEnum{TEnum}" /> をテストします。
        /// </summary>
        /// <typeparam name="TEnum">判定したい列挙体の型</typeparam>
        /// <param name="caseName">テストしたい内容、ケースの名称</param>
        /// <param name="expected">期待される結果</param>
        /// <param name="target">判定する値</param>
        /// <param name="defaultValue">変換に失敗した場合の戻り値</param>
        [Theory]
        [MemberData(nameof(ToEnumTheoryTestData))]
        public void Test_ToEnum_Theory<TEnum>(string caseName, TEnum expected, string target, TEnum? defaultValue)
            where TEnum : struct
        {
            // arrange
            // act
            var realResult = defaultValue.HasValue ? target.ToEnum(defaultValue.Value) : target.ToEnum<TEnum>();

            // assert
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{realResult}, 期待値:{expected}");
            Assert.Equal(realResult, expected);
        }

        #endregion

        #region ToInt Method test

        /// <summary>
        /// <see cref="Test_ToInt_Theory"/> のテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> ToIntTheoryTestData
            => new List<object[]>
               {
                   new object[] {"通常通りに変換できること。", 123, "123", null},
                   new object[] {"マイナスも変換できること。", -45, "-45", null},
                   new object[] {"小数は変換できないこと。", 0, "1.0", null},
                   new object[] {"カンマ区切りは変換できないこと。", 0, "1'000", null},
                   new object[] {"変換失敗時の戻り値を指定する。", 4321, "failed", 4321},
                   new object[] {"int型の最大値を超えた変換はできないこと。", 0, "100000000000000000000000000000000", null},
               };

        /// <summary>
        /// <see cref="StringExtensions.ToInt" /> をテストします。
        /// </summary>
        /// <param name="caseName">テストしたい内容、ケースの名称</param>
        /// <param name="expected">期待される結果</param>
        /// <param name="target">判定する値</param>
        /// <param name="defaultValue">変換に失敗した場合の戻り値</param>
        [Theory]
        [MemberData(nameof(ToIntTheoryTestData))]
        public void Test_ToInt_Theory(string caseName, int expected, string target, int? defaultValue)
        {
            // arrange
            // act
            var realResult = defaultValue.HasValue ? target.ToInt(defaultValue.Value) : target.ToInt();

            // assert
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{realResult}, 期待値:{expected}");
            Assert.Equal(expected, realResult);
        }

        #endregion

        #region IsTimeSpanValue Method test

        /// <summary>
        /// <see cref="Test_IsTimeSpanValue_Theory"/> で使用するテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> IsTimeSpanValueTestData
            => new List<object[]>
                   {
                       new object[] {"(異常系) null を判定するとfalse を返すこと。", false, null, },
                       new object[] {"(異常系) string.Empty を判定するとfalse を返すこと。", false, string.Empty, },
                       new object[] {"(異常系) 半角スペースを判定するとfalse を返すこと。", false, " ", },
                       new object[] {"(異常系) 記号を判定するとfalse を返すこと。", false, "abc", },
                       new object[] {"(正常系) 時刻文字列を判定するとtrue を返すこと。", true, DateTime.Now.TimeOfDay.ToString(), },
                   };

        /// <summary>
        /// <see cref="StringExtensions.IsTimeSpanValue" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="expected">期待する戻り値</param>
        /// <param name="target">判定する値</param>
        [Theory]
        [MemberData(nameof(IsTimeSpanValueTestData))]
        public void Test_IsTimeSpanValue_Theory(string caseName, bool expected, string target)
        {
            // arrange
            var result = false;

            // act
            var ex = Record.Exception(() => result = target.IsTimeSpanValue());

            // assert
            Assert.Null(ex);
            Assert.Equal(expected, result);

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(expected)}:{expected}{Environment.NewLine}" +
                             $" {nameof(target)}:{target}");
        }

        #endregion

        #region ToTimeSpan method test

        /// <summary>
        /// <see cref="Test_ToTimeSpan_Theory"/> で使用するテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> ToTimeSpanTestData
            => new List<object[]>
                   {
                       new object[]
                           {
                               $"(異常系) 文字列がnull で、null を許容しない場合、{typeof(ArgumentNullException)} をスローすること。",
                               null,
                               null,
                               false,
                               typeof(ArgumentNullException),
                           },
                       new object[]
                           {
                               $"(異常系) 文字列がstring.Empty で、null を許容しない場合、{typeof(ArgumentNullException)} をスローすること。",
                               null,
                               string.Empty,
                               false,
                               typeof(ArgumentNullException),
                           },
                       new object[]
                           {
                               $"(異常系) 文字列がnull で、null を許容する場合、戻り値がnull であること。",
                               null,
                               null,
                               true,
                               null,
                           },
                       new object[]
                           {
                               $"(異常系) 文字列がstring.Empty で、null を許容する場合、戻り値がnull であること。",
                               null,
                               string.Empty,
                               true,
                               null,
                           },
                       new object[]
                           {
                               $"(異常系) 文字列が時刻書式でなく、null を許容しない場合、null を返すこと。",
                               null,
                               "abc",
                               false,
                               null,
                           },
                       new object[]
                           {
                               $"(異常系) 文字列が時刻書式でなく、null を許容する場合、null を返すこと。",
                               null,
                               "abc",
                               true,
                               null,
                           },
                       new object[]
                           {
                               $"(正常系) 文字列が時刻書式で、null を許容しない場合、指定した時刻データを返すこと。",
                               TimeSpan.Parse("12:34:56.789"),
                               "12:34:56.789",
                               false,
                               null,
                           },
                       new object[]
                           {
                               $"(正常系) 文字列が時刻書式で、null を許容する場合、指定した時刻データを返すこと。",
                               TimeSpan.Parse("12:34:56.789"),
                               "12:34:56.789",
                               true,
                               null,
                           },
                   };

        /// <summary>
        /// <see cref="StringExtensions.ToTimeSpan" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="expected">期待する戻り値</param>
        /// <param name="target">検証対象の文字列</param>
        /// <param name="allowNullValue"><paramref name="target"/> がnull を許容するかどうか</param>
        /// <param name="exceptionType">スローされる例外の型</param>
        [Theory]
        [MemberData(nameof(ToTimeSpanTestData))]
        public void Test_ToTimeSpan_Theory(string caseName,
                                           TimeSpan? expected,
                                           string target,
                                           bool allowNullValue,
                                           Type exceptionType)
        {
            // arrange
            TimeSpan? result = null;

            // act
            var ex = Record.Exception(() => result = target.ToTimeSpan(allowNullValue));

            // assert
            if (exceptionType == null)
            {
                Assert.Null(ex);
                Assert.Equal(expected, result);
            }
            else
            {
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(expected)}:{expected}{Environment.NewLine}" +
                             $" {nameof(target)}:{target}{Environment.NewLine}" +
                             $" {nameof(allowNullValue)}:{allowNullValue}{Environment.NewLine}" +
                             $" {nameof(exceptionType)}:{exceptionType}");
        }

        #endregion
        }
}