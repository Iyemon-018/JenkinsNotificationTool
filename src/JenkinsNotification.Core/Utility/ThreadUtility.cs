namespace JenkinsNotification.Core.Utility
{
    using System;
    using System.Threading;

    /// <summary>
    /// スレッドに関するユーティリティ機能クラスです。
    /// </summary>
    public static class ThreadUtility
    {
        #region Const

        /// <summary>
        /// 同期コンテキスト
        /// </summary>
        private static SynchronizationContext _context;

        #endregion

        #region Methods

        /// <summary>
        /// UIスレッドでアクションハンドラを実行します。
        /// </summary>
        /// <param name="execute">UIスレッドで実行するハンドラ</param>
        public static void ExecuteUiThread(Action execute)
        {
            var dispatcher = Constants.RootDispatcher;
            dispatcher.Invoke(execute);
        }

        /// <summary>
        /// 同期コンテキストを初期化します。<para/>
        /// 初期化はUIスレッド上で実行してください。
        /// </summary>
        public static void InitializeSynchronizationContext()
        {
            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            if (_context == null)
            {
                _context = SynchronizationContext.Current;
            }
        }

        /// <summary>
        /// UIスレッド上でアクションを実行します。
        /// </summary>
        /// <param name="execute">ポストするアクション</param>
        public static void PostUi(Action execute)
        {
            _context.Post(x => execute.Invoke(), null);
        }

        #endregion
    }
}