namespace JenkinsNotification.Core.ComponentModels
{
    using System;
    using JenkinsNotification.Core.Properties;

    /// <summary>
    /// View からViewModel を識別するための属性クラスです。
    /// </summary>
    public sealed class ViewModelAttribute : Attribute
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewModelType">DataContext にバインドするViewModel の型</param>
        /// <exception cref="System.TypeAccessException">
        /// <paramref name="viewModelType"/> に<see cref="ViewModelBase"/> を継承していない型を設定した場合にスローされます。
        /// </exception>
        public ViewModelAttribute(Type viewModelType)
        {
            if (!viewModelType.IsSubclassOf(typeof(ViewModelBase))) throw new TypeAccessException(Resources.ViewModelAttributeUnmatchTypeErrorMessage);
            ViewModelType = viewModelType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// DataContext にバインドするViewModel の型を取得します。
        /// </summary>
        public Type ViewModelType { get; }

        #endregion
    }
}