namespace JenkinsNotification.Core.Extensions
{
    using System.IO;
    using Properties;

    /// <summary>
    /// Xml デシリアライズ拡張メソッドクラスです。
    /// </summary>
    public static class XmlDeserializer
    {
        #region Methods

        /// <summary>
        /// 指定したファイルからデシリアライズします。
        /// </summary>
        /// <typeparam name="T">自分自身の型</typeparam>
        /// <param name="filePath">自分自身</param>
        /// <returns>デシリアライズオブジェクト</returns>
        /// <exception cref="System.IO.FileNotFoundException">指定したファイルが見つかりません。</exception>
        public static T Deserialize<T>(this string filePath)
            where T : class, new()
        {
            if (filePath.IsEmpty() || !File.Exists(filePath))
            {
                throw new FileNotFoundException(Resources.FileNotFoundMessage, filePath);
            }

            T result;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                result = (T)serializer.Deserialize(fs);
            }

            return result;
        }

        #endregion
    }
}