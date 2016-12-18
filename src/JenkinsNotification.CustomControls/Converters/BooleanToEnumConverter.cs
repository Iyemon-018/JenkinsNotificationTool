namespace JenkinsNotification.CustomControls.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using JenkinsNotification.Core.Extensions;

    /// <summary>
    /// <see cref="bool"/> 型の値から<see cref="Enum"/> へのコンバータークラスです。
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    public class BooleanToEnumConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || !value.GetType().IsEnum)
            {
                return DependencyProperty.UnsetValue;
            }

            var actualParameter = parameter;
            if (!parameter.GetType().IsEnum && parameter is string)
            {
                actualParameter = parameter.ToString().ToEnum(value.GetType());
            }

            var actualValue = value.ToString().ToEnum(value.GetType());
            return Equals(actualValue, actualParameter);
        }

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ターゲットによって生成される値。</param>
        /// <param name="targetType">変換後の型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return DependencyProperty.UnsetValue;
            }

            bool actualValue;
            if (!bool.TryParse(value.ToString(), out actualValue))
            {
                return DependencyProperty.UnsetValue;
            }

            return !actualValue ? DependencyProperty.UnsetValue : parameter.ToString().ToEnum(targetType);
        }

        #endregion
    }
}