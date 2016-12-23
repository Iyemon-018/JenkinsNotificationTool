namespace JenkinsNotification.Core.ViewModels.Api.Converter
{
    /// <summary>
    /// APIデータの変換機能クラスです。
    /// </summary>
    public static class ApiConverter
    {
        #region Methods

        /// <summary>
        /// <see cref="JobResultType"/> をJson で使用できる文字列へ変換します。
        /// </summary>
        /// <param name="result">ジョブ結果種別</param>
        /// <returns>変換結果</returns>
        public static string JobResultTypeToString(JobResultType result)
        {
            switch (result)
            {
                case JobResultType.Success:
                    return "SUCCESS";
                case JobResultType.Warning:
                    return "Warning";
                case JobResultType.Failed:
                    return "Failed";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// <see cref="JobStatus"/> をJson で使用できる文字列へ変換します。
        /// </summary>
        /// <param name="status">ジョブ状態</param>
        /// <returns>変換結果</returns>
        public static string JobStatusToString(JobStatus status)
        {
            switch (status)
            {
                case JobStatus.Start:
                    return "START";
                case JobStatus.Success:
                    return "SUCCESS";
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}