namespace JenkinsNotification.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ComponentModels;

    /// <summary>
    /// <see cref="ViewModelBase"/> のユーティリティ機能クラスです。
    /// </summary>
    public static class ViewModelUtility
    {
        #region Methods

        /// <summary>
        /// 指定した<see cref="ViewModelBase" /> コレクションのうち条件に一致するオブジェクトのみ検証を行います。
        /// </summary>
        /// <param name="viewModels">検証対象の<see cref="ViewModelBase" /> コレクション</param>
        /// <param name="predicate"><paramref name="viewModels" /> のうち検証を行いたい<see cref="ViewModelBase" /> を抽出するための条件</param>
        /// <returns>検証結果(検証結果が１件でも異常であればfalseを返します。)</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> がnull の場合にスローされます。</exception>
        public static bool Validates(IEnumerable<ViewModelBase> viewModels, Func<ViewModelBase, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return Validates(viewModels.Where(predicate));
        }

        /// <summary>
        /// 指定した<see cref="ViewModelBase"/> コレクションを全て検証します。
        /// </summary>
        /// <param name="viewModels">検証対象の<see cref="ViewModelBase" /> コレクション</param>
        /// <returns>検証結果(検証結果が１件でも異常であればfalseを返します。)</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="viewModels"/> がnull の場合にスローされます。</exception>
        public static bool Validates(IEnumerable<ViewModelBase> viewModels)
        {
            if (viewModels == null) throw new ArgumentNullException(nameof(viewModels));
            using (TimeTracer.StartNew($"ViewModel コレクションの検証を行う。(Count:{viewModels.Count()})"))
            {
                foreach (var viewModel in viewModels)
                {
                    viewModel.Validate();
                }
            }

            return viewModels.Any(x => x.HasErrors);
        }

        #endregion
    }
}