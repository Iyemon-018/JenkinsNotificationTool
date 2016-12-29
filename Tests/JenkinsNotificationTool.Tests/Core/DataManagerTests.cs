namespace JenkinsNotificationTool.Tests.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Executers;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="DataManager" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class DataManagerTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public DataManagerTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region AddTask method test

        /// <summary>
        /// <see cref="Test_AddTask_Thoey"/> のテスト用データです。
        /// </summary>
        /// <returns>テスト用データ</returns>
        public static IEnumerable<object[]> GetAddTaskData()
        {
            yield return new object[] {"(正常系) インスタンス化されたExecuter を追加して例外がスローされないこと。", null, new JobResultExecuter() };
            yield return new object[] {"(異常系) 引数にnull を設定すると、ArgumentNullException がスローされること。", typeof(ArgumentNullException), null};
        }

        /// <summary>
        /// <see cref="DataManager.AddTask" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="exceptionType">スローされる例外の型</param>
        /// <param name="task">タスク</param>
        [Theory]
        [MemberData(nameof(GetAddTaskData))]
        public void Test_AddTask_Thoey(string caseName, Type exceptionType, IExecuter task)
        {
            // arrange
            Exception ex;
            var manager = new DataManager();

            // act
            // assert
            if (exceptionType != null)
            {
                ex = Assert.Throws(exceptionType, () => manager.AddTask(task));
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
                Assert.False(manager.HasTask);
            }
            else
            {
                ex = Record.Exception(() => manager.AddTask(task));
                Assert.Null(ex);
                Assert.True(manager.HasTask);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}スローされた例外:{ex}, 期待値の例外:{exceptionType}");
        }

        #endregion

        #region AddTasks method test

        /// <summary>
        /// <see cref="Test_AddTasks_Theory"/> で使用するテストデータを取得します。
        /// </summary>
        /// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
        public static IEnumerable<object[]> GetAddTasksData()
        {
            yield return new object[] { "(正常系) コレクションを追加して例外をスローしないこと。", null, new List<IExecuter> {new JobResultExecuter(), new JobResultExecuter() }, };
            yield return new object[] { "(異常系) 空のコレクションを追加するとArgumentNullException をスローすること。", typeof(ArgumentNullException), Enumerable.Empty<IExecuter>(), };
            yield return new object[] { "(異常系) 引数にnull 設定するとArgumentNullException をスローすること。", typeof(ArgumentNullException), null, };
        }

        /// <summary>
        /// <see cref="DataManager.AddTasks" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="exceptionType">スローされる例外の型</param>
        /// <param name="tasks">タスク</param>
        [Theory]
        [MemberData(nameof(GetAddTasksData))]
        public void Test_AddTasks_Theory(string caseName, Type exceptionType, IEnumerable<IExecuter> tasks)
        {
            // arrange
            Exception ex;
            var manager = new DataManager();

            // act
            // assert
            if (exceptionType == null)
            {
                ex = Record.Exception(() => manager.AddTasks(tasks));
                Assert.Null(ex);
                Assert.True(manager.HasTask);
            }
            else
            {
                ex = Assert.Throws(exceptionType, () => manager.AddTasks(tasks));
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
                Assert.False(manager.HasTask);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}スローされた例外:{ex}, 期待値の例外:{exceptionType}");
        }
        

        #endregion
    }
}