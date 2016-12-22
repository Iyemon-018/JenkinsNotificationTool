namespace JenkinsNotificationTool.Tests.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using JenkinsNotification.Core.Utility;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="PathUtility"/> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class PathUtilityTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public PathUtilityTests(ITestOutputHelper output) : base(output)
        {
            //
            // 予めアセンブリをロードしておく。
            //
            Products.SetAssembly(Assembly.Load("JenkinsNotificationTool"));
        }

        #endregion

        #region Methods

        #region Static fields test

        /// <summary>
        /// static フィールドのテストメソッドです。
        /// </summary>
        [Fact]
        public void Test_StaticFields()
        {
            // CurrentPath のテスト
            // ・空文字でないこと
            Assert.NotNull(PathUtility.CurrentPath);
            // ・ディレクトリが存在すること。
            Assert.True(Directory.Exists(PathUtility.CurrentPath));
            Output.WriteLine($"{nameof(PathUtility.CurrentPath)} = {PathUtility.CurrentPath}");

            // AppTempPath のテスト
            // ・空文字でないこと
            Assert.NotNull(PathUtility.AppTempPath);
            Output.WriteLine($"{nameof(PathUtility.AppTempPath)} = {PathUtility.AppTempPath}");

            // LogPath のテスト
            // ・空文字でないこと
            Assert.NotNull(PathUtility.LogPath);
            Output.WriteLine($"{nameof(PathUtility.LogPath)} = {PathUtility.LogPath}");

            // ExecuteRootPath のテスト
            // ・空文字でないこと
            Assert.NotNull(PathUtility.ExecuteRootPath);
            Output.WriteLine($"{nameof(PathUtility.ExecuteRootPath)} = {PathUtility.ExecuteRootPath}");

            // ExecuteDriveString のテスト
            // ・空文字でないこと
            Assert.NotNull(PathUtility.ExecuteDriveString);
            // ・１文字だけの文字データであること。
            Assert.True(PathUtility.ExecuteDriveString.Length == 1);
            Output.WriteLine($"{nameof(PathUtility.ExecuteDriveString)} = {PathUtility.ExecuteDriveString}");
        }

        #endregion

        #region GetFilePathList method test

        /// <summary>
        /// <see cref="Test_GetFilePathList_Theory"/> のテストデータです。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> GetFilePathListTestData()
        {
            var fileNames = new List<string>
                                {
                                    "TestData.a",
                                    "TestData.ab",
                                    "TestData.ac",
                                    "TestData.abc"
                                };
            var directory = Path.Combine(Environment.CurrentDirectory, "GetFilePathListTestData");

            yield return new object[]
                             {
                                 "(異常系) 指定したディレクトリが空文字だった場合、例外をスローせずに空のコレクションを返すこと。"
                                 , Enumerable.Empty<string>()
                                 , string.Empty
                                 , fileNames
                                 , ".a"
                                 , true
                             };
            yield return new object[]
                             {
                                 "(異常系) 指定したディレクトリが存在しない場合、例外をスローせずに空のコレクションを返すこと。"
                                 , Enumerable.Empty<string>()
                                 , string.Empty
                                 , fileNames
                                 , ".a"
                                 , false
                             };
            yield return new object[]
                             {
                                 "(異常系) 検索対象のパターン文字列がnull の場合、例外をスローせずに空のコレクションを返すこと。"
                                 , Enumerable.Empty<string>()
                                 , directory
                                 , fileNames
                                 , null
                                 , true
                             };
            yield return new object[]
                             {
                                 "(正常) 一致するデータが存在しない場合、空のコレクションを返すこと。"
                                 , Enumerable.Empty<string>()
                                 , directory
                                 , fileNames
                                 , "UnknownFile.xxx"
                                 , true
                             };
            yield return new object[]
                             {
                                 "(正常) 完全一致で検索すると一致したデータが取得できること。"
                                 , new List<string> { Path.Combine(directory, "TestData.ab") }
                                 , directory
                                 , fileNames
                                 , "TestData.ab"
                                 , true
                             };
            yield return new object[]
                             {
                                 "(正常) 部分一致で検索した場合、期待した結果が得られること。"
                                 , new List<string> { Path.Combine(directory, "TestData.ab"), Path.Combine(directory, "TestData.abc") }
                                 , directory
                                 , fileNames
                                 , "TestData.ab*"
                                 , true
                             };
        }

        /// <summary>
        /// <see cref="PathUtility.GetFilePathList"/> のテストメソッドです。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="expected">期待値</param>
        /// <param name="directory">検索対象のディレクトリ パス</param>
        /// <param name="createFileNames">予め作成しておくファイル名コレクション</param>
        /// <param name="searchPattern">検索パターン文字列</param>
        /// <param name="createDirectory">true ならディレクトリを作成する。false ならディレクトリは作成しない。</param>
        [Theory]
        [MemberData(nameof(GetFilePathListTestData))]
        public void Test_GetFilePathList_Theory(string caseName,
                                                IEnumerable<string> expected,
                                                string directory,
                                                IEnumerable<string> createFileNames,
                                                string searchPattern,
                                                bool createDirectory)
        {
            // arrange
            // 予めファイルを作成しておく。
            FileUtility.DeleteDirectory(directory, true);
            if (createDirectory)
            {
                FileUtility.CreateDirectory(directory);
            }
            foreach (var filePath in createFileNames.Select(x => Path.Combine(directory, x)))
            {
                File.WriteAllText(filePath, DateTime.Now.ToString("O"));
            }

            // act
            var result = PathUtility.GetFilePathList(directory, searchPattern);

            // assert
            Assert.Equal(expected, result);
            Output.WriteLine(caseName);
        }

        #endregion

        #endregion
    }
}