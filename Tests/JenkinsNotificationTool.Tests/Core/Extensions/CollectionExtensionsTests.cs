namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Utility;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="CollectionExtensions"/> のユニットテスト クラスです。
    /// </summary>
    /// <seealso cref="TestBase" />
    public class CollectionExtensionsTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public CollectionExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// 試験用のモック クラスです。
        /// </summary>
        private class MockData
        {
            #region Properties

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public int Value { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            #endregion

            #region Methods

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return $"{nameof(Value)}: {Value}, {nameof(Name)}: {Name}";
            }

            #endregion
        }

        #endregion

        #region ToObservableCollection method test

        /// <summary>
        /// <see cref="CollectionExtensions.ToObservableCollection{T}"/> のテストメソッドです。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・データ型のコレクションを変換した結果、取得したコレクションの数と内容が一致していること。
        /// </remarks>
        [Fact]
        public void Test_ToObservableCollection_Success_DataType()
        {
            // arrange
            var target = EnumUtility.ToEnumerable<DayOfWeek>();
            var expected = new ObservableCollection<DayOfWeek>(Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>());

            // act
            var result = target.ToObservableCollection();

            // assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
            WriteResult("(正常系) データ型コレクションを変換した結果、数と内容が一致していること。", result.ToConcatenate(), expected.ToConcatenate());
        }

        /// <summary>
        /// <see cref="CollectionExtensions.ToObservableCollection{T}"/> のテストメソッドです。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・参照型のコレクションを変換した結果、取得したコレクションの数と内容が一致していること。
        /// </remarks>
        [Fact]
        public void Test_ToObservableCollection_Success_ReferenceType()
        {
            // arrange
            var target = new List<MockData>
                             {
                                 new MockData {Name = "Test1", Value = DateTime.Now.Millisecond},
                                 new MockData {Name = "Test2", Value = DateTime.Now.Second},
                                 new MockData {Name = "Test3", Value = DateTime.Now.Minute},
                                 new MockData {Name = "Test4", Value = DateTime.Now.Hour},
                                 new MockData {Name = "Test5", Value = DateTime.Now.Day}
                             };
            var expected = new ObservableCollection<MockData>(target);

            // act
            var result = target.ToObservableCollection();

            // assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
            WriteResult("(正常系) 参照型コレクションを変換した結果、数と内容が一致していること。", result.ToConcatenate(), expected.ToConcatenate());
        }

        /// <summary>
        /// <see cref="CollectionExtensions.ToObservableCollection{T}"/> のテストメソッドです。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・null を変換した結果、初期化されたコレクション要素を返すこと。
        /// </remarks>
        [Fact]
        public void Test_ToObservableCollection_Failed_GetEmpty()
        {
            // arrange
            IEnumerable<MockData> target = null;
            var expected = new ObservableCollection<MockData>();

            // act
            var result = target.ToObservableCollection();

            // assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
            WriteResult("(異常系) nullを変換した結果、初期化されたコレクション要素を返すこと。", result.Any(), expected.Any());
        }

        #endregion

        #region ToConcatenate method test

        /// <summary>
        /// <see cref="CollectionExtensions.ToConcatenate{T}"/> のテストメソッドです。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・引数無しで変換した結果、区切り文字にカンマが使用された文字列を取得すること。
        /// </remarks>
        [Fact]
        public void Test_ToConcatenate_Success_NoneParameter()
        {
            // arrange
            var target = EnumUtility.ToEnumerable<DayOfWeek>();
            var expected = string.Join(",", target);

            // act
            var result = target.ToConcatenate();

            // assert
            Assert.Equal(expected, result);
            WriteResult("(正常系) 引数無しで変換した結果、区切り文字にカンマが使用された文字列を取得すること。", result, expected);
        }

        /// <summary>
        /// <see cref="CollectionExtensions.ToConcatenate{T}"/> のテストメソッドです。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・区切り文字を指定た結果、その区切り文字が正しく設定されていること。
        /// </remarks>
        [Fact]
        public void Test_ToConcatenate_Success_SetParameter()
        {
            // arrange
            var separator = Environment.NewLine;
            var target = EnumUtility.ToEnumerable<DayOfWeek>();
            var expected = string.Join(separator, target);

            // act
            var result = target.ToConcatenate(separator);

            // assert
            Assert.Equal(expected, result);
            WriteResult("(正常系) 区切り文字を指定た結果、その区切り文字が正しく設定されていること。", result, expected);
        }

        /// <summary>
        /// <see cref="CollectionExtensions.ToConcatenate{T}"/> のテストメソッドです。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・区切り文字が空文字の場合、その区切り文字が正しく設定されていること。
        /// </remarks>
        [Fact]
        public void Test_ToConcatenate_Success_SetEmptyParameter()
        {
            // arrange
            var separator = string.Empty;
            var target = EnumUtility.ToEnumerable<DayOfWeek>();
            var expected = string.Join(separator, target);

            // act
            var result = target.ToConcatenate(separator);

            // assert
            Assert.Equal(expected, result);
            WriteResult("(正常系) 区切り文字が空文字の場合、その区切り文字が正しく設定されていること。", result, expected);
        }

        #endregion
    }
}