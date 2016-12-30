namespace JenkinsNotification.Core.Executers
{
    using System;
    using Extensions;
    using Jenkins.Api;
    using Logs;
    using ViewModels.Api;

    /// <summary>
    /// Jenkins のジョブ結果を蓄積するタスク実行クラスです。
    /// </summary>
    /// <seealso cref="IExecuter" />
    public class JobResultExecuter : IExecuter
    {
        #region Fields

        /// <summary>
        /// ジョブ結果
        /// </summary>
        private JobExecuteResult _result;

        #endregion

        #region Methods

        /// <summary>
        /// タスクが実行できるかどうかを判定します。
        /// </summary>
        /// <param name="message">判定対象の文字列データ</param>
        /// <returns>true の場合、タスクを実行することができます。false の場合、タスクは実行することができません。</returns>
        public bool CanExecute(string message)
        {
            try
            {
                _result = message.JsonSerialize<JobExecuteResult>();
                LogManager.Info("Jenkins ジョブ結果を受け取った。");
                return true;
            }
            catch (Exception e)
            {
                LogManager.Error("変換に失敗した。", e);
                return false;
            }
        }

        /// <summary>
        /// タスクが実行できるかどうかを判定します。
        /// </summary>
        /// <param name="data">判定対象のバイト配列データ</param>
        /// <returns>true の場合、タスクを実行することができます。false の場合、タスクは実行することができません。</returns>
        /// <exception cref="System.NotImplementedException">このクラスでは使用できません。</exception>
        public bool CanExecute(byte[] data)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 割り当てられているタスクを実行します。
        /// </summary>
        public void Execute()
        {
            LogManager.Info("Jenkins ジョブ結果データを蓄積する。");
            var data = _result.Map<JobExecuteResultViewModel>();
            data.Received = DateTime.Now;
            DataStore.Instance.AddJobResult(data);
        }

        #endregion
    }
}