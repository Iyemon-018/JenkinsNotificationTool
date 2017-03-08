namespace JenkinsNotification.Core.ViewModels.WebApi
{
    using System.Windows.Media;
    using JenkinsNotification.Core.ComponentModels;

    /// <summary>
    /// WebAPI で取得したJenkins のジョブ データ クラスです。
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    public class JobViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// ジョブの種別名称を取得します。<para/>
        /// [_class] のデータを保持します。
        /// </summary>
        public string TypeName { get; internal set; }

        /// <summary>
        /// ジョブ名称を取得します。<para/>
        /// [name] のデータを保持します。
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// ジョブのページURLを取得します。<para/>
        /// [url] のデータを保持します。
        /// </summary>
        public string Url { get; internal set; }

        /// <summary>
        /// 状態の色を取得します。<para/>
        /// [color] のデータを保持します。
        /// </summary>
        public Color Color { get; internal set; }

        #endregion
    }
}