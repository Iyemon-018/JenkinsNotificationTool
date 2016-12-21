namespace JenkinsNotificationTool.Tests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls.Primitives;
    using AutoMapper;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.ViewModels.Configurations;
    using Xunit;
    using Xunit.Abstractions;
    using Profile = JenkinsNotification.Core.Profile;

    /// <summary>
    /// <see cref="JenkinsNotification.Core.Profile" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class ProfileTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public ProfileTests(ITestOutputHelper output) : base(output)
        {
            Mapper.Initialize(x => x.AddProfile<Profile>());
            Mapper.AssertConfigurationIsValid();
        }

        #endregion

        #region Methods

        #region Mapping test

        /// <summary>
        /// オブジェクトのマッピングをテストします。
        /// </summary>
        /// <typeparam name="TSource">マッピング元のオブジェクトの型</typeparam>
        /// <typeparam name="TDestination">マッピング先のオブジェクトの型</typeparam>
        /// <param name="caseName">テスト ケースの内容</param>
        /// <param name="source">マッピング元のオブジェクト</param>
        /// <param name="sourceProperty"><paramref name="source"/> のテスト対象プロパティ名</param>
        /// <param name="destination">マッピング先のオブジェクト（期待値）</param>
        /// <param name="destinationProperty"><paramref name="sourceProperty"/> にマッピングされる<typeparamref name="TDestination"/> のプロパティ名</param>
        [Theory]
        [MemberData(nameof(Map_NotifyConfiguration_TestData))]
        [MemberData(nameof(Map_NotifyConfigurationViewModel_TestData))]
        public void Test_Map_Property<TSource, TDestination>(string caseName,
                                                             TSource source,
                                                             string sourceProperty,
                                                             TDestination destination,
                                                             string destinationProperty)
        {
            // arrange
            //
            // マッピングする。
            // TSource → TDestination
            //
            var mappedValue = source.Map<TDestination>();

            // act
            //
            // マップ結果のオブジェクトからプロパティの値を取得する。
            // 期待値(destination.destinationProperty) の値も取得する。
            //
            var targetValue = typeof(TDestination).GetProperty(destinationProperty).GetValue(mappedValue);
            var expectedValue = typeof(TDestination).GetProperty(destinationProperty).GetValue(destination);

            // assert
            //
            // マップ結果を比較して検証する。結果も表示する。
            //
            Assert.Equal(expectedValue, targetValue);
            WriteResult($"{caseName}" +
                        $"{Environment.NewLine}" +
                        $"({typeof(TSource)}.{sourceProperty} To {typeof(TDestination)}.{destinationProperty})"
                , targetValue
                , expectedValue);
        }

        #endregion

        #region Valid test

        /// <summary>
        /// <see cref="JenkinsNotification.Core.Profile"/> がマッピング可能な定義になっているかどうかを判定します。
        /// </summary>
        [Fact]
        public void Test_MapProfile_Validate()
        {
            Mapper.Initialize(x => x.AddProfile<Profile>());
            var ex = Record.Exception(() => Mapper.AssertConfigurationIsValid());
            Assert.Null(ex);
        }

        #endregion

        #endregion

        #region Test Data

        /// <summary>
        /// <see cref="NotifyConfiguration"/> から<see cref="NotifyConfigurationViewModel"/> へのマッピングテストデータです。
        /// </summary>
        /// <returns>テスト データ</returns>
        public static IEnumerable<object[]> Map_NotifyConfiguration_TestData()
        {
            //
            // Source でIgnore に指定しているプロパティを以下に示す。
            // これら以外の値はマッピングが可能である。
            //
            // ・NotifyConfiguration.PopupTimeoutValue
            //
            var src = new NotifyConfiguration();
            var dst = new NotifyConfigurationViewModel();

            src.TargetUri = DateTime.Now.ToString("HH:mm:ss.fff");
            dst.TargetUri = src.TargetUri;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.TargetUri)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.TargetUri),
                    dst,
                    nameof(dst.TargetUri)
                };

            src.PopupAnimationType = PopupAnimation.Slide;
            dst.PopupAnimationType = PopupAnimation.Slide;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.PopupAnimationType)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.PopupAnimationType),
                    dst,
                    nameof(dst.PopupAnimationType)
                };

            src.PopupTimeout = DateTime.Now.TimeOfDay;
            dst.PopupTimeout = src.PopupTimeout;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.PopupTimeout)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.PopupTimeout),
                    dst,
                    nameof(dst.PopupTimeout)
                };

            src.DisplayHistoryCount = DateTime.Now.Millisecond;
            dst.DisplayHistoryCount = src.DisplayHistoryCount;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.DisplayHistoryCount)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.DisplayHistoryCount),
                    dst,
                    nameof(dst.DisplayHistoryCount)
                };

            src.IsNotifySuccess = true;
            dst.IsNotifySuccess = src.IsNotifySuccess;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.IsNotifySuccess)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.IsNotifySuccess),
                    dst,
                    nameof(dst.IsNotifySuccess)
                };
        }

        /// <summary>
        /// <see cref="NotifyConfigurationViewModel"/> から<see cref="NotifyConfiguration"/> へのマッピングテストデータです。
        /// </summary>
        /// <returns>テスト データ</returns>
        public static IEnumerable<object[]> Map_NotifyConfigurationViewModel_TestData()
        {
            //
            // Source でIgnore に指定しているプロパティを以下に示す。
            // これら以外の値はマッピングが可能である。
            //
            // ・NotifyConfiguration.PopupTimeoutValue
            //
            var src = new NotifyConfigurationViewModel();
            var dst = new NotifyConfiguration();

            src.TargetUri = DateTime.Now.ToString("HH:mm:ss.fff");
            dst.TargetUri = src.TargetUri;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.TargetUri)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.TargetUri),
                    dst,
                    nameof(dst.TargetUri)
                };

            src.PopupAnimationType = PopupAnimation.Scroll;
            dst.PopupAnimationType = src.PopupAnimationType;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.PopupAnimationType)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.PopupAnimationType),
                    dst,
                    nameof(dst.PopupAnimationType)
                };

            src.PopupTimeout = DateTime.Now.TimeOfDay;
            dst.PopupTimeout = src.PopupTimeout;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.PopupTimeout)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.PopupTimeout),
                    dst,
                    nameof(dst.PopupTimeout)
                };

            src.DisplayHistoryCount = DateTime.Now.Millisecond;
            dst.DisplayHistoryCount = src.DisplayHistoryCount;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.DisplayHistoryCount)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.DisplayHistoryCount),
                    dst,
                    nameof(dst.DisplayHistoryCount)
                };

            src.IsNotifySuccess = true;
            dst.IsNotifySuccess = src.IsNotifySuccess;
            yield return
                new object[]
                {
                    $"(正常系) {nameof(src.IsNotifySuccess)}をマッピングした場合、プロパティの値がマップできること。",
                    src,
                    nameof(src.IsNotifySuccess),
                    dst,
                    nameof(dst.IsNotifySuccess)
                };
        }

        #endregion
    }
}