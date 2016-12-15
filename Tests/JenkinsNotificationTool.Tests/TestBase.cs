namespace JenkinsNotificationTool.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit.Abstractions;

    /// <summary>
    /// テストで使用する機能を包含したすべてのテストクラスの基底クラスです。
    /// </summary>
    public abstract class TestBase
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

        #endregion
    }
}