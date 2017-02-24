namespace JenkinsNotification.Core.ViewModels.Api.Converter
{
    using JenkinsNotification.Core.Extensions;

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
                case JobResultType.Failure:
                    return "Failure";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 文字列を<see cref="JobResultType"/> へ変換します。
        /// </summary>
        /// <param name="result">変換対象の文字列</param>
        /// <returns>変換結果</returns>
        public static JobResultType JobResultTypeStringToEnum(string result)
        {
            if (result.IsEmpty()) return JobResultType.None;

            switch (result.ToUpper())
            {
                case "SUCCESS":
                    return JobResultType.Success;
                case "WARNING":
                    return JobResultType.Warning;
                case "FAILURE":
                    return JobResultType.Failure;
                default:
                    return JobResultType.None;
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

        /// <summary>
        /// 文字列を<see cref="JobStatus"/> に変換します。
        /// </summary>
        /// <param name="status">変換対象の文字列</param>
        /// <returns>変換結果</returns>
        public static JobStatus JobStatusStringToEnum(string status)
        {
            if (status.IsEmpty()) return JobStatus.None;

            switch (status.ToUpper())
            {
                case "START":
                    return JobStatus.Start;
                case "SUCCESS":
                    return JobStatus.Success;
                case "FAILURE":
                    return JobStatus.Failure;
                default:
                    return JobStatus.None;
            }
        }

        #endregion
    }
}