namespace JenkinsNotification.Core.Services
{
    using System.Windows;

    /// <summary>
    /// 画面表示サービス インターフェースです。
    /// </summary>
    public interface IViewService
    {
        /// <summary>
        /// 指定した画面を表示します。
        /// </summary>
        /// <param name="key">表示する画面識別子</param>
        void Show(ScreenKey key);

        /// <summary>
        /// 指定した画面識別子の<see cref="Window"/> を取得します。
        /// </summary>
        /// <param name="key">画面識別子</param>
        /// <returns>
        /// 該当の画面オブジェクト<para/>
        /// 該当する画面が表示されていない場合は、null を返します。
        /// </returns>
        Window GetView(ScreenKey key);
    }
}