namespace JenkinsNotification.Core.Extensions
{
    using System.Collections.Generic;
    using AutoMapper;

    /// <summary>
    /// AutoMapper 用の拡張メソッドを定義します。
    /// </summary>
    public static class AutoMapperExtensions
    {
        #region Methods

        /// <summary>
        /// このインスタンスの値をマッピングした別のインスタンスを生成します。
        /// </summary>
        /// <typeparam name="TDestination">変換先の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <returns>マッピング結果</returns>
        public static TDestination Map<TDestination>(this object self)
        {
            return Mapper.Map<TDestination>(self);
        }

        /// <summary>
        /// このインスタンスの値を別のインスタンスにマッピングします。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="destination">マッピング先のインスタンス</param>
        public static void Map(this object self, object destination)
        {
            Mapper.Map(self, destination);
        }

        /// <summary>
        /// このコレクション インスタンスの値をマッピングした別のコレクション インスタンスを生成します。
        /// </summary>
        /// <typeparam name="TDestination">変換後の要素の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <returns>マッピング結果</returns>
        public static IEnumerable<TDestination> MapCollection<TDestination>(this IEnumerable<object> self)
        {
            return Mapper.Map<IEnumerable<TDestination>>(self);
        }

        #endregion
    }
}