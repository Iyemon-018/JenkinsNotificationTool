namespace JenkinsNotificationTool.Tests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Executers;
    using Executers;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="DataManager" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    //public class DataManagerTests : TestBase
    //{
    //    #region Ctor

    //    /// <summary>
    //    /// コンストラクタ
    //    /// </summary>
    //    /// <param name="output">テスト出力ヘルパー</param>
    //    public DataManagerTests(ITestOutputHelper output) : base(output)
    //    {
    //    }

    //    #endregion

    //    #region AddTask method test

    //    /// <summary>
    //    /// <see cref="Test_AddTask_Thoey"/> のテスト用データです。
    //    /// </summary>
    //    /// <returns>テスト用データ</returns>
    //    public static IEnumerable<object[]> GetAddTaskData()
    //    {
    //        yield return new object[] {"(正常系) インスタンス化されたExecuter を追加して例外がスローされないこと。", null, new JobResultExecuter()};
    //        yield return new object[] {"(異常系) 引数にnull を設定すると、ArgumentNullException がスローされること。", typeof(ArgumentNullException), null};
    //    }

    //    /// <summary>
    //    /// <see cref="DataManager.AddTask" /> をテストします。
    //    /// </summary>
    //    /// <param name="caseName">テスト内容</param>
    //    /// <param name="exceptionType">スローされる例外の型</param>
    //    /// <param name="task">タスク</param>
    //    [Theory]
    //    [MemberData(nameof(GetAddTaskData))]
    //    public void Test_AddTask_Thoey(string caseName, Type exceptionType, IExecuter task)
    //    {
    //        // arrange
    //        Exception ex;
    //        var manager = new DataManager();

    //        // act
    //        // assert
    //        if (exceptionType != null)
    //        {
    //            ex = Assert.Throws(exceptionType, () => manager.AddTask(task));
    //            Assert.NotNull(ex);
    //            Assert.IsType(exceptionType, ex);
    //            Assert.False(manager.HasTask);
    //        }
    //        else
    //        {
    //            ex = Record.Exception(() => manager.AddTask(task));
    //            Assert.Null(ex);
    //            Assert.True(manager.HasTask);
    //        }

    //        Output.WriteLine($"{caseName}{Environment.NewLine}スローされた例外:{ex}, 期待値の例外:{exceptionType}");
    //    }

    //    #endregion

    //    #region AddTasks method test

    //    /// <summary>
    //    /// <see cref="Test_AddTasks_Theory"/> で使用するテストデータを取得します。
    //    /// </summary>
    //    /// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
    //    public static IEnumerable<object[]> GetAddTasksData()
    //    {
    //        yield return new object[] {"(正常系) コレクションを追加して例外をスローしないこと。", null, new List<IExecuter> {new JobResultExecuter(), new JobResultExecuter()},}
    //            ;
    //        yield return
    //            new object[] {"(異常系) 空のコレクションを追加するとArgumentNullException をスローすること。", typeof(ArgumentNullException), Enumerable.Empty<IExecuter>(),};
    //        yield return new object[] {"(異常系) 引数にnull 設定するとArgumentNullException をスローすること。", typeof(ArgumentNullException), null,};
    //    }

    //    /// <summary>
    //    /// <see cref="DataManager.AddTasks" /> をテストします。
    //    /// </summary>
    //    /// <param name="caseName">テスト内容</param>
    //    /// <param name="exceptionType">スローされる例外の型</param>
    //    /// <param name="tasks">タスク</param>
    //    [Theory]
    //    [MemberData(nameof(GetAddTasksData))]
    //    public void Test_AddTasks_Theory(string caseName, Type exceptionType, IEnumerable<IExecuter> tasks)
    //    {
    //        // arrange
    //        Exception ex;
    //        var manager = new DataManager();

    //        // act
    //        // assert
    //        if (exceptionType == null)
    //        {
    //            ex = Record.Exception(() => manager.AddTasks(tasks));
    //            Assert.Null(ex);
    //            Assert.True(manager.HasTask);
    //        }
    //        else
    //        {
    //            ex = Assert.Throws(exceptionType, () => manager.AddTasks(tasks));
    //            Assert.NotNull(ex);
    //            Assert.IsType(exceptionType, ex);
    //            Assert.False(manager.HasTask);
    //        }

    //        Output.WriteLine($"{caseName}{Environment.NewLine}スローされた例外:{ex}, 期待値の例外:{exceptionType}");
    //    }


    //    #endregion

    //    #region ReceivedData method test

    //    // １．引数がnull
    //    // ２．引数がEmpty
    //    // ３．Tasksが空
    //    // ４．該当するタスクがない。(Tasksが複数)
    //    // ５．該当するタスクが存在する。(Tasksが複数)

    //    /// <summary>
    //    /// <see cref="DataManager.ReceivedData"/> のユニットテスト検証用の値
    //    /// </summary>
    //    private static bool _detectedReceivedData = false;

    //    /// <summary>
    //    /// <see cref="Test_ReceivedData_Theory"/> で使用するテストデータを取得します。
    //    /// </summary>
    //    /// <returns>テストデータ</returns>
    //    public static IEnumerable<object[]> GetReceivedDataTestData()
    //    {
    //        var detectData = new byte[] {0x01, 0x02, 0x03,};
    //        var task1 = new MockExecuter(x => x == new byte[] {0x00, 0x00, 0x01,}, () => _detectedReceivedData = true);
    //        var task2 = new MockExecuter(x => x == detectData, () => _detectedReceivedData = true);
    //        var tasks = new List<IExecuter> {task1, task2};

    //        yield return new object[] {"(異常系) 引数がnull の場合、タスクが実行されず例外もスローされないこと。", null, false, tasks, null};
    //        yield return new object[] {"(異常系) 引数がEmpty の場合、タスクが実行されず例外もスローされないこと。", null, false, tasks, Enumerable.Empty<byte>()};
    //        yield return new object[] {"(異常系) Tasks が空の場合、タスクが実行されず例外もスローされないこと。。", null, false, null, detectData};
    //        yield return new object[] {"(正常系) 該当するタスクが存在しない場合、タスクが実行されないこと。", null, false, tasks, new byte[] {0x00, 0x01,}};
    //        yield return new object[] {"(正常系) 該当するタスクを検出した場合、そのタスクが実行されること。", null, true, tasks, detectData};
    //    }

    //    /// <summary>
    //    /// <see cref="DataManager.ReceivedData" /> をテストします。
    //    /// </summary>
    //    /// <param name="caseName">テスト内容</param>
    //    /// <param name="exceptionType">発生するであろう例外の型</param>
    //    /// <param name="expected">期待する結果値</param>
    //    /// <param name="tasks">実行タスクコレクション</param>
    //    /// <param name="data">検証対象の値</param>
    //    [Theory]
    //    [MemberData(nameof(GetReceivedDataTestData))]
    //    public void Test_ReceivedData_Theory(string caseName, Type exceptionType, bool expected, IEnumerable<IExecuter> tasks, byte[] data)
    //    {
    //        // arrange
    //        Exception ex;
    //        var dataManager = new DataManager();
    //        if (tasks != null && tasks.Any())
    //        {
    //            dataManager.AddTasks(tasks);
    //        }

    //        // act

    //        // assert
    //        if (exceptionType == null)
    //        {
    //            ex = Record.Exception(() => dataManager.ReceivedData(data));
    //            Assert.Null(ex);
    //        }
    //        else
    //        {
    //            ex = Assert.Throws(exceptionType, () => dataManager.ReceivedData(data));
    //            Assert.NotNull(ex);
    //            Assert.IsType(exceptionType, ex);
    //        }
    //        Assert.Equal(expected, _detectedReceivedData);
    //        Output.WriteLine(
    //            $"{caseName}{Environment.NewLine}" +
    //            $"期待値:{expected}, 結果:{_detectedReceivedData}{Environment.NewLine}");
    //    }

    //    #endregion

    //    #region ReceivedMessage method test
        
    //    /// <summary>
    //    /// <see cref="DataManager.ReceivedMessage"/> のユニットテスト検証用の値
    //    /// </summary>
    //    private static bool _detectedReceivedMessage = false;

    //    /// <summary>
    //    /// <see cref="Test_ReceivedMessage_Theory"/> で使用するテストデータを取得します。
    //    /// </summary>
    //    /// <returns>テストデータ</returns>
    //    public static IEnumerable<object[]> GetReceivedMessageTestData()
    //    {
    //        var detectData = DateTime.Now.ToString("O");
    //        var task1 = new MockExecuter(x => x == DateTime.Today.AddDays(-10).ToString("O"), () => _detectedReceivedMessage = true);
    //        var task2 = new MockExecuter(x => x == detectData, () => _detectedReceivedMessage = true);
    //        var tasks = new List<IExecuter> {task1, task2};

    //        yield return new object[] {"(異常系) 引数がnull の場合、タスクが実行されず例外もスローされないこと。", null, false, tasks, null};
    //        yield return new object[] {"(異常系) 引数がEmpty の場合、タスクが実行されず例外もスローされないこと。", null, false, tasks, string.Empty};
    //        yield return new object[] {"(異常系) Tasks が空の場合、タスクが実行されず例外もスローされないこと。。", null, false, null, detectData};
    //        yield return new object[] {"(正常系) 該当するタスクが存在しない場合、タスクが実行されないこと。", null, false, tasks, "test message"};
    //        yield return new object[] {"(正常系) 該当するタスクを検出した場合、そのタスクが実行されること。", null, true, tasks, detectData};
    //    }

    //    /// <summary>
    //    /// <see cref="DataManager.ReceivedMessage" /> をテストします。
    //    /// </summary>
    //    /// <param name="caseName">テスト内容</param>
    //    /// <param name="exceptionType">発生するであろう例外の型</param>
    //    /// <param name="expected">期待する結果値</param>
    //    /// <param name="tasks">実行タスクコレクション</param>
    //    /// <param name="message">検証対象の値</param>
    //    [Theory]
    //    [MemberData(nameof(GetReceivedMessageTestData))]
    //    public void Test_ReceivedMessage_Theory(string caseName, Type exceptionType, bool expected, IEnumerable<IExecuter> tasks, string message)
    //    {
    //        // arrange
    //        Exception ex;
    //        var dataManager = new DataManager();
    //        if (tasks != null && tasks.Any())
    //        {
    //            dataManager.AddTasks(tasks);
    //        }

    //        // act

    //        // assert
    //        if (exceptionType == null)
    //        {
    //            ex = Record.Exception(() => dataManager.ReceivedMessage(message));
    //            Assert.Null(ex);
    //        }
    //        else
    //        {
    //            ex = Assert.Throws(exceptionType, () => dataManager.ReceivedMessage(message));
    //            Assert.NotNull(ex);
    //            Assert.IsType(exceptionType, ex);
    //        }
    //        Assert.Equal(expected, _detectedReceivedMessage);
    //        Output.WriteLine(
    //            $"{caseName}{Environment.NewLine}" +
    //            $"期待値:{expected}, 結果:{_detectedReceivedData}{Environment.NewLine}");
    //    }

    //    #endregion
    //}
}