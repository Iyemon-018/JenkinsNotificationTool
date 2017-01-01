namespace JenkinsNotificationTool.Tests
{
    using System;
    using Xunit.Abstractions;

    /// <summary>
    /// テストで使用する機能を包含したすべてのテストクラスの基底クラスです。
    /// </summary>
    /// <remarks>
    /// 単体テストを実行するための汎用的な機能を実装したテスト機能クラスです。<para/><para/>
    /// 
    /// テスト前に必ず実行したい処理は、コンストラクタに実装してください。<para/>
    /// テスト終了後に必ず実行したい処理は、<see cref="OnDisposingUnmanagedResources"/> メソッドをオーバーライドして実装してください。<para/><para/>
    /// 
    /// <see cref="Output"/> プロパティからコンソール出力機能を呼び出すことができます。（参照：<see cref="ITestOutputHelper"/>)
    /// </remarks>
    public abstract class TestBase : IDisposable
    {
        #region Fields

        /// <summary>
        /// テスト出力ヘルパー
        /// </summary>
        private readonly ITestOutputHelper _output;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        protected TestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        #endregion

        #region Properties

        /// <summary>
        /// テスト出力ヘルパーを取得します。
        /// </summary>
        protected ITestOutputHelper Output => _output;

        #endregion

        #region Methods

        /// <summary>
        /// テスト結果を出力します。
        /// </summary>
        /// <param name="caseName">テストケース</param>
        /// <param name="result">実行結果</param>
        /// <param name="expected">期待値</param>
        protected void WriteResult(string caseName, string result, string expected)
        {
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{result}, 期待値:{expected}");
        }

        /// <summary>
        /// テスト結果を出力します。
        /// </summary>
        /// <param name="caseName">テストケース</param>
        /// <param name="result">実行結果</param>
        /// <param name="expected">期待値</param>
        protected void WriteResult(string caseName, object result, object expected)
        {
            Output.WriteLine($"{caseName}{Environment.NewLine}結果:{result}, 期待値:{expected}");
        }

        #region IDisposable Support

        private bool disposedValue = false; // 重複する呼び出しを検出するには

        /// <summary>
        /// このオブジェクトを解放します。
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnDisposingManagedResources();
                }

                OnDisposingUnmanagedResources();    

                disposedValue = true;
            }
        }

        /// <summary>
        /// アンマネージドリソースを解放します。
        /// </summary>
        protected virtual void OnDisposingUnmanagedResources()
        {
            
        }

        /// <summary>
        /// マネージドリソースを解放します。
        /// </summary>
        protected virtual void OnDisposingManagedResources()
        {
            
        }

        /// <summary>
        /// ファイナライズ
        /// </summary>
        ~TestBase()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        /// <summary>
        /// アンマネージ リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }
}