namespace JenkinsNotification.Core.Utility
{
    using System;
    using System.Windows;

    /// <summary>
    /// スレッドに関するユーティリティ機能クラスです。
    /// </summary>
    public static class ThreadUtility
    {
        /// <summary>
        /// UIスレッドでアクションハンドラを実行します。
        /// </summary>
        /// <param name="execute">UIスレッドで実行するハンドラ</param>
        public static void ExecuteUiThread(Action execute)
        {
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(execute));
        }
    }
}