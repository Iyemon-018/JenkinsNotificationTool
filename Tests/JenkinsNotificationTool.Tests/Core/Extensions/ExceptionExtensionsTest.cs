namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.Linq;
    using JenkinsNotification.Core.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="ExceptionExtensions"/> 拡張メソッドのテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class ExceptionExtensionsTest : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public ExceptionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region ToStackTraceMessages Method test

        /// <summary>
        /// <see cref="ExceptionExtensions.ToStackTraceMessages"/> のテストです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・引数がnull の場合、空の文字列コレクションを取得すること。
        /// </remarks>
        [Fact]
        public void Test_ToStackTraceMessages_ArgumentNull()
        {
            // arrange
            Exception target = null;

            // act
            var result = target.ToStackTraceMessages();

            // assert
            Assert.False(result.Any());
            Output.WriteLine($"値がnullの場合のテストです。{Environment.NewLine}結果:{result.Any()}");
        }

        /// <summary>
        /// <see cref="ExceptionExtensions.ToStackTraceMessages"/> のテストです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・InnerExceptionを持たないExceptionからStackTraceが取得できるかどうか。
        /// </remarks>
        [Fact]
        public void Test_ToStackTraceMessages_NoneInnerException()
        {
            // arrange
            Exception target = null;
            try
            {
                throw new ApplicationException("テスト用の例外です。");
            }
            catch (Exception exception)
            {
                target = exception;
            }

            // act
            var result = target.ToStackTraceMessages();

            // assert
            Assert.True(result.Any());
            Output.WriteLine($"Exceptionのみのテストです。{Environment.NewLine}結果:{string.Join(Environment.NewLine, result)}");
        }

        /// <summary>
        /// <see cref="ExceptionExtensions.ToStackTraceMessages"/> のテストです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・InnerExceptionを持つExceptionから内部例外も含めてStackTraceが取得できるかどうか。
        /// </remarks>
        [Fact]
        public void Test_ToStackTraceMessages_HasInnerExceptions()
        {
            // arrange
            Exception target = null;
            try
            {
                try
                {
                    throw new ApplicationException("テストのInnerExceptionです。");
                }
                catch (Exception exception)
                {
                    throw new ApplicationException("テスト用の例外です。", exception);
                }
            }
            catch (Exception exception)
            {
                target = exception;
            }

            // act
            var result = target.ToStackTraceMessages();

            // assert
            Assert.True(result.Any());
            Output.WriteLine($"InnerExceptionを持つExceptionのテストです。{Environment.NewLine}結果:{string.Join(Environment.NewLine, result)}");
        }

        #endregion
    }
}