namespace JenkinsNotificationTool.Tests.Core.Utility
{
    using System;
    using System.IO;
    using System.Linq;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Utility;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="FileUtility"/> のユニットテスト クラスです。
    /// </summary>
    /// <seealso cref="TestBase" />
    public class FileUtilityTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public FileUtilityTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region CreateDirectory method test

        /// <summary>
        /// <see cref="FileUtility.CreateDirectory"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・ディレクトリを新たに作成できること。
        /// </remarks>
        [Fact]
        public void Test_CreateDirectory_Success()
        {
            // arrange
            var targetDir = Path.Combine(Environment.CurrentDirectory, "CreateDirectory_TestDir");
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }

            // act
            FileUtility.CreateDirectory(targetDir);

            // assert
            Assert.True(Directory.Exists(targetDir));
            FileUtility.DeleteDirectory(targetDir, true);
        }

        /// <summary>
        /// <see cref="FileUtility.CreateDirectory"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・既に存在しているディレクトリを作成しようとしても例外をスローせずに実行できること。
        /// </remarks>
        [Fact]
        public void Test_CreateDirectory_Success_ExistDirectory()
        {
            // arrange
            var targetDir = Path.Combine(Environment.CurrentDirectory, "CreateDirectory_TestDir");
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // act
            FileUtility.CreateDirectory(targetDir);

            // assert
            Assert.True(Directory.Exists(targetDir));
            FileUtility.DeleteDirectory(targetDir, true);
        }

        /// <summary>
        /// <see cref="FileUtility.CreateDirectory"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・引数がnull の場合でも例外をスローせずに実行できること。
        /// </remarks>
        [Fact]
        public void Test_CreateDirectory_Failed_ParameterNull()
        {
            // arrange
            // act
            // この時点で例外が発生しなければOK。
            var ex = Record.Exception(() => FileUtility.CreateDirectory(null));

            // assert
            Assert.Null(ex);
        }

        #endregion

        #region DeleteDirectory method test

        /// <summary>
        /// <see cref="FileUtility.DeleteDirectory"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・存在するディレクトリにサブディレクトリが存在する場合でも正常に削除できること。
        /// </remarks>
        [Fact]
        public void Test_DeleteDirectory_Success()
        {
            // arrange
            var targetDir = Path.Combine(Environment.CurrentDirectory, "Test_DeleteDirectory");
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }
            FileUtility.CreateDirectory(Path.Combine(targetDir, "Sub_Test_DeleteDirectory"));

            // act
            FileUtility.DeleteDirectory(targetDir, true);

            // assert
            Assert.False(Directory.Exists(targetDir));
        }

        /// <summary>
        /// <see cref="FileUtility.DeleteDirectory"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・存在しないディレクトリパスを削除しようとしても例外をスローせずに実行できること。
        /// </remarks>
        [Fact]
        public void Test_DeleteDirectory_Success_NotExistDirectory()
        {
            // arrange
            var targetDir = Path.Combine(Environment.CurrentDirectory, "Test_DeleteDirectory");
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }

            // act
            var ex = Record.Exception(() => FileUtility.DeleteDirectory(targetDir, true));

            // assert
            Assert.False(Directory.Exists(targetDir));
            Assert.Null(ex);
        }

        /// <summary>
        /// <see cref="FileUtility.DeleteDirectory"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・引数に指定したディレクトリパスがnull でも例外をスローせずに実行できること。
        /// </remarks>
        [Fact]
        public void Test_DeleteDirectory_Failed_ParameterNull()
        {
            // arrange
            // act
            var ex = Record.Exception(() => FileUtility.DeleteDirectory(null, true));

            // assert
            Assert.Null(ex);
        }

        /// <summary>
        /// <see cref="FileUtility.DeleteDirectory"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・存在するディレクトリにサブディレクトリが存在する場合に<see cref="IOException"/> がスローされること。
        /// </remarks>
        [Fact]
        public void Test_DeleteDirectory_Failed_ExistSubDirectory()
        {
            // arrange
            var targetDir = Path.Combine(Environment.CurrentDirectory, "Test_DeleteDirectory");
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }
            FileUtility.CreateDirectory(Path.Combine(targetDir, "Sub_Test_DeleteDirectory"));

            // act
            var ex = Assert.Throws<IOException>(() => FileUtility.DeleteDirectory(targetDir));

            // assert
            Assert.True(Directory.Exists(targetDir));
            Assert.NotNull(ex);
            Assert.IsType<IOException>(ex);

            // 後始末
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }
        }

        #endregion

        #region RemoveFile method test

        /// <summary>
        /// <see cref="FileUtility.RemoveFile"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・ファイルが存在しない場合でも例外をスローしないこと。
        /// </remarks>
        [Fact]
        public void Test_RemoveFile_Failed_FileNotExist()
        {
            // arrange
            var target = Path.Combine(Environment.CurrentDirectory, $"Test_{DateTime.Now:yyyyMMdd-HHmmssfff}.txt");
            var expected = false;

            // act
            FileUtility.RemoveFile(target);

            // assert
            Assert.True(File.Exists(target) == expected);
        }

        /// <summary>
        /// <see cref="FileUtility.RemoveFile"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・指定したファイルを削除すること。
        /// </remarks>
        [Fact]
        public void Test_RemoveFile_Success_FileExist()
        {
            // arrange
            var target = Path.Combine(Environment.CurrentDirectory, $"Test_{DateTime.Now:yyyyMMdd-HHmmssfff}.txt");
            var expected = false;
            File.WriteAllText(target, DateTime.Now.ToString("G"));

            // act
            FileUtility.RemoveFile(target);

            // assert
            Assert.True(File.Exists(target) == expected);
        }

        #endregion

        #region GetFilesForPreviousSpan method test

        private static readonly string GetFilesDirectory = Path.Combine(Environment.CurrentDirectory, "GetFilesForPreviousSpan");

        /// <summary>
        /// <see cref="FileUtility.GetFilesForPreviousSpan"/> のテストメソッドです。（異常系）
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・削除対象のディレクトリが存在しない場合、例外をスローせずに空のコレクションを返すこと。
        /// </remarks>
        [Fact]
        public void Test_GetFilesForPreviousSpan_Failed_NotExistDirectory()
        {
            // arrange
            // 試験対象のフォルダを作成する。
            // フォルダ内のファイルは全て削除する。
            FileUtility.DeleteDirectory(GetFilesDirectory, true);
            var result = Enumerable.Empty<string>();
            
            // act
            var ex = Record.Exception(() =>
                                         result =FileUtility.GetFilesForPreviousSpan(GetFilesDirectory,
                                                                                     TimeSpan.FromDays(1)));

            // assert
            Assert.Null(ex);
            Assert.False(result.Any());
            WriteResult("(異常系)　削除対象のディレクトリが存在しない場合、例外をスローせずに空のコレクションを返すこと。", result.Any(), false);
        }

        /// <summary>
        /// <see cref="FileUtility.GetFilesForPreviousSpan"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・削除対象のファイルが０件の場合、空のコレクションを返すこと。
        /// </remarks>
        [Fact]
        public void Test_GetFilesForPreviousSpan_Success_TargetFilesNothing()
        {
            // arrange
            // 試験対象のフォルダを作成する。
            // フォルダ内のファイルは全て削除する。
            FileUtility.DeleteDirectory(GetFilesDirectory, true);
            FileUtility.CreateDirectory(GetFilesDirectory);
            var result = Enumerable.Empty<string>();

            // ダミーのファイルを作成する。
            // 削除対象外のファイルのみ。
            foreach (var i in Enumerable.Range(0, 3))
            {
                var dt = DateTime.Now.AddDays(-1 * i);
                var path = Path.Combine(GetFilesDirectory, $"test_{dt:yyyy-MM-dd_HHmmssfff}.txt");
                CreateTestFile(path, dt);
            }

            // act
            var ex = Record.Exception(() => result = FileUtility.GetFilesForPreviousSpan(GetFilesDirectory, TimeSpan.FromDays(3)));

            // assert
            Assert.Null(ex);
            Assert.False(result.Any());
            WriteResult("(異常系)　削除対象のファイルが０件の場合、空のコレクションを返すこと。", result.Any(), false);
        }

        /// <summary>
        /// <see cref="FileUtility.GetFilesForPreviousSpan"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・しきい値よりも後の時間に作成されたファイルは削除されること。
        /// </remarks>
        [Fact]
        public void Test_GetFilesForPreviousSpan_Success()
        {
            // arrange
            // 試験対象のフォルダを作成する。
            // フォルダ内のファイルは全て削除する。
            FileUtility.DeleteDirectory(GetFilesDirectory, true);
            FileUtility.CreateDirectory(GetFilesDirectory);
            const int createFileCount = 5;
            const int deleteFileCount = 3;
            const int expected = createFileCount - deleteFileCount;
            var result = Enumerable.Empty<string>();

            // ダミーのファイルを作成する。
            // 削除対象外のファイルのみ。
            foreach (var i in Enumerable.Range(0, createFileCount))
            {
                var dt = DateTime.Now.AddDays(-1 * i).Truncate(TimeUnitKind.Minutes);
                var path = Path.Combine(GetFilesDirectory, $"test_{dt:yyyy-MM-dd_HHmmssfff}.txt");
                CreateTestFile(path, dt);
            }

            // act
            var ex = Record.Exception(() => result = FileUtility.GetFilesForPreviousSpan(GetFilesDirectory, TimeSpan.FromDays(deleteFileCount)));

            // assert
            Assert.Null(ex);
            Assert.Equal(expected, result.Count());
            WriteResult("(正常系) しきい値よりも後の時間に作成されたファイルは削除されること。", result.Count(), expected);
        }
        
        #endregion

        #region DeleteFilesForPreviousSpan method test

        private static readonly string DeleteFilesTestDirectory = Path.Combine(Environment.CurrentDirectory, "DeleteFilesForPreviousSpanTest");

        private void CreateTestFile(string path, DateTime create)
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, DateTime.Now.ToString("G"));
                File.SetCreationTime(path, create);
            }
        }
        
        /// <summary>
        /// <see cref="FileUtility.DeleteFilesForPreviousSpan"/> のテストメソッドです。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・指定したフォルダ内で３時間以上前に作成されたファイルが全て削除されていること。
        /// </remarks>
        [Fact]
        public void Test_DeleteFilesForPreviousSpan_Success()
        {
            // arrange
            const int createFileCount = 5;
            const int deleteFileCount = 3;
            const int expected = createFileCount - deleteFileCount;
            int result = -1;
            FileUtility.DeleteDirectory(DeleteFilesTestDirectory, true);
            FileUtility.CreateDirectory(DeleteFilesTestDirectory);


            // テスト用のファイルを作成する。
            foreach (var i in Enumerable.Range(0, createFileCount))
            {
                var dt = DateTime.Now.AddHours(-1 * i).Truncate(TimeUnitKind.Hours);
                var filePath = Path.Combine(DeleteFilesTestDirectory, $"test_{dt:yyyy-MM-dd_HHmmssfff}.txt");
                CreateTestFile(filePath, dt);
            }

            // act
            var ex = Record.Exception(() =>
                                          result = FileUtility.DeleteFilesForPreviousSpan(DeleteFilesTestDirectory,
                                                                                         TimeSpan.FromHours(deleteFileCount)));

            // assert
            Assert.Null(ex);
            Assert.Equal(expected, result);
            WriteResult("(正常系) 指定したフォルダ内で３時間以上前に作成されたファイルが全て削除されていること。", result, expected);
        }

        /// <summary>
        /// <see cref="FileUtility.DeleteFilesForPreviousSpan"/> のテストメソッドです。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・指定したフォルダ内で３時間以上前に作成されたファイルが存在しない場合、戻り値に0 を返すこと。
        /// ・例外を発生させないこと。
        /// </remarks>
        public void Test_DeleteFilesForPreviousSpan_Success_NothingCanDeleteFile()
        {
            // arrange
            const int createFileCount = 3;
            const int deleteFileCount = 3;
            const int expected = createFileCount - deleteFileCount;
            int result = -1;
            FileUtility.DeleteDirectory(DeleteFilesTestDirectory, true);
            FileUtility.CreateDirectory(DeleteFilesTestDirectory);


            // テスト用のファイルを作成する。
            foreach (var i in Enumerable.Range(0, createFileCount))
            {
                var dt = DateTime.Now.AddHours(-1 * i);
                var filePath = Path.Combine(DeleteFilesTestDirectory, $"test_{dt:yyyy-MM-dd_HHmmssfff}.txt");
                CreateTestFile(filePath, dt);
            }

            // act
            var ex = Record.Exception(() =>
                                          result = FileUtility.DeleteFilesForPreviousSpan(DeleteFilesTestDirectory,
                                                                                         TimeSpan.FromHours(deleteFileCount)));

            // assert
            Assert.Null(ex);
            Assert.Equal(expected, result);
            WriteResult("(正常系) 指定したフォルダ内で３時間以上前に作成されたファイルが存在しない場合、戻り値に0 を返すこと。", result, expected);
        }

        /// <summary>
        /// <see cref="FileUtility.DeleteFilesForPreviousSpan"/> のテストメソッドです。(失敗パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・指定したフォルダが存在しない場合、戻り値に0 を返すこと。
        /// ・例外を発生させないこと。
        /// </remarks>
        public void Test_DeleteFilesForPreviousSpan_Failed_NotExistFolder()
        {
            // arrange
            int result = -1;
            FileUtility.DeleteDirectory(DeleteFilesTestDirectory, true);
            
            // act
            var ex = Record.Exception(() =>
                                          result = FileUtility.DeleteFilesForPreviousSpan(DeleteFilesTestDirectory,
                                                                                         TimeSpan.FromHours(1)));

            // assert
            Assert.Null(ex);
            WriteResult("(異常系) 指定したフォルダが存在しない場合、戻り値に0 を返すこと。", result, 0);
        }

        /// <summary>
        /// <see cref="FileUtility.DeleteFilesForPreviousSpan"/> のテストメソッドです。(失敗パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・指定したフォルダ内にファイルが存在しない場合、戻り値に0 を返すこと。
        /// ・例外を発生させないこと。
        /// </remarks>
        public void Test_DeleteFilesForPreviousSpan_Failed_NotExistFiles()
        {
            // arrange
            const int expected = 0;
            int result = -1;
            FileUtility.DeleteDirectory(DeleteFilesTestDirectory, true);
            FileUtility.CreateDirectory(DeleteFilesTestDirectory);
            
            // act
            var ex = Record.Exception(() =>
                                          result = FileUtility.DeleteFilesForPreviousSpan(DeleteFilesTestDirectory,
                                                                                         TimeSpan.FromHours(12)));

            // assert
            Assert.Null(ex);
            Assert.Equal(expected, result);
            WriteResult("(異常系) 指定したフォルダ内にファイルが存在しない場合、戻り値に0 を返すこと。", result, expected);
        }

        #endregion
    }
}