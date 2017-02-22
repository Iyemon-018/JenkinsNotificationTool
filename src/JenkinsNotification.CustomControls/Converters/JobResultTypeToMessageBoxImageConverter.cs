namespace JenkinsNotification.CustomControls.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Core;
    using Core.Extensions;

    /// <summary>
    /// <see cref="JobResultType"/> から<see cref="MessageBoxImage"/> へのコンバーター クラスです。
    /// </summary>
    /// <seealso cref="IValueConverter" />
    [ValueConversion(typeof(JobResultType), typeof(MessageBoxImage))]
    public class JobResultTypeToMessageBoxImageConverter : IValueConverter
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
            if (value == null) return DependencyProperty.UnsetValue;
            if (!value.ToString().IsDefined<JobResultType>()) return DependencyProperty.UnsetValue;

            var resultType = value.ToString().ToEnum<JobResultType>();
            switch (resultType)
            {
                case JobResultType.None:
                    return MessageBoxImage.Information;
                case JobResultType.Success:
                    return MessageBoxImage.Information;
                case JobResultType.Warning:
                    return MessageBoxImage.Warning;
                case JobResultType.Failed:
                    return MessageBoxImage.Error;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ターゲットによって生成される値。</param>
        /// <param name="targetType">変換後の型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        /// <exception cref="System.InvalidOperationException">この機能は使用できません。</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        #endregion
    }
}