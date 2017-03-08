namespace JenkinsNotification.Core.Jenkins.WebApi
{
    using System;
    using JenkinsNotification.Core.Communicators.WebApi;

    /// <summary>
    /// ジョブの詳細情報を取得するためのWebAPIタスク クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.Communicators.WebApi.IWebApiTask" />
    public class GetJobDetailApiTask : IWebApiTask
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