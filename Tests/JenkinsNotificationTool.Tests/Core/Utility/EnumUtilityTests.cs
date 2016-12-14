namespace JenkinsNotificationTool.Tests.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Utility;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="EnumUtility"/> のユニットテスト クラスです。
    /// </summary>
    public class EnumUtilityTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public EnumUtilityTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region ToEnumerable method test

        /// <summary>
        /// <see cref="EnumUtility.ToEnumerable{TEnum}"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・<see cref="DayOfWeek"/> 型をコレクションに変換できることをテストします。
        /// </remarks>
        [Fact]
        public void Test_ToEnumerable_Success()
        {
            // arrange
            var expected = Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>();

            // act
            var result = EnumUtility.ToEnumerable<DayOfWeek>();

            // assert
            Assert.Equal(result, expected);
            Output.WriteLine($"列挙体をコレクション変換の成功パターンです。" +
                             $"{Environment.NewLine}" +
                             $"結果:{string.Join(", ", result.Select(x => x.ToString()))}" +
                             $"{Environment.NewLine}" +
                             $"期待値:{string.Join(", ", expected.Select(x => x.ToString()))}");
        }

        /// <summary>
        /// <see cref="EnumUtility.ToEnumerable{TEnum}"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・列挙体でない型をコレクションに変換しようとすると、<see cref="InvalidOperationException"/> がスローされることを確認します。
        /// </remarks>
        [Fact]
        public void Test_ToEnumerable_Failed_UnmatchedType()
        {
            // arrange
            IEnumerable<int> result = null;

            // act
            var ex = Assert.Throws<InvalidOperationException>(() => result = EnumUtility.ToEnumerable<int>());

            // assert
            Assert.NotNull(ex);
            Assert.Null(result);
            Output.WriteLine($"列挙体をコレクション変換の失敗パターンです。{Environment.NewLine}結果:{ex.Message}");
        }

        #endregion
    }
}