namespace JenkinsNotificationTool.Tests.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
            var directory = Path.Combine(Environment.CurrentDirectory, "TestDirectory1");
            var timeSpan = TimeSpan.FromDays(1);
            var expected = Enumerable.Empty<string>();
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            // act
            var result = FileUtility.GetFilesForPreviousSpan(directory, timeSpan);

            // assert
            Assert.Equal(expected, result);
            WriteResult("(異常系) 削除対象のディレクトリが存在しない場合、例外をスローせずに空のコレクションを返すこと。", result.Any(), expected.Any());
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
            var directory = Path.Combine(Environment.CurrentDirectory, "TestDirectory2");
            var timeSpan = TimeSpan.FromDays(1);
            var files = Enumerable.Range(0, 3).Select(x => Path.Combine(directory, $"Test{x}.txt"));
            var expected = Enumerable.Empty<string>();

            // ダミーファイルを作成する。
            RemoveFiles(files);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var dt = DateTime.Today.Add(timeSpan);
            foreach (var filePath in files)
            {
                File.WriteAllText(filePath, dt.ToString("G"));
                File.SetCreationTime(filePath, dt);
            }

            // act
            var result = FileUtility.GetFilesForPreviousSpan(directory, timeSpan);

            // assert
            Assert.Equal(expected, result);
            WriteResult("(異常系) 削除対象のファイルが０件の場合、空のコレクションを返すこと。", result.Any(), expected.Any());
            RemoveFiles(files);
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
            var directory = Path.Combine(Environment.CurrentDirectory, "TestDirectory3");
            var timeSpan = TimeSpan.FromDays(1);
            var files = Enumerable.Range(0, 3).Select(x => Path.Combine(directory, $"Test{x}.txt"));
            var expected = files.Take(1);

            // ダミーファイルを作成する。
            RemoveFiles(files);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var dt = DateTime.Today.Add(timeSpan);
            foreach (var filePath in files.Skip(1))
            {
                File.WriteAllText(filePath, dt.ToString("G"));
                File.SetCreationTime(filePath, dt);
            }

            foreach (var filePath in expected)
            {
                File.WriteAllText(filePath, dt.ToString("G"));
                File.SetCreationTime(filePath, DateTime.Today);
            }

            // act
            var result = FileUtility.GetFilesForPreviousSpan(directory, timeSpan);

            // assert
            Assert.Equal(expected, result);
            WriteResult("(正常系) しきい値よりも後の時間に作成されたファイルは削除されること。", result.Any(), expected.Any());
            RemoveFiles(files);
        }

        /// <summary>
        /// 指定したファイルを削除します。
        /// </summary>
        /// <param name="paths">パス</param>
        private void RemoveFiles(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        #endregion
    }
}