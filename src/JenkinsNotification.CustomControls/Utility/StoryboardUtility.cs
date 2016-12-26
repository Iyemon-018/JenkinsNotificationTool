namespace JenkinsNotification.CustomControls.Utility
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Storyboard に関するユーティリティ機能クラスです。
    /// </summary>
    public static class StoryboardUtility
    {
        /// <summary>
        /// <see cref="EasingDoubleKeyFrame"/> を生成します。
        /// </summary>
        /// <param name="value">初期値</param>
        /// <param name="keyTime">キー時刻</param>
        /// <returns><see cref="EasingDoubleKeyFrame"/> オブジェクト</returns>
        public static EasingDoubleKeyFrame CreateEasingDoubleKeyFrame(double value, TimeSpan keyTime)
        {
            return new EasingDoubleKeyFrame(value, KeyTime.FromTimeSpan(keyTime));
        }

        /// <summary>
        /// ２値間の<see cref="DoubleAnimationUsingKeyFrames"/> を生成します。
        /// </summary>
        /// <param name="fromValue">初期値</param>
        /// <param name="fromKeyTime">初期値キー時刻</param>
        /// <param name="toValue">終了値</param>
        /// <param name="toKeyTime">終了値キー時刻</param>
        /// <returns><see cref="DoubleAnimationUsingKeyFrames"/> オブジェクト</returns>
        public static DoubleAnimationUsingKeyFrames CreateDoubleAnimationUsingKeyFrames(double fromValue,
                                                                                        TimeSpan fromKeyTime,
                                                                                        double toValue,
                                                                                        TimeSpan toKeyTime)
        {
            var frames = new DoubleAnimationUsingKeyFrames();
            frames.KeyFrames.Add(CreateEasingDoubleKeyFrame(fromValue, fromKeyTime));
            frames.KeyFrames.Add(CreateEasingDoubleKeyFrame(toValue, toKeyTime));
            return frames;
        }

        /// <summary>
        /// ２値間の<see cref="DoubleAnimationUsingKeyFrames"/> を生成します。
        /// </summary>
        /// <param name="target">キーフレームを設定する対象の依存関係オブジェクト</param>
        /// <param name="targetProperty">アニメーションを実行する依存関係プロパティ</param>
        /// <param name="fromValue">初期値</param>
        /// <param name="fromKeyTime">初期値キー時刻</param>
        /// <param name="toValue">終了値</param>
        /// <param name="toKeyTime">終了値キー時刻</param>
        /// <returns><see cref="DoubleAnimationUsingKeyFrames"/> オブジェクト</returns>
        public static DoubleAnimationUsingKeyFrames CreateDoubleAnimationUsingKeyFrames(DependencyObject target,
                                                                                        DependencyProperty targetProperty,
                                                                                        double fromValue,
                                                                                        TimeSpan fromKeyTime,
                                                                                        double toValue,
                                                                                        TimeSpan toKeyTime)
        {
            var frames = CreateDoubleAnimationUsingKeyFrames(fromValue, fromKeyTime, toValue, toKeyTime);
            Storyboard.SetTarget(frames, target);
            Storyboard.SetTargetProperty(frames, new PropertyPath(targetProperty));
            return frames;
        }

        /// <summary>
        /// フェードイン アニメーション ストーリーボードを生成します。
        /// </summary>
        /// <param name="target">キーフレームを設定する対象の依存関係オブジェクト</param>
        /// <param name="fromKeyTime">初期値キー時刻</param>
        /// <param name="toKeyTime">終了値キー時刻</param>
        /// <returns>フェードイン アニメーション ストーリーボード</returns>
        public static Storyboard CreateFadeInStoryboard(DependencyObject target, TimeSpan fromKeyTime, TimeSpan toKeyTime)
        {
            var frames = CreateDoubleAnimationUsingKeyFrames(target, UIElement.OpacityProperty, 0, fromKeyTime, 1, toKeyTime);
            frames.Freeze();
            var storyboard = new Storyboard();
            storyboard.Children.Add(frames);
            return storyboard;
        }

        /// <summary>
        /// フェードアウト アニメーション ストーリーボードを生成します。
        /// </summary>
        /// <param name="target">キーフレームを設定する対象の依存関係オブジェクト</param>
        /// <param name="fromKeyTime">初期値キー時刻</param>
        /// <param name="toKeyTime">終了値キー時刻</param>
        /// <returns>フェードアウト アニメーション ストーリーボード</returns>
        public static Storyboard CreateFadeOutStoryboard(DependencyObject target, TimeSpan fromKeyTime, TimeSpan toKeyTime)
        {
            var frames = CreateDoubleAnimationUsingKeyFrames(target, UIElement.OpacityProperty, 1, fromKeyTime, 0, toKeyTime);
            frames.Freeze();
            var storyboard = new Storyboard();
            storyboard.Children.Add(frames);
            return storyboard;
        }
    }
}