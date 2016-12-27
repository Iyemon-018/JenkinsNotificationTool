namespace JenkinsNotification.Core.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Json 形式のデータをデシリアライズするための拡張メソッドを定義します。
    /// </summary>
    public static class JsonDeserializer
    {
        /// <summary>
        /// インスタンスをJson形式のバッファーへデシリアライズします。
        /// </summary>
        /// <typeparam name="T">デシリアライズする型</typeparam>
        /// <param name="self">自分自身</param>
        /// <returns>デシリアライズ結果</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="self"/> がnull の場合にスローされます。</exception>
        public static byte[] JsonDeserialize<T>(this T self)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            byte[] result;

            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(ms, self);
                result = ms.GetBuffer().TakeWhile(x => x != 0).ToArray();
            }

            return result;
        }

        /// <summary>
        /// インスタンスをJson形式のバッファーへデシリアライズします。
        /// </summary>
        /// <typeparam name="T">デシリアライズする型</typeparam>
        /// <param name="self">自分自身</param>
        /// <param name="encode">
        /// エンコード<para/>
        /// null を設定する場合、<see cref="Encoding.UTF8"/> でデシリアライズします。
        /// </param>
        /// <returns>デシリアライズ結果</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="self"/> がnull の場合にスローされます。</exception>
        public static string JsonDeserialize<T>(this T self, Encoding encode)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }
            var buffer = self.JsonDeserialize();
            return encode.GetString(buffer);
        }
    }
}