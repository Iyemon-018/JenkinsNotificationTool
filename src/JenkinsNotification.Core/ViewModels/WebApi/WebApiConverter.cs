namespace JenkinsNotification.Core.ViewModels.WebApi
{
    using System.Windows.Media;

    /// <summary>
    /// Jenkins WebAPI のパラメータ変換機能クラスです。
    /// </summary>
    public static class WebApiConverter
    {
        /// <summary>
        /// 色の名称をカラーコードに変換します。
        /// </summary>
        /// <param name="jobColor">Color of the job.</param>
        /// <returns>Color.</returns>
        public static Color ToJobColor(string jobColor)
        {
            jobColor = jobColor.ToUpper();

            // TODO どこかのリソースから取得するように変える。Converterにしたほうがいいかも？
            if (jobColor.Equals("RED")) return Colors.Red;
            if (jobColor.Equals("BLUE")) return Colors.Blue;
            if (jobColor.Equals("GREEN")) return Colors.Green;
            if (jobColor.Equals("YELLOW")) return Colors.Yellow;
            return Colors.Gray;
        }
    }
}