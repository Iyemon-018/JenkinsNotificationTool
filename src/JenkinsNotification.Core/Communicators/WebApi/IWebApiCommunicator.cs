namespace JenkinsNotification.Core.Communicators.WebApi
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// WebAPIを実行するための通信インターフェースです。
    /// </summary>
    public interface IWebApiCommunicator
    {
        /// <summary>
        /// API実行のための資格情報を設定します。
        /// </summary>
        /// <param name="userName">ユーザー名</param>
        /// <param name="password">パスワード</param>
        void SetCredential(string userName, string password);

        /// <summary>
        /// APIの実行タイムアウトを設定します。
        /// </summary>
        /// <param name="timeout">タイムアウト</param>
        void SetTimeout(TimeSpan timeout);

        /// <summary>
        /// リクエストを非同期で取得します。
        /// </summary>
        /// <param name="url">リクエストを実行するURL</param>
        /// <param name="timeoutAction">タイムアウト発生時のアクション デリゲート</param>
        /// <param name="exceptionAction">例外発生時のアクション デリゲート</param>
        /// <returns>リクエストデータ</returns>
        Task<string> GetRequest(string url, Action timeoutAction, Action<Exception> exceptionAction);
    }
}