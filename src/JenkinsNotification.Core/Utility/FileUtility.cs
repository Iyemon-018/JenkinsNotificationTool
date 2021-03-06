﻿namespace JenkinsNotification.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Logs;

    /// <summary>
    /// ファイル操作関連のユーティリティ機能クラスです。
    /// </summary>
    public static class FileUtility
    {
        #region Methods

        /// <summary>
        /// 指定したパスのディレクトリを作成します。<para/>
        /// サブディレクトリも作成します。
        /// </summary>
        /// <param name="path">ディレクトリパス</param>
        public static void CreateDirectory(string path)
        {
            if (path.HasText() && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                LogManager.Info($"ディレクトリを新規作成しました。(Path:{path})");
            }
        }

        /// <summary>
        /// 指定したパスのディレクトリを削除します。
        /// </summary>
        /// <param name="path">削除するディレクトリ パス</param>
        /// <param name="recursive"><paramref name="path"/> のディレクトリ、サブディレクトリ、およびファイルを削除する場合は true。それ以外の場合は false。</param>
        public static void DeleteDirectory(string path, bool recursive = false)
        {
            if (path.HasText() && Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
                LogManager.Info($"ディレクトリを{(recursive ? "サブディレクトリ、ファイルも含めて": string.Empty)}削除しました。(Path:{path})");
            }
        }

        /// <summary>
        /// 現在日付から<paramref name="previousDate"/>よりも後日に作成されたファイルを削除します。。
        /// </summary>
        /// <param name="directory">検索対象のディレクトリパス</param>
        /// <param name="previousDate">
        /// 取得期間の<see cref="T:TimeSpan"/><para/>
        /// 現在日時からこの<see cref="T:TimeSpan"/> 以降の日付に作成されたファイルパスを取得します。
        /// </param>
        /// <returns>削除したファイルの数</returns>
        /// <example>
        /// フォルダ"C:\work\test"から３日前までに作成されたファイルを全て削除する例を以下に示します。
        /// <code><![CDATA[
        /// public class App : Application
        /// {
        ///     protected override void OnStartup(StartupEventArgs e)
        ///     {
        ///         base.OnStartup(e);
        /// 
        ///         //
        ///         // Output delete files before three days from target folder.
        ///         //
        ///         var deleteFileCount = FileUtility.DeleteFilesForPreviousSpan(@"C:\work\test", TimeSpan.FromDays(3));
        ///         Console.WriteLine(string.Join(Environment.NewLine, deleteFileCount));
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public static int DeleteFilesForPreviousSpan(string directory, TimeSpan previousDate)
        {
            var result = 0;
            var files = GetFilesForPreviousSpan(directory, previousDate);
            if (!files.Any())
            {
                return result;
            }

            foreach (var file in files)
            {
                RemoveFile(file);
                result++;
            }
            return result;
        }

        /// <summary>
        /// 現在日付から<paramref name="previousDate"/> よりも後日に作成されたファイル パスを取得します。
        /// </summary>
        /// <param name="directory">検索対象のディレクトリパス</param>
        /// <param name="previousDate">
        /// 取得期間の<see cref="T:TimeSpan"/><para/>
        /// 現在日時からこの<see cref="T:TimeSpan"/> 以降の日付に作成されたファイルパスを取得します。
        /// </param>
        /// <returns>
        /// 該当ファイルパス コレクション<para/>
        /// <param name="directory"></param> が存在しない場合は、<see cref="Enumerable.Empty{TResult}"/> を返します。
        /// </returns>
        /// <example>
        /// フォルダ"C:\work\test"から７日前までに作成されたファイルを全て列挙する例を以下に示します。
        /// <code><![CDATA[
        /// public class App : Application
        /// {
        ///     protected override void OnStartup(StartupEventArgs e)
        ///     {
        ///         base.OnStartup(e);
        /// 
        ///         //
        ///         // Get created file paths before than seven days ago from target directory.
        ///         //
        ///         var filePaths = FileUtility.GetFilesForPreviousSpan(@"C:\work\test", TimeSpan.FromDays(7));
        ///         Console.WriteLine(string.Join(Environment.NewLine, filePaths);
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public static IEnumerable<string> GetFilesForPreviousSpan(string directory, TimeSpan previousDate)
        {
            using (TimeTracer.StartNew($"{previousDate:g} よりも後日に作成されたファイル パスを取得する。"))
            {
                if (!Directory.Exists(directory))
                {
                    // 該当フォルダなし。
                    return Enumerable.Empty<string>();
                }

                //
                // 一度、Key=ファイルパス, Value=作成日時 の連想配列に変換し、
                // 日時でフィルターし、その結果をファイルパス配列に変換して返す。
                //
                return Directory.GetFiles(directory)
                                .ToDictionary(x => x, File.GetCreationTime)
                                .Where(x => x.Value.CompareTo(DateTime.Now.Subtract(previousDate)) == -1)
                                .Select(x => x.Key);
            }
        }

        /// <summary>
        /// 指定したファイルを削除します。<para/>
        /// ファイルが存在しない場合は削除が実行されません。
        /// </summary>
        /// <param name="filePath">削除対象のファイルパス</param>
        public static void RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                LogManager.Info($"ファイル[{Path.GetFileName(filePath)}]を削除しました。(Path:{filePath})");
            }
        }
        
        #endregion
    }
}