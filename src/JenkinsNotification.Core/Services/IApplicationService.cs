namespace JenkinsNotification.Core.Services
{
    /// <summary>
    /// このアプリケーション インスタンスの制御サービス インターフェースです。
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// シャットダウンします。
        /// </summary>
        void Shutdown();
    }
}