namespace JenkinsNotification.Core.Extensions
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Json 形式のデータをシリアライズするための拡張メソッドを定義します。
    /// </summary>
    public static class JsonSerializer
    {
        /// <summary>
        /// Json形式のバッファーをシリアライズします。
        /// </summary>
        /// <typeparam name="T">シリアライズ対象の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <returns>変換結果</returns>
        public static T JsonSerialize<T>(this byte[] self) where T : class
        {
            T result;

            using (var ms = new MemoryStream(self))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                result = serializer.ReadObject(ms) as T;
            }

            return result;
        }

        /// <summary>
        /// 指定したエンコードでJson形式の文字列をシリアライズします。
        /// </summary>
        /// <typeparam name="T">シリアライズ対象の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <param name="encode">エンコード</param>
        /// <returns>変換結果</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="self"/> がnull の場合にスローされます。</exception>
        public static T JsonSerialize<T>(this string self, Encoding encode) where T : class
        {
            if (self.IsEmpty())
            {
                throw new ArgumentNullException(nameof(self));
            }
            return encode.GetBytes(self).JsonSerialize<T>();
        }

        /// <summary>
        /// UTF-8 エンコードでJson形式の文字列をシリアライズします。
        /// </summary>
        /// <typeparam name="T">シリアライズ対象の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <returns>変換結果</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="self"/> がnull の場合にスローされます。</exception>
        public static T JsonSerialize<T>(this string self) where T : class
        {
            return self.JsonSerialize<T>(Encoding.UTF8);
        }
    }
}