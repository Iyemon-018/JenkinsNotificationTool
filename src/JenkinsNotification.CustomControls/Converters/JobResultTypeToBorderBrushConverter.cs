namespace JenkinsNotification.CustomControls.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Extensions;

    /// <summary>
    /// <see cref="JobResultType"/> から枠線色<see cref="Brush"/> へのコンバーター クラスです。
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    [ValueConversion(typeof(JobResultType), typeof(Brush))]
    public class JobResultTypeToBorderBrushConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        /// <see cref="JobResultType"/> に対する枠線色のマップ情報
        /// </summary>
        private readonly Dictionary<JobResultType, Brush> _brushMap;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JobResultTypeToBorderBrushConverter()
        {
            var app = Application.Current;
            var successBrush = app.TryFindResource("BalloonTip.JobSuccess.BorderBrush") as Brush;
            var warningBrush = app.TryFindResource("BalloonTip.JobWarning.BorderBrush") as Brush;
            var failedBrush  = app.TryFindResource("BalloonTip.JobFailed.BorderBrush") as Brush;
            _brushMap        = new Dictionary<JobResultType, Brush>
                            {
                                {JobResultType.None, successBrush},
                                {JobResultType.Success, successBrush},
                                {JobResultType.Warning, warningBrush},
                                {JobResultType.Failure, failedBrush}
                            };
        }

        #endregion

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
            return _brushMap[resultType];
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