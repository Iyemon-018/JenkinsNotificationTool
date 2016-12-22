namespace JenkinsNotification.Core.ComponentModels
{
    using System;
    using Services;

    /// <summary>
    /// 汎用的な機能、サービスを持つViewModel クラスです。
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    public abstract class ShareViewModelBase : ViewModelBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        /// <exception cref="System.ArgumentNullException"><see cref="DialogService"/> がnull の場合にスローされます。</exception>
        protected ShareViewModelBase(IDialogService dialogService)
        {
            if (dialogService == null) throw new ArgumentNullException(nameof(dialogService));
            DialogService = dialogService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// ダイアログ サービスを取得します。
        /// </summary>
        protected IDialogService DialogService { get; private set; }

        #endregion
    }
}