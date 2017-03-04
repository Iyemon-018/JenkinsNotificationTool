namespace JenkinsNotification.Core.Communicators
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Utility;

    public class WebApiCommunicator : IWebApiCommunicator
    {
        private static readonly HttpClient Client = new HttpClient();

        private string _url;

        public void SetUrl(string url)
        {
            _url = url;
            LogManager.Info($"WebAPIの実行URLを {url} に設定した。");
        }

        public async Task<string> GetRequest()
        {
            try
            {
                //
                // リクエストを非同期で取得する。
                // タイムアウトが発生する場合は、ここでTaskCanceledException がスローされる。
                //
                LogManager.Info($"WebAPIのリクエストを取得する。(URL:{_url})");
                using (TimeTracer.StartNew($"WebAPIのリクエストを取得する。(URL:{_url})"))
                using (var request = await Client.GetAsync(_url))
                {
                    LogManager.Info("WebAPIのリクエストが取得できたので、内容を文字列にシリアル化する。");
                    return await request.Content.ReadAsStringAsync();
                }
            }
            catch (TaskCanceledException e)
            {
                // TODO メッセージをリソースに定義する。
                LogManager.Error($"WebAPIのリクエスト取得でタイムアウトが発生した。(URL:{_url})", e);
                throw new TimeoutException("WebAPI の応答がありませんでした。", e);
            }
        }

        public void SetTimeout(TimeSpan timeout)
        {
            Client.Timeout = timeout;
            LogManager.Info($"WebAPIのタイムアウトを {timeout} に設定した。");
        }
    }
}