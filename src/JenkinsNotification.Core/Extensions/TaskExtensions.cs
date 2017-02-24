namespace JenkinsNotification.Core.Extensions
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 非同期タスク<see cref="Task"/> に関する拡張メソッドを定義します。
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// 現在のタスクにタイムアウトを設定します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="timeout">タイムアウト発生までの時間</param>
        /// <returns>実行結果の非同期タスク</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="self"/> がnull の場合にスローされます。</exception>
        /// <exception cref="System.TimeoutException"><paramref name="timeout"/> までにタスクが完了しなかった場合にスローされます。</exception>
        /// <remarks>
        /// タスクが完了するまでの時間を設定して、その時間までに完了しない場合にタイムアウトを発生させたい場合に使用します。
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// async void Main()
        /// {
        ///     //
        ///     // Write console to "Task Failure....".
        ///     //
        /// 	bool result = false;
        /// 	try
        /// 	{
        /// 		await Task.Run(async () =>
        /// 		{
        ///             // Task wait 5 sec.
        /// 			await Task.Delay(TimeSpan.FromSeconds(5));
        /// 			result = true;
        /// 		})
        ///         // timeout is 3 sec.
        /// 		.Timeout(TimeSpan.FromSeconds(3));
        ///
        /// 		Console.Write("Task Completed.");
        /// 	}
        /// 	catch (TimeoutException tex)
        /// 	{
        /// 		Console.Write($"Task Failure.{tex.Message}");
        /// 	}
        /// }
        /// ]]></code>
        /// </example>
        public static async Task Timeout(this Task self, TimeSpan timeout)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            var delay = Task.Delay(timeout);
            if (await Task.WhenAny(self, delay) == delay)
            {
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// 現在のタスクにタイムアウトを設定します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="timeout">タイムアウト発生までの時間</param>
        /// <returns>実行結果の非同期タスク</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="self"/> がnull の場合にスローされます。</exception>
        /// <exception cref="System.TimeoutException"><paramref name="timeout"/> までにタスクが完了しなかった場合にスローされます。</exception>
        /// <remarks>
        /// タスクが完了するまでの時間を設定して、その時間までに完了しない場合にタイムアウトを発生させたい場合に使用します。
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// async void Main()
        /// {
        ///     //
        ///     // Write console to "Task Failure....".
        ///     //
        /// 	try
        /// 	{
        /// 		result = await Task<bool>.Run(async () =>
        /// 		{
        ///             // Task wait 5 sec.
        /// 			await Task.Delay(TimeSpan.FromSeconds(5));
        /// 			return true;
        /// 		})
        ///         // timeout is 3 sec.
        /// 		.Timeout<bool>(TimeSpan.FromSeconds(3));
        ///
        /// 		Console.Write("Task Completed.");
        /// 	}
        /// 	catch (TimeoutException tex)
        /// 	{
        /// 		Console.Write($"Task Failure.{tex.Message}");
        /// 	}
        /// }
        /// ]]></code>
        /// </example>
        public static async Task<T> Timeout<T>(this Task<T> self, TimeSpan timeout)
        {
            await ((Task) self).Timeout(timeout);
            return await self;
        }
    }
}