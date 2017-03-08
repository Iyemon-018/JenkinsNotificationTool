namespace JenkinsNotification.Core.Communicators.WebApi
{
    using System;

    /// <summary>
    /// WebAPIを実行、レスポンスを処理するためのインターフェースです。
    /// </summary>
    public interface IWebApiTask
    {
        /// <summary>
        /// APIを実行するためのURLを取得します。
        /// </summary>
        /// <returns>URL</returns>
        string GetUrl();

        /// <summary>
        /// レスポンス取得処理を実行します。
        /// </summary>
        /// <param name="response">レスポンス データ</param>
        void ExecuteReceivedResponse(string response);

        /// <summary>
        /// API実行タイムアウトを検出しました。
        /// </summary>
        void DetectedTimeout();

        /// <summary>
        /// API実行エラーを検出しました。
        /// </summary>
        /// <param name="exception">キャッチした例外インスタンス</param>
        void DetectedError(Exception exception);
    }
}