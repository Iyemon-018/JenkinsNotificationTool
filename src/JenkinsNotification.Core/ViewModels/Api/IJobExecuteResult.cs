namespace JenkinsNotification.Core.ViewModels.Api
{
    using System;

    /// <summary>
    /// ジョブの実行結果データ インターフェースです。
    /// </summary>
    public interface IJobExecuteResult
    {
        /// <summary>
        /// 受信日時を取得します。
        /// </summary>
        DateTime Received { get; }

        /// <summary>
        /// ジョブ名称を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ビルド番号を取得します。
        /// </summary>
        int BuildNumber { get; }

        /// <summary>
        /// 状態を取得します。
        /// </summary>
        JobStatus Status { get; }

        /// <summary>
        /// 実行結果を取得します。
        /// </summary>
        JobResultType Result { get; }
    }
}