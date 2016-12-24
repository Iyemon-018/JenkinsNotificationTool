namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Extensions;
    using Xunit;
    using Xunit.Abstractions;
    using TaskExtensions = JenkinsNotification.Core.Extensions.TaskExtensions;

    /// <summary>
    /// <see cref="TaskExtensions" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class TaskExtensionsTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public TaskExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Timeout method test

        /// <summary>
        /// <see cref="JenkinsNotification.Core.Extensions.TaskExtensions.Timeout"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・対象のタスクがnull の場合、<see cref="ArgumentNullException"/> をスローすること。
        /// </remarks>
        [Fact]
        public void Test_Timeout_Failed_TaskNull()
        {
            // arrange
            var dummyTask = (Task)null;
            var task = TaskExtensions.Timeout(dummyTask, TimeSpan.FromSeconds(2));

            // act
            var ex = Assert.Throws<AggregateException>(() => task.Wait());

            // assert
            Assert.NotNull(ex);
            var exceptions = ex.Flatten().InnerExceptions;
            Assert.True(exceptions.Count == 1);
            Assert.IsType<ArgumentNullException>(exceptions.First());
            Output.WriteLine("(異常系) 対象のタスクがnull の場合、ArgumentNullException をスローすること。");
        }

        /// <summary>
        /// <see cref="JenkinsNotification.Core.Extensions.TaskExtensions.Timeout"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・対象のタスクが完了する前にタイムアウト次官になると、<see cref="TimeoutException"/> をスローすること。
        /// </remarks>
        [Fact]
        public void Test_Timeout_Success_Timeout()
        {
            // arrange
            var result = false;
            var task = Task.Run(async () =>
                                {
                                    await Task.Delay(TimeSpan.FromSeconds(2));
                                    result = true;
                                })
                           .Timeout(TimeSpan.FromSeconds(1));

            // act
            var ex = Assert.Throws<AggregateException>(() => task.Wait());

            // assert
            Assert.NotNull(ex);
            var exceptions = ex.Flatten().InnerExceptions;
            Assert.NotNull(exceptions);
            Assert.True(exceptions.Count == 1);
            Assert.IsType<TimeoutException>(exceptions.First());
            Assert.False(result);
            Output.WriteLine("(正常系) 対象のタスクが完了する前にタイムアウト次官になると、TimeoutException をスローすること。");
        }

        /// <summary>
        /// <see cref="JenkinsNotification.Core.Extensions.TaskExtensions.Timeout"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・対象のタスクがタイムアウト時間までに完了すると例外をスローせずに完了すること。
        /// </remarks>
        [Fact]
        public void Test_Timeout_Success()
        {
            // arrange
            var result = false;
            var task = Task.Run(async () =>
                                {
                                    await Task.Delay(TimeSpan.FromSeconds(1));
                                    result = true;
                                })
                           .Timeout(TimeSpan.FromSeconds(2));

            // act
            var ex = Record.Exception(() => task.Wait());

            // assert
            Assert.Null(ex);
            Assert.True(result);
            Output.WriteLine("(正常系) 対象のタスクがタイムアウト時間までに完了すると例外をスローせずに完了すること。");
        }

        #endregion

        #region Timeout<T> method test

        /// <summary>
        /// <see cref="JenkinsNotification.Core.Extensions.TaskExtensions.Timeout"/> のテストメソッドです。(異常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・対象のタスクがnull の場合、<see cref="ArgumentNullException"/> をスローすること。
        /// </remarks>
        [Fact]
        public void Test_Timeout_T_Failed_TaskNull()
        {
            // arrange
            var dummyTask = (Task<bool>) null;
            var task = dummyTask.Timeout(TimeSpan.FromSeconds(2));

            // act
            var ex = Assert.Throws<AggregateException>(() => task.Wait());

            // assert
            Assert.NotNull(ex);
            var exceptions = ex.Flatten().InnerExceptions;
            Assert.True(exceptions.Count == 1);
            Assert.IsType<ArgumentNullException>(exceptions.First());
            Output.WriteLine("(異常系) 対象のタスクがnull の場合、ArgumentNullException をスローすること。");
        }

        /// <summary>
        /// <see cref="JenkinsNotification.Core.Extensions.TaskExtensions.Timeout"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・対象のタスクが完了する前にタイムアウト次官になると、<see cref="TimeoutException"/> をスローすること。
        /// </remarks>
        [Fact]
        public void Test_Timeout_T_Success_Timeout()
        {
            // arrange
            var result = false;
            var task = Task.Run(async () =>
                                      {
                                          await Task.Delay(TimeSpan.FromSeconds(2));
                                          return true;
                                      })
                           .Timeout(TimeSpan.FromSeconds(1));

            // act
            var ex = Assert.ThrowsAsync<AggregateException>(async () => result = task.Result);

            // assert
            Assert.NotNull(ex);
            var exceptions = ex.Result.Flatten().InnerExceptions;
            Assert.NotNull(exceptions);
            Assert.True(exceptions.Count == 1);
            Assert.IsType<TimeoutException>(exceptions.First());
            Assert.False(result);
            Output.WriteLine("(正常系) 対象のタスクが完了する前にタイムアウト次官になると、TimeoutException をスローすること。");
        }

        /// <summary>
        /// <see cref="JenkinsNotification.Core.Extensions.TaskExtensions.Timeout"/> のテストメソッドです。(正常系)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・対象のタスクがタイムアウト時間までに完了すると例外をスローせずに完了すること。
        /// </remarks>
        [Fact]
        public async void Test_Timeout_T_Success()
        {
            // arrange
            var result = false;

            // act
            result = await Task<bool>.Run(async () =>
                                          {
                                              await Task.Delay(TimeSpan.FromSeconds(1));
                                              return true;
                                          })
                                     .Timeout(TimeSpan.FromSeconds(2));

            // assert
            Assert.True(result);
            Output.WriteLine("(正常系) 対象のタスクがタイムアウト時間までに完了すると例外をスローせずに完了すること。");
        }

        #endregion
    }
}