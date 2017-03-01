namespace JenkinsNotificationTool.Tests.Core.ViewModels.Api
{
    using System;
    using System.Collections.Generic;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.ViewModels.Api.Converter;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="ApiConverter" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class ApiConverterTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public ApiConverterTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region JobResultTypeStringToEnum method test

        /// <summary>
        /// <see cref="Test_Theory_JobResultTypeStringToEnum"/> で使用するテストデータを取得します。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> GetJobResultTypeStringToEnumTestData()
        {
            yield return new object[]
                             {
                                 $"(正常系) {ApiConverter.JobResultSuccess} を設定した場合、{JobResultType.Success} を返すこと。",
                                 JobResultType.Success,
                                 ApiConverter.JobResultSuccess,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {ApiConverter.JobResultWarning} を設定した場合、{JobResultType.Warning} を返すこと。",
                                 JobResultType.Warning,
                                 ApiConverter.JobResultWarning,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {ApiConverter.JobResultFailure} を設定した場合、{JobResultType.Failure} を返すこと。",
                                 JobResultType.Failure,
                                 ApiConverter.JobResultFailure,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(異常系) string.Emptyを設定した場合、{JobResultType.None} を返すこと。",
                                 JobResultType.None,
                                 string.Empty,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(異常系) nullを設定した場合、{JobResultType.None} を返すこと。",
                                 JobResultType.None,
                                 null,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(異常系) 種別名に該当しない文字列を設定した場合、{JobResultType.None} を返すこと。",
                                 JobResultType.None,
                                 ApiConverter.JobResultSuccess + ApiConverter.JobResultFailure + ApiConverter.JobResultWarning,
                                 null
                             };
        }

        /// <summary>
        /// <see cref="ApiConverter.JobResultTypeStringToEnum" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="expected">期待値</param>
        /// <param name="result">変換対象の文字列</param>
        /// <param name="exceptionType">スローされる例外</param>
        [Theory]
        [MemberData(nameof(GetJobResultTypeStringToEnumTestData))]
        public void Test_Theory_JobResultTypeStringToEnum(string caseName,
                                                          JobResultType expected,
                                                          string result,
                                                          Type exceptionType)
        {
            // arrange
            var resultType = JobResultType.None;

            // act
            var ex = Record.Exception(() => resultType = ApiConverter.JobResultTypeStringToEnum(result));

            // assert
            if (exceptionType == null)
            {
                Assert.Null(ex);
                Assert.Equal(expected, resultType);
            }
            else
            {
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(expected)} : {expected}{Environment.NewLine}" +
                             $" {nameof(result)} : {result}{Environment.NewLine}" +
                             $" {nameof(exceptionType)} : {exceptionType}{Environment.NewLine}");
        }

        #endregion

        #region JobResultTypeToString method test

        /// <summary>
        /// <see cref="Test_Theory_JobResultTypeToString"/> で使用するテストデータを取得します。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> GetJobResultTypeToStringTestData()
        {
            yield return new object[]
                             {
                                 $"(正常系) {JobResultType.Success} を設定すると、{ApiConverter.JobResultSuccess} を返すこと。",
                                 ApiConverter.JobResultSuccess,
                                 JobResultType.Success,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {JobResultType.Failure} を設定すると、{ApiConverter.JobResultFailure} を返すこと。",
                                 ApiConverter.JobResultFailure,
                                 JobResultType.Failure,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {JobResultType.Warning} を設定すると、{ApiConverter.JobResultWarning} を返すこと。",
                                 ApiConverter.JobResultWarning,
                                 JobResultType.Warning,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {JobResultType.None} を設定すると、string.Empty を返すこと。",
                                 string.Empty,
                                 JobResultType.None,
                                 null
                             };
        }

        /// <summary>
        /// <see cref="ApiConverter.JobResultTypeToString" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="expected">期待値</param>
        /// <param name="result">ジョブ結果種別</param>
        /// <param name="exceptionType">スローされる例外</param>
        [Theory]
        [MemberData(nameof(GetJobResultTypeToStringTestData))]
        public void Test_Theory_JobResultTypeToString(string caseName,
                                                      string expected,
                                                      JobResultType result,
                                                      Type exceptionType)
        {
            // arrange
            var resultType = string.Empty;

            // act
            var ex = Record.Exception(() => resultType = ApiConverter.JobResultTypeToString(result));

            // assert
            if (exceptionType == null)
            {
                Assert.Null(ex);
                Assert.Equal(expected, resultType);
            }
            else
            {
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(expected)} : {expected}{Environment.NewLine}" +
                             $" {nameof(result)} : {result}{Environment.NewLine}" +
                             $" {nameof(exceptionType)} : {exceptionType}{Environment.NewLine}");
        }

        #endregion

        #region JobStatusStringToEnum method test

        /// <summary>
        /// <see cref="Test_Theory_JobStatusStringToEnum"/> で使用するテストデータを取得します。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> GetJobStatusStringToEnum()
        {
            yield return new object[]
                             {
                                 $"(正常系) {ApiConverter.JobStatusStart} を設定すると、{JobStatus.Start} を返すこと。",
                                 JobStatus.Start,
                                 ApiConverter.JobStatusStart,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {ApiConverter.JobStatusFailure} を設定すると、{JobStatus.Failure} を返すこと。",
                                 JobStatus.Failure,
                                 ApiConverter.JobStatusFailure,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {ApiConverter.JobStatusSuccess} を設定すると、{JobStatus.Success} を返すこと。",
                                 JobStatus.Success,
                                 ApiConverter.JobStatusSuccess,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(異常系) string.Empty を設定すると、{JobStatus.None} を返すこと。",
                                 JobStatus.None,
                                 string.Empty,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(異常系) null を設定すると、{JobStatus.None} を返すこと。",
                                 JobStatus.None,
                                 null,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(異常系) 該当しない状態種別名称を設定すると、{JobStatus.None} を返すこと。",
                                 JobStatus.None,
                                 ApiConverter.JobStatusFailure + ApiConverter.JobStatusStart + ApiConverter.JobStatusSuccess,
                                 null
                             };
        }

        /// <summary>
        /// <see cref="ApiConverter.JobStatusStringToEnum" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="expected">期待値</param>
        /// <param name="status">変換対象の文字列</param>
        /// <param name="exceptionType">スローされる例外</param>
        [Theory]
        [MemberData(nameof(GetJobStatusStringToEnum))]
        public void Test_Theory_JobStatusStringToEnum(string caseName,
                                                      JobStatus expected,
                                                      string status,
                                                      Type exceptionType)
        {
            // arrange
            var resultType = JobStatus.None;

            // act
            var ex = Record.Exception(() => resultType = ApiConverter.JobStatusStringToEnum(status));

            // assert
            if (exceptionType == null)
            {
                Assert.Null(ex);
                Assert.Equal(expected, resultType);
            }
            else
            {
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(expected)} : {expected}{Environment.NewLine}" +
                             $" {nameof(status)} : {status}{Environment.NewLine}" +
                             $" {nameof(exceptionType)} : {exceptionType}{Environment.NewLine}");
        }

        #endregion

        #region JobStatusToString method test

        /// <summary>
        /// <see cref="Test_Theory_JobStatusToString"/> で使用するテストデータを取得します。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> GetJobStatusToStringTestData()
        {
            yield return new object[]
                             {
                                 $"(正常系) {JobStatus.Success} を設定すると、{ApiConverter.JobStatusSuccess} を返すこと。",
                                 ApiConverter.JobStatusSuccess,
                                 JobStatus.Success,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {JobStatus.Failure} を設定すると、{ApiConverter.JobStatusFailure} を返すこと。",
                                 ApiConverter.JobStatusFailure,
                                 JobStatus.Failure,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {JobStatus.Start} を設定すると、{ApiConverter.JobStatusStart} を返すこと。",
                                 ApiConverter.JobStatusStart,
                                 JobStatus.Start,
                                 null
                             };
            yield return new object[]
                             {
                                 $"(正常系) {JobStatus.None} を設定すると、string.Empty を返すこと。",
                                 string.Empty,
                                 JobStatus.None,
                                 null
                             };
        }

        /// <summary>
        /// <see cref="ApiConverter.JobStatusToString" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="expected">期待値</param>
        /// <param name="status">ジョブ状態</param>
        /// <param name="exceptionType">スローされる例外</param>
        [Theory]
        [MemberData(nameof(GetJobStatusToStringTestData))]
        public void Test_Theory_JobStatusToString(string caseName,
                                                      string expected,
                                                      JobStatus status,
                                                      Type exceptionType)
        {
            // arrange
            var resultType = string.Empty;

            // act
            var ex = Record.Exception(() => resultType = ApiConverter.JobStatusToString(status));

            // assert
            if (exceptionType == null)
            {
                Assert.Null(ex);
                Assert.Equal(expected, resultType);
            }
            else
            {
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(expected)} : {expected}{Environment.NewLine}" +
                             $" {nameof(status)} : {status}{Environment.NewLine}" +
                             $" {nameof(exceptionType)} : {exceptionType}{Environment.NewLine}");
        }

        #endregion
    }
}