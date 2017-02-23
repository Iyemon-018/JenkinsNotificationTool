namespace JenkinsNotificationTool.Tests.Core.Configurations.Verify
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls.Primitives;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Configurations.Verify;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="ApplicationConfigurationVerify" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class ApplicationConfigurationVerifyTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public ApplicationConfigurationVerifyTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Verify method test

        /// <summary>
        /// <see cref="Test_Theory_Verify"/> で使用するテストデータを取得します。
        /// </summary>
        public static IEnumerable<object[]> VerifyTestData
            => new List<object[]>
                   {
                       new object[]
                           {
                               $"(異常系) 構成情報がnull の場合、{typeof(ArgumentNullException)} をスローすること。",
                               null,
                               typeof(ArgumentNullException)
                           },
                       new object[]
                           {
                               $"(正常系) 構成情報が設定されている場合、正常に検証が終了すること。",
                               new ApplicationConfiguration
                                   {
                                       DisplayConfiguration = new DisplayConfiguration(),
                                       NotifyConfiguration = new NotifyConfiguration
                                                                 {
                                                                     TargetUri = "ws://127.0.0.1:12345/",
                                                                     PopupAnimationType = PopupAnimation.Slide,
                                                                     PopupTimeout = DateTime.Now.TimeOfDay,
                                                                     PopupTimeoutValue =
                                                                             DateTime.Now.TimeOfDay.ToString(),
                                                                     DisplayHistoryCount =
                                                                             NotifyConfigurationVerify
                                                                                     .DisplayHistoryMaximum,
                                                                     IsNotifySuccess = false
                                                                 }
                                   },
                               null
                           }
                   };

        /// <summary>
        /// <see cref="ApplicationConfigurationVerify.Verify" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="configuration">アプリケーション構成情報</param>
        /// <param name="exceptionType">スローされる例外</param>
        [Theory]
        [MemberData(nameof(VerifyTestData))]
        public void Test_Theory_Verify(string caseName, ApplicationConfiguration configuration, Type exceptionType)
        {
            // arrange
            var verify = new ApplicationConfigurationVerify();

            // act
            var ex = Record.Exception(() => verify.Verify(configuration));

            // assert
            if (exceptionType == null)
            {
                Assert.Null(ex);
            }
            else
            {
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(exceptionType)}:{exceptionType}");
        }

        #endregion
    }
}