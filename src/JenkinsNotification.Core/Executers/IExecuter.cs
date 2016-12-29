namespace JenkinsNotification.Core.Executers
{
    /// <summary>
    /// タスクを実行するための処理実行インターフェースです。
    /// </summary>
    public interface IExecuter
    {
        /// <summary>
        /// タスクが実行できるかどうかを判定します。
        /// </summary>
        /// <param name="message">判定対象の文字列データ</param>
        /// <returns>true の場合、タスクを実行することができます。false の場合、タスクは実行することができません。</returns>
        bool CanExecute(string message);

        /// <summary>
        /// タスクが実行できるかどうかを判定します。
        /// </summary>
        /// <param name="data">判定対象のバイト配列データ</param>
        /// <returns>true の場合、タスクを実行することができます。false の場合、タスクは実行することができません。</returns>
        bool CanExecute(byte[] data);

        /// <summary>
        /// 割り当てられているタスクを実行します。
        /// </summary>
        void Execute();
    }
}