namespace JenkinsNotification.Core.Communicators
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// WebAPIを実行するための通信インターフェースです。
    /// </summary>
    public interface IWebApiCommunicator
    {
        /// <summary>
        /// URLを設定します。
        /// </summary>
        /// <param name="url">URL</param>
        void SetUrl(string url);

        /// <summary>
        /// APIの実行タイムアウトを設定します。
        /// </summary>
        /// <param name="timeout">タイムアウト</param>
        void SetTimeout(TimeSpan timeout);

        /// <summary>
        /// リクエストを非同期で取得します。
        /// </summary>
        /// <returns>リクエストデータ</returns>
        Task<string> GetRequest();
    }
}