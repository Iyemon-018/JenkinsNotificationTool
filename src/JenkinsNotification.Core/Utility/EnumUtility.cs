namespace JenkinsNotification.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using JenkinsNotification.Core.Extensions;
    using Properties;

    /// <summary>
    /// 列挙体に関するユーティリティ機能クラスです。
    /// </summary>
    public static class EnumUtility
    {
        #region Methods

        /// <summary>
        /// 列挙体の全ての列挙子を持つ<see cref="IEnumerable{TEnum}"/>に変換します。
        /// </summary>
        /// <typeparam name="TEnum">変換対象の列挙体</typeparam>
        /// <returns><typeparamref name="TEnum"/> の全ての列挙子コレクション</returns>
        /// <exception cref="System.InvalidOperationException"><typeparamref name="TEnum"/> が列挙体の型でない場合にスローされます。</exception>
        public static IEnumerable<TEnum> ToEnumerable<TEnum>() where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException(Resources.EnumTypeUnmatchMessage);
            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>();
        }

        /// <summary>
        /// 列挙体の全ての列挙子を持つ<see cref="ObservableCollection{TEnum}"/> に変換します。
        /// </summary>
        /// <typeparam name="TEnum">変換対象の列挙体</typeparam>
        /// <returns><typeparamref name="TEnum"/> の全ての列挙子コレクション</returns>
        /// <exception cref="System.InvalidOperationException"><typeparamref name="TEnum"/> が列挙体の型でない場合にスローされます。</exception>
        public static ObservableCollection<TEnum> ToObservableCollection<TEnum>() where TEnum : struct
        {
            return ToEnumerable<TEnum>().ToObservableCollection();
        }

        #endregion
    }
}