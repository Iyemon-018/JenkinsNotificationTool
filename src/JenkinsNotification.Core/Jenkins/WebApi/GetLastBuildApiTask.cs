namespace JenkinsNotification.Core.Jenkins.WebApi
{
    using System;
    using JenkinsNotification.Core.Communicators.WebApi;

    /// <summary>
    /// 最終ビルド情報を取得するWebAPI実行タスク クラスです。
    /// </summary>
    /// <remarks>
    /// 指定したジョブの最終ビルド情報を取得し、
    /// </remarks>
    /// <seealso cref="IWebApiTask" />
    public class GetLastBuildApiTask : IWebApiTask
    {
        public string GetUrl()
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteReceivedResponse(string response)
        {
            throw new System.NotImplementedException();
        }

        public void DetectedTimeout()
        {
            throw new System.NotImplementedException();
        }

        public void DetectedError(Exception exception)
        {
            throw new System.NotImplementedException();
        }
    }
}