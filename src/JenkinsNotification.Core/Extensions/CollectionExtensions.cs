namespace JenkinsNotification.Core.Extensions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// コレクションに関する拡張メソッドを定義します。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// コレクション要素を<see cref="ObservableCollection{T}"/> に変換します。
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <returns>
        /// 変換結果<para/>
        /// 現在のコレクション要素がnull の場合、空の<see cref="ObservableCollection{T}"/> を返します。
        /// </returns>
        public static ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> self)
        {
            return self == null ? new ObservableCollection<T>() : new ObservableCollection<T>(self);
        }
    }
}