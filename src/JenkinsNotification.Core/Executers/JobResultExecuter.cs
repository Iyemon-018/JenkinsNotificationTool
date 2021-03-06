﻿namespace JenkinsNotification.Core.Executers
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
        /// データ蓄積領域
        /// </summary>
        private readonly IDataStore _dataStore;

        /// <summary>
        /// ジョブ結果
        /// </summary>
        private JobExecuteResult _result;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataStore">データ蓄積領域</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="dataStore"/> がnull の場合にスローされます。</exception>
        public JobResultExecuter(IDataStore dataStore)
        {
            if (dataStore == null) throw new ArgumentNullException(nameof(dataStore));
            _dataStore = dataStore;
        }

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
            }
            catch (Exception e)
            {
                LogManager.Error("変換に失敗した。", e);
                return false;
            }

            LogManager.Info("Jenkins ジョブ結果を受け取った。");
            return true;
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
            _dataStore.JobResults.Add(data);
        }

        #endregion
    }
}