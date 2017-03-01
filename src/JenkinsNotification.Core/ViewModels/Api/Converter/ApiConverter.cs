namespace JenkinsNotification.Core.ViewModels.Api.Converter
{
    using System.Collections.Generic;
    using JenkinsNotification.Core.Extensions;

    /// <summary>
    /// APIデータの変換機能クラスです。
    /// </summary>
    public static class ApiConverter
    {
        #region Const

        /// <summary>
        /// <see cref="JobResultType.Failure"/> に該当する結果種別文字列です。
        /// </summary>
        public static readonly string JobResultFailure = "Failure";

        /// <summary>
        /// <see cref="JobResultType.Success"/> に該当する結果種別文字列です。
        /// </summary>
        public static readonly string JobResultSuccess = "SUCCESS";

        /// <summary>
        /// <see cref="JobResultType.Warning"/> に該当する結果種別文字列です。
        /// </summary>
        public static readonly string JobResultWarning = "Warning";

        /// <summary>
        /// <see cref="JobStatus.Failure"/> に該当する状態種別文字列です。
        /// </summary>
        public static readonly string JobStatusFailure = "FAILURE";

        /// <summary>
        /// <see cref="JobStatus.Start"/> に該当する状態種別文字列です。
        /// </summary>
        public static readonly string JobStatusStart = "START";

        /// <summary>
        /// <see cref="JobStatus.Success"/> に該当する状態種別文字列です。
        /// </summary>
        public static readonly string JobStatusSuccess = "SUCCESS";

        /// <summary>
        /// <see cref="JobResultType"/> に対する種別文字列のマッピング情報
        /// </summary>
        private static readonly IReadOnlyDictionary<JobResultType, string> JobResultValueMap
                = new Dictionary<JobResultType, string>
                      {
                          {JobResultType.Success, JobResultSuccess},
                          {JobResultType.Warning, JobResultWarning},
                          {JobResultType.Failure, JobResultFailure}
                      };

        /// <summary>
        /// <see cref="JobStatus"/> に対する状態文字列のマッピング情報
        /// </summary>
        private static readonly IReadOnlyDictionary<JobStatus, string> JobStatusValueMap
                = new Dictionary<JobStatus, string>
                      {
                          {JobStatus.Success, JobStatusSuccess},
                          {JobStatus.Start, JobStatusStart},
                          {JobStatus.Failure, JobStatusFailure}
                      };

        #endregion

        #region Methods

        /// <summary>
        /// 文字列を<see cref="JobResultType"/> へ変換します。
        /// </summary>
        /// <param name="result">変換対象の文字列</param>
        /// <returns>変換結果</returns>
        public static JobResultType JobResultTypeStringToEnum(string result)
        {
            if (result.IsEmpty()) return JobResultType.None;

            var value = result.ToUpper();
            if (value.Equals(JobResultSuccess.ToUpper())) return JobResultType.Success;
            if (value.Equals(JobResultWarning.ToUpper())) return JobResultType.Warning;
            if (value.Equals(JobResultFailure.ToUpper())) return JobResultType.Failure;

            return JobResultType.None;
        }

        /// <summary>
        /// <see cref="JobResultType"/> をJson で使用できる文字列へ変換します。
        /// </summary>
        /// <param name="result">ジョブ結果種別</param>
        /// <returns>変換結果</returns>
        public static string JobResultTypeToString(JobResultType result)
        {
            return JobResultValueMap.ContainsKey(result) ? JobResultValueMap[result] : string.Empty;
        }

        /// <summary>
        /// 文字列を<see cref="JobStatus"/> に変換します。
        /// </summary>
        /// <param name="status">変換対象の文字列</param>
        /// <returns>変換結果</returns>
        public static JobStatus JobStatusStringToEnum(string status)
        {
            if (status.IsEmpty()) return JobStatus.None;

            var value = status.ToUpper();
            if (value.Equals(JobStatusSuccess.ToUpper())) return JobStatus.Success;
            if (value.Equals(JobStatusStart.ToUpper())) return JobStatus.Start;
            if (value.Equals(JobStatusFailure.ToUpper())) return JobStatus.Failure;

            return JobStatus.None;
        }

        /// <summary>
        /// <see cref="JobStatus"/> をJson で使用できる文字列へ変換します。
        /// </summary>
        /// <param name="status">ジョブ状態</param>
        /// <returns>変換結果</returns>
        public static string JobStatusToString(JobStatus status)
        {
            return JobStatusValueMap.ContainsKey(status) ? JobStatusValueMap[status] : string.Empty;
        }

        #endregion
    }
}