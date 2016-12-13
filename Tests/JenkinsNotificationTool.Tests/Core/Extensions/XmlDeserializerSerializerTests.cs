namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using JenkinsNotification.Core.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="XmlDeserializer"/>, <see cref="JenkinsNotification.Core.Extensions.XmlSerializer"/> クラスのユニットテスト クラスです。
    /// </summary>
    public class XmlDeserializerSerializerTests : TestBase
    {
        #region Const

        /// <summary>
        /// デシリアライズ可能なXML文字列
        /// </summary>
        private static readonly string DefaultXmlString = "<?xml version='1.0' encoding=\"UTF-8\"?>"
                                                          + "<MockRoot>"
                                                          + "  <MockXmlData>"
                                                          + "    <StringData>ABC</StringData>"
                                                          + "    <Value>123</Value>"
                                                          + "  </MockXmlData>"
                                                          + "  <MockXmlData>"
                                                          + "    <StringData>Test</StringData>"
                                                          + "    <Value>223</Value>"
                                                          + "  </MockXmlData>"
                                                          + "</MockRoot>";

        /// <summary>
        /// フォーマットエラーのXML文字列
        /// </summary>
        /// <remarks>
        /// 最後のMockRoot が閉じられていない。
        /// </remarks>
        private static readonly string ErrorFormatXmlString = "<?xml version='1.0' encoding=\"UTF-8\"?>"
                                                              + "<MockRoot>"
                                                              + "  <MockXmlData>"
                                                              + "    <StringData>ABC</StringData>"
                                                              + "    <Value>123</Value>"
                                                              + "  </MockXmlData>"
                                                              + "  <MockXmlData>"
                                                              + "    <StringData>Test</StringData>"
                                                              + "    <Value>223</Value>"
                                                              + "  </MockXmlData>"
                                                              + "<MockRoot>";

        /// <summary>
        /// このテストで使用するファイルパス
        /// </summary>
        private static readonly string TestFilePath = Path.Combine(Environment.CurrentDirectory, "TestXmlFile.xml");

        /// <summary>
        /// デシリアライズもとの型と一致しないデータが設定されているXML文字列
        /// </summary>
        /// <remarks>
        /// ２番めのValue が文字列になっている。本来は数値が正しい。
        /// </remarks>
        private static readonly string TypeUnmatchXmlString = "<?xml version='1.0' encoding=\"UTF-8\"?>"
                                                          + "<MockRoot>"
                                                          + "  <MockXmlData>"
                                                          + "    <StringData>ABC</StringData>"
                                                          + "    <Value>123</Value>"
                                                          + "  </MockXmlData>"
                                                          + "  <MockXmlData>"
                                                          + "    <StringData>Test</StringData>"
                                                          + "    <Value>UnmatchData</Value>"
                                                          + "  </MockXmlData>"
                                                          + "</MockRoot>";

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public XmlDeserializerSerializerTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Mock Data

        /// <summary>
        /// テストで使用するモック クラスです。
        /// </summary>
        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false)]
        public class MockRoot
        {
            #region Properties

            /// <remarks/>
            [XmlElement("MockXmlData")]
            public MockRootMockXmlData[] MockXmlData { get; set; }

            #endregion

            #region Methods

            public override string ToString()
            {
                return $"{nameof(MockXmlData)}: {string.Join(",", MockXmlData.Select(x => x + Environment.NewLine))}";
            }

            #endregion
        }

        /// <remarks/>
        [XmlType(AnonymousType = true)]
        public class MockRootMockXmlData
        {
            #region Properties

            /// <remarks/>
            public string StringData { get; set; }

            /// <remarks/>
            public byte Value { get; set; }

            #endregion

            #region Methods

            public override string ToString()
            {
                return $"{nameof(StringData)}: {StringData}, {nameof(Value)}: {Value}";
            }

            #endregion
        }

        #endregion

        #region XmlDeserializer class test

        #region Deserialize Method test

        /// <summary>
        /// <see cref="XmlDeserializer.Deserialize{T}" /> をテストします。(成功パターン)
        /// </summary>
        [Fact]
        public void Test_Deserilize_Success()
        {
            // arrange
            File.WriteAllText(TestFilePath, DefaultXmlString);

            // act
            var result = TestFilePath.Deserialize<MockRoot>();

            // assert
            Assert.NotNull(result);
            Output.WriteLine($"XMLのデシリアライズ成功パターンのテストです。{Environment.NewLine}結果:{result}");
            File.Delete(TestFilePath);
        }

        /// <summary>
        /// <see cref="XmlDeserializer.Deserialize{T}"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・デシリアライズしたファイルの内容と、指定したシリアライズ対象の型が一致しない場合、<see cref="InvalidOperationException"/> がスローされることをテストする。
        /// </remarks>
        [Fact]
        public void Test_Deserialize_Failed_TypeUnmatch()
        {
            // arrange
            File.WriteAllText(TestFilePath, DefaultXmlString);
            MockRootMockXmlData result = null;

            // act
            var ex = Assert.Throws<InvalidOperationException>(
                () =>
                {
                    result = TestFilePath.Deserialize<MockRootMockXmlData>();
                });

            // assert
            Assert.NotNull(ex);
            Assert.Null(result);
            Output.WriteLine($"デシリアライズで指定した型とファイルの内容が一致しないパターンのテストです。{Environment.NewLine}結果:{ex.Message}");
            File.Delete(TestFilePath);
        }

        /// <summary>
        /// <see cref="XmlDeserializer.Deserialize{T}"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・存在しないファイルパスをデシリアライズした結果、<see cref="FileNotFoundException"/> がスローされることをテストする。
        /// </remarks>
        [Fact]
        public void Test_Deserialize_Failed_FileNotFound()
        {
            // arrange
            var dummyFilePath = Path.Combine(Environment.CurrentDirectory, "DummyFile.xml");

            // act
            var ex = Assert.Throws<FileNotFoundException>(() =>
                                                          {
                                                              var result = dummyFilePath.Deserialize<MockRoot>();
                                                          } );

            // assert
            Assert.NotNull(ex);
            Output.WriteLine($"存在しないファイルをデシリアライズするテストです。{Environment.NewLine}結果:{ex.Message}, {ex.FileName}");
        }

        /// <summary>
        /// <see cref="XmlDeserializer.Deserialize{T}"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・XMLファイルのフォーマットエラーの場合、<see cref="InvalidOperationException"/> がスローされることをテストする。
        /// </remarks>
        [Fact]
        public void Test_Deserialize_Failed_ErrorFormat()
        {
            // arrange
            File.WriteAllText(TestFilePath, ErrorFormatXmlString);
            MockRoot result = null;

            // act
            var ex = Assert.Throws<InvalidOperationException>(
                () =>
                {
                    result = TestFilePath.Deserialize<MockRoot>();
                });

            // assert
            Assert.NotNull(ex);
            Assert.Null(result);
            Output.WriteLine($"フォーマットエラーデータのデシリアライズ パターンのテストです。{Environment.NewLine}結果:{ex.Message}");
            File.Delete(TestFilePath);
        }

        /// <summary>
        /// <see cref="XmlDeserializer.Deserialize{T}"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・デシリアライズ元のデータ型とXMLファイルのデータの型が一致しない場合、<see cref="InvalidOperationException"/> がスローされることをテストする。
        /// </remarks>
        [Fact]
        public void Test_Deserialize_Failed_UnmactchedDataType()
        {
            // arrange
            File.WriteAllText(TestFilePath, TypeUnmatchXmlString);
            MockRoot result = null;

            // act
            var ex = Assert.Throws<InvalidOperationException>(
                () =>
                {
                    result = TestFilePath.Deserialize<MockRoot>();
                });

            // assert
            Assert.NotNull(ex);
            Assert.Null(result);
            Output.WriteLine($"データ型の異なるフォーマットでデシリアライズするパターンでテストです。{Environment.NewLine}結果:{ex.Message}");
            File.Delete(TestFilePath);

        }

        #endregion

        #endregion
    }
}