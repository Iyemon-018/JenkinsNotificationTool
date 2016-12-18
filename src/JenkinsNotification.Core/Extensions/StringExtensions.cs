namespace JenkinsNotification.Core.Extensions
{
    using System;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// <see cref="string"/> 型の拡張メソッドを定義します。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 文字列が空文字かどうかを判定します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <returns>空文字、もしくはnull の場合、true を返します。それ以外の場合、false を返します。</returns>
        public static bool IsEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        /// <summary>
        /// 文字列が設定されているかどうかを判定します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <returns>文字列が設定されている場合、true を返します。それ以外の場合、false を返します。</returns>
        public static bool HasText(this string self)
        {
            return !self.IsEmpty();
        }

        /// <summary>
        /// 値が<typeparamref name="TEnum"/> 型の列挙値として定義されているかどうかを判定します。<para/>
        /// 値が空文字、あるいはnull の場合は、false を返します。
        /// </summary>
        /// <typeparam name="TEnum">判定対象の列挙体の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <returns>判定結果(true:定義済み, false:未定義)</returns>
        public static bool IsDefined<TEnum>(this string self) where TEnum : struct
        {
            return !self.IsEmpty() && Enum.IsDefined(typeof(TEnum), self);
        }

        /// <summary>
        /// 値を<typeparamref name="TEnum"/> 型に変換します。<para/>
        /// 値が未定義だったり変換に失敗（値が空文字、null の場合など）した場合は、<paramref name="defaultValue"/> を返します。
        /// </summary>
        /// <typeparam name="TEnum">変換する列挙体の型</typeparam>
        /// <param name="self">自分自身</param>
        /// <param name="defaultValue">変換に失敗した場合の戻り値</param>
        /// <returns>変換結果の値</returns>
        public static TEnum ToEnum<TEnum>(this string self, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            TEnum result;
            return self.IsDefined<TEnum>()
                ? Enum.TryParse(self, out result) ? result : defaultValue
                : defaultValue;
        }

        /// <summary>
        /// 値を<see cref="enumType"/> 型の列挙体に変換します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="enumType">変換後の列挙体の型</param>
        /// <returns>変換結果</returns>
        public static object ToEnum(this string self, Type enumType)
        {
            return Enum.Parse(enumType, self);
        }
        
        /// <summary>
        /// 文字列を整数値に変換します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="defaultValue">変換失敗時の戻り値</param>
        /// <returns>変換結果</returns>
        public static int ToInt(this string self, int defaultValue = default(int))
        {
            int result;
            return int.TryParse(self, out result) ? result : defaultValue;
        }
    }
}