namespace JenkinsNotification.Core.Communicators.WebApi
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Utility;

    public class WebApiCommunicator : IWebApiCommunicator
    {
        #region Const

        private static readonly HttpClient Client = new HttpClient();

        #endregion

        #region Methods

        /// <summary>
        /// リクエストを非同期で取得します。
        /// </summary>
        /// <param name="url">リクエストを実行するURL</param>
        /// <param name="timeoutAction">タイムアウト発生時のアクション デリゲート</param>
        /// <param name="exceptionAction">例外発生時のアクション デリゲート</param>
        /// <returns>リクエストデータ</returns>
        /// <exception cref="System.InvalidOperationException">Basic認証が設定されていません。</exception>
        /// <exception cref="System.TimeoutException">WebAPI の応答がありませんでした。</exception>
        public async Task<string> GetRequest(string url, Action timeoutAction, Action<Exception> exceptionAction)
        {
            if (Client.DefaultRequestHeaders.Authorization == null)
            {
                // TODO リソースに定義する。
                throw new InvalidOperationException("Basic認証が設定されていません。");
            }

            try
            {
                //
                // リクエストを非同期で取得する。
                // タイムアウトが発生する場合は、ここでTaskCanceledException がスローされる。
                //
                LogManager.Info($"WebAPIのリクエストを取得する。(URL:{url})");
                using (TimeTracer.StartNew($"WebAPIのリクエストを取得する。(URL:{url})"))
                using (var request = await Client.GetAsync(url))
                {
                    LogManager.Info("WebAPIのリクエストが取得できたので、内容を文字列にシリアル化する。");
                    return await request.Content.ReadAsStringAsync();
                }
            }
            catch (TaskCanceledException e)
            {
                // TODO メッセージをリソースに定義する。
                LogManager.Error($"WebAPIのリクエスト取得でタイムアウトが発生した。(URL:{url})", e);
                throw new TimeoutException("WebAPI の応答がありませんでした。", e);
            }
            catch (Exception e)
            {
                // TODO メッセージをリソースに定義する。
                LogManager.Error($"WebAPIのリクエスト取得で例外が発生した。(URL:{url})", e);
                throw;
            }
        }

        /// <summary>
        /// API実行のための資格情報を設定します。
        /// </summary>
        /// <param name="userName">ユーザー名</param>
        /// <param name="password">パスワード</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="userName"/>、もしくは<paramref name="password"/> がnull の場合にスローされます。</exception>
        public void SetCredential(string userName, string password)
        {
            if (userName.IsEmpty()) throw new ArgumentNullException(nameof(userName));
            if (password.IsEmpty()) throw new ArgumentNullException(nameof(password));

            var asciiData         = Encoding.ASCII.GetBytes($"{userName}:{password}");
            var authorizationCode = Convert.ToBase64String(asciiData);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationCode);
            LogManager.Info($"{userName}, {password} で資格情報を登録した。");
        }

        /// <summary>
        /// APIの実行タイムアウトを設定します。
        /// </summary>
        /// <param name="timeout">タイムアウト</param>
        public void SetTimeout(TimeSpan timeout)
        {
            Client.Timeout = timeout;
            LogManager.Info($"WebAPIのタイムアウトを {timeout} に設定した。");
        }

        #endregion
    }
}