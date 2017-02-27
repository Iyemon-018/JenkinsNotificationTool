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
    /// <see cref="NotifyConfigurationVerify" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="TestBase" />
    public class NotifyConfigurationVerifyTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public NotifyConfigurationVerifyTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// <see cref="Test_Theory_Verify"/> で使用するテストデータを取得します。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> GetVerifyTestData()
        {
            var targetUri = "ws://127.0.0.1:12345/";
            var popupTime = DateTime.Now.TimeOfDay;
            var popupTimeValue = popupTime.ToString();

            yield return new object[]
                         {
                             $"(異常系) 検証対象の構成情報がnull の場合、{typeof(ArgumentNullException)} をスローすること。",
                             null,
                             typeof(ArgumentNullException)
                         };
            yield return new object[]
                         {
                             $"(異常系) DisplayHistoryCount が{NotifyConfigurationVerify.DisplayHistoryMinimum - 1} の場合、{typeof(ConfigurationVerifyException)} をスローすること。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMinimum - 1,
                                 IsNotifySuccess = false
                             },
                             typeof(ConfigurationVerifyException)
                         };

            yield return new object[]
                         {
                             $"(異常系) DisplayHistoryCount が{int.MinValue} の場合、{typeof(ConfigurationVerifyException)} をスローすること。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = int.MinValue,
                                 IsNotifySuccess = false
                             },
                             typeof(ConfigurationVerifyException)
                         };

            yield return new object[]
                         {
                             $"(正常系) DisplayHistoryCount が{NotifyConfigurationVerify.DisplayHistoryMinimum} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMinimum,
                                 IsNotifySuccess = false
                             },
                             null
                         };

            yield return new object[]
                         {
                             $"(異常系) DisplayHistoryCount が{NotifyConfigurationVerify.DisplayHistoryMaximum + 1} の場合、{typeof(ConfigurationVerifyException)} をスローすること",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum + 1,
                                 IsNotifySuccess = false
                             },
                             typeof(ConfigurationVerifyException)
                         };
            yield return new object[]
                         {
                             $"(異常系) DisplayHistoryCount が{int.MaxValue} の場合、{typeof(ConfigurationVerifyException)} をスローすること",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = int.MaxValue,
                                 IsNotifySuccess = false
                             },
                             typeof(ConfigurationVerifyException)
                         };
            yield return new object[]
                         {
                             $"(異常系) DisplayHistoryCount が{NotifyConfigurationVerify.DisplayHistoryMaximum + 1} の場合、{typeof(ConfigurationVerifyException)} をスローすること。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum + 1,
                                 IsNotifySuccess = false
                             },
                             typeof(ConfigurationVerifyException)
                         };
            yield return new object[]
                         {
                             $"(正常系) TargetUri がString.Empty の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = string.Empty,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) TargetUri がnull の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = null,
                                 PopupAnimationType = PopupAnimation.None,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupAnimationType {PopupAnimation.Fade} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Fade,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupAnimationType {PopupAnimation.Scroll} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Scroll,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupAnimationType {PopupAnimation.Slide} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = popupTime,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupTimeout がnull の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = null,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupTimeout が{TimeSpan.Zero} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = TimeSpan.Zero,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupTimeout が{TimeSpan.MinValue} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = TimeSpan.MinValue,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupTimeout が{TimeSpan.MaxValue} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = TimeSpan.MaxValue,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupTimeoutValue がString.Empty の場合、正常に終了すること。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = TimeSpan.MaxValue,
                                 PopupTimeoutValue = string.Empty,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null,
                         };
            yield return new object[]
                         {
                             $"(正常系) PopupTimeoutValue がnull の場合、正常に終了すること。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = TimeSpan.MaxValue,
                                 PopupTimeoutValue = null,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             null,
                         };
            yield return new object[]
                         {
                             $"(異常系) PopupTimeoutValue がTimeSpan 書式でない場合、{typeof(ConfigurationVerifyException)} をスローすること。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = TimeSpan.MaxValue,
                                 PopupTimeoutValue = "a",
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = false
                             },
                             typeof(ConfigurationVerifyException)
                         };
            yield return new object[]
                         {
                             $"(正常系) IsNotifySuccess が{true} の場合、例外をスローしないこと。",
                             new NotifyConfiguration
                             {
                                 TargetUri = targetUri,
                                 PopupAnimationType = PopupAnimation.Slide,
                                 PopupTimeout = TimeSpan.MaxValue,
                                 PopupTimeoutValue = popupTimeValue,
                                 DisplayHistoryCount = NotifyConfigurationVerify.DisplayHistoryMaximum,
                                 IsNotifySuccess = true
                             },
                             null
                         };
        }

        /// <summary>
        /// <see cref="NotifyConfigurationVerify.Verify" /> をテストします。
        /// </summary>
        /// <param name="caseName">テストケース内容</param>
        /// <param name="config">構成情報</param>
        /// <param name="exceptionType">スローされる例外の型</param>
        [Theory]
        [MemberData(nameof(GetVerifyTestData))]
        public void Test_Theory_Verify(string caseName, NotifyConfiguration config, Type exceptionType)
        {
            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $" {nameof(config)}:{config}{Environment.NewLine}" +
                             $" {nameof(exceptionType)}:{exceptionType}");

            // arrange
            var verify = new NotifyConfigurationVerify();

            // act
            var ex = Record.Exception(() => verify.Verify(config));

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
        }

        #endregion
    }
}