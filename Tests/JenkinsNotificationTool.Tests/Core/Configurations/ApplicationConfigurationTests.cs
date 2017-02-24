namespace JenkinsNotificationTool.Tests.Core.Configurations
{
    using System;
    using System.IO;
    using System.Reflection;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Configurations.Verify;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Utility;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="ApplicationConfiguration" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class ApplicationConfigurationTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public ApplicationConfigurationTests(ITestOutputHelper output) : base(output)
        {
            //
            // 予めアセンブリをロードしておく。
            //
            Products.SetAssembly(Assembly.Load("JenkinsNotificationTool"));
        }

        #endregion

        #region Methods

        #region DefaultFilePath property test

        /// <summary>
        /// <see cref="ApplicationConfiguration.DefaultFilePath" /> の初期値をテストします。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・<see cref="ApplicationConfiguration.DefaultFilePath"/> は、アプリケーション一時フォルダパスの"ApplicationConfiguration.xml"であること。
        /// </remarks>
        [Fact]
        public void Test_DefaultFilePath_DefaultValue()
        {
            // arrange
            var result = string.Empty;

            // act
            var ex = Record.Exception(() => result = ApplicationConfiguration.DefaultFilePath);

            // assert
            Assert.Null(ex);
            Assert.NotNull(result);
            Assert.NotEqual(string.Empty, result);
            Output.WriteLine($"デフォルトパス:{result}");
        }

        #endregion

        #endregion

        #region SaveCurrent method test

        /// <summary>
        /// <see cref="ApplicationConfiguration.SaveCurrent" /> をテストします。(失敗パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・<see cref="ApplicationConfiguration.Current"/> がnull の場合、<see cref="ApplicationConfiguration.SaveCurrent"/> の戻り値がfalse であること。
        /// </remarks>
        [Fact]
        public void Test_Failed_SaveCurrent_Currentがnull()
        {
            // arrange
            var result = false;
            ApplicationConfiguration.Current = null;

            // act
            var ex = Record.Exception(() => result = ApplicationConfiguration.SaveCurrent());

            // assert
            Assert.Null(ex);
            Assert.Equal(false, result);
        }

        /// <summary>
        /// <see cref="ApplicationConfiguration.SaveCurrent" /> をテストします。(失敗パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・<see cref="ApplicationConfiguration.Current"/> に不正な値が設定されている場合、<see cref="ApplicationConfiguration.SaveCurrent"/> の戻り値がfalse であること。
        /// </remarks>
        [Fact]
        public void Test_Failed_SaveCurrent_検証エラー()
        {
            // arrange
            var result = false;

            // テスト用のファイルを保存する。
            var testFileName = Path.Combine(Environment.CurrentDirectory, "SaveCurrent検証エラー.xml");
            var config = new ApplicationConfiguration();
            ConfigurationUtility.Save(config, testFileName, new ApplicationConfigurationVerify());

            // ファイルをロードする。
            ApplicationConfiguration.LoadCurrent(testFileName);

            // 検証エラーとなる値を設定する。
            var current = ApplicationConfiguration.Current;
            current.NotifyConfiguration.DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum + 1;

            // act
            var ex = Record.Exception(() => result = ApplicationConfiguration.SaveCurrent(testFileName));

            // assert
            Assert.Null(ex);
            Assert.Equal(false, result);

            File.Delete(testFileName);
        }

        /// <summary>
        /// <see cref="ApplicationConfiguration.SaveCurrent" /> をテストします。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・正常な構成情報を保存した場合、戻り値がtrue となること。
        /// </remarks>
        [Fact]
        public void Test_Success_SaveCurrent()
        {
            // arrange
            var result = false;

            // テスト用のファイルを保存する。
            var testFileName = Path.Combine(Environment.CurrentDirectory, "SaveCurrentSuccess.xml");
            var config = new ApplicationConfiguration();
            ConfigurationUtility.Save(config, testFileName, new ApplicationConfigurationVerify());

            // ファイルをロードする。
            ApplicationConfiguration.LoadCurrent(testFileName);

            // act
            // 読み込みに成功したファイルであれば保存も成功する。
            var ex = Record.Exception(() => result = ApplicationConfiguration.SaveCurrent(testFileName));

            // assert
            Assert.Null(ex);
            Assert.Equal(true, result);

            File.Delete(testFileName);
        }

        #endregion

        #region LoadCurrent method test

        /// <summary>
        /// <see cref="ApplicationConfiguration.LoadCurrent" /> をテストします。(失敗パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・存在しないファイルを読み込もうとしたとき、<see cref="ConfigurationLoadException"/> をスローすること。
        /// ・<see cref="ApplicationConfiguration.Current"/> には、初期値が設定されていること。
        /// </remarks>
        [Fact]
        public void Test_Failed_LoadCurrent_存在しないファイルを読み込む()
        {
            // arrange
            var testFileName = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());

            // act
            var ex = Record.Exception(() => ApplicationConfiguration.LoadCurrent(testFileName));

            // assert
            Assert.NotNull(ex);
            Assert.IsType<ConfigurationLoadException>(ex);
            Assert.Equal(new ApplicationConfiguration().ToString(), ApplicationConfiguration.Current.ToString());
        }

        /// <summary>
        /// <see cref="ApplicationConfiguration.LoadCurrent" /> をテストします。(失敗パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・検証エラーとなる値を持つ構成情報ファイルを読み込もうとしたとき、<see cref="ConfigurationLoadException"/> をスローすること。
        /// ・<see cref="ApplicationConfiguration.Current"/> には、初期値が設定されていること。
        /// </remarks>
        [Fact]
        public void Test_Failed_LoadCurrent_検証エラーのファイルを読み込む()
        {
            // arrange
            var testFileName = Path.Combine(Environment.CurrentDirectory, "LoadCurrent検証エラー.xml");

            // 構成情報に検証エラーとなるような値を設定する。
            var config = new ApplicationConfiguration();
            config.NotifyConfiguration.DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum + 1;
            config.Serialize(testFileName);

            // act
            var ex = Record.Exception(() => ApplicationConfiguration.LoadCurrent(testFileName));

            // assert
            Assert.NotNull(ex);
            Assert.IsType<ConfigurationLoadException>(ex);
            Assert.Equal(new ApplicationConfiguration().ToString(), ApplicationConfiguration.Current.ToString());

            File.Delete(testFileName);
        }

        /// <summary>
        /// <see cref="ApplicationConfiguration.LoadCurrent" /> をテストします。(成功パターン)
        /// </summary>
        [Fact]
        public void Test_Success_LoadCurrent()
        {
            // arrange
            var testFileName = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
            var config = new ApplicationConfiguration();
            config.NotifyConfiguration.TargetUri = DateTime.Now.ToString("O");
            config.Serialize(testFileName);

            // act
            var ex = Record.Exception(() => ApplicationConfiguration.LoadCurrent(testFileName));

            // assert
            Assert.Null(ex);
            Assert.Equal(config.ToString(), ApplicationConfiguration.Current.ToString());

            File.Delete(testFileName);
        }

        /// <summary>
        /// <see cref="ApplicationConfiguration.LoadCurrent" /> をテストします。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・引数がnull の場合、例外をスローしないこと。
        /// </remarks>
        [Fact]
        public void Test_Success_LoadCurrent_ParameterNull()
        {
            // arrange
            var testFileName = ApplicationConfiguration.DefaultFilePath;
            if (!File.Exists(testFileName))
            {
                var config = new ApplicationConfiguration();
                config.Serialize(testFileName);
            }

            // act
            var ex = Record.Exception(() => ApplicationConfiguration.LoadCurrent());

            // assert
            Assert.Null(ex);
        }

        #endregion
    }
}