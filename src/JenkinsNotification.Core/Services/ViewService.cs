namespace JenkinsNotification.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using JenkinsNotification.Core.ComponentModels;
    using Logs;

    /// <summary>
    /// 画面表示サービス クラスです。
    /// </summary>
    /// <seealso cref="IViewService" />
    public class ViewService : IViewService
    {
        #region Const

        /// <summary>
        /// このアプリケーションの持つ<see cref="Window"/> を管理するマップ情報です。
        /// </summary>
        /// <remarks>
        /// Key:画面識別子,
        /// Value: ・Item1:Windowが存在するアセンブリ名 ・Item2:アセンブリ名を含めたWindowのフル型名
        /// </remarks>
        private static readonly Dictionary<ScreenKey, Tuple<string, string>> _viewMapping
            = new Dictionary<ScreenKey, Tuple<string, string>>();

        #endregion

        #region Fields

        /// <summary>
        /// このアプリケーションが表示している<see cref="Window"/> のキャッシュ
        /// </summary>
        private readonly Dictionary<ScreenKey, Window> _viewCash = new Dictionary<ScreenKey, Window>();

        #endregion

        #region Methods

        /// <summary>
        /// 指定した画面を閉じます。
        /// </summary>
        /// <param name="key">閉じる画面識別子</param>
        public void Close(ScreenKey key)
        {
            if (_viewCash.ContainsKey(key))
            {
                _viewCash[key].Close();
            }
        }

        /// <summary>
        /// 指定した画面識別子の<see cref="Window" /> を取得します。
        /// </summary>
        /// <param name="key">画面識別子</param>
        /// <returns>該当の画面オブジェクト<para />
        /// 該当する画面が表示されていない場合は、null を返します。</returns>
        public Window GetView(ScreenKey key)
        {
            return _viewCash.ContainsKey(key) ? _viewCash[key] : null;
        }

        /// <summary>
        /// このアプリケーションで使用する画面を登録します。
        /// </summary>
        /// <param name="key">画面識別子</param>
        /// <param name="viewType">画面(View) の型</param>
        public static void RegisterView(ScreenKey key, Type viewType)
        {
            if (!_viewMapping.ContainsKey(key))
            {
                var assemblyName = viewType.Assembly.FullName;
                var viewTypeName = viewType.FullName;
                _viewMapping.Add(key, Tuple.Create(assemblyName, viewTypeName));
                LogManager.Info($"画面識別子:{key} に{viewTypeName}(アセンブリ:{assemblyName})を追加しました。");
            }
        }

        /// <summary>
        /// 指定した画面を表示します。
        /// </summary>
        /// <param name="key">表示する画面識別子</param>
        /// <exception cref="System.ArgumentException">登録されている画面識別子<paramref name="key"/> が存在しない場合にスローされます。</exception>
        /// <exception cref="System.InvalidOperationException">画面識別子に登録されている画面の生成に失敗した場合にスローされます。</exception>
        public void Show(ScreenKey key)
        {
            if (!_viewMapping.ContainsKey(key))
            {
                throw new ArgumentException($"指定したScreenKeyは画面に登録されていません。({key})", nameof(key));
            }

            if (_viewCash.ContainsKey(key))
            {
                LogManager.Info($"{key} 画面を再表示する。");
                _viewCash[key].Activate();
                return;
            }

            var value = _viewMapping[key];
            var view = Activator.CreateInstance(value.Item1, value.Item2).Unwrap() as Window;
            if (view == null)
            {
                throw new InvalidOperationException($"指定したViewは存在しません。({value.Item1} / {value.Item2})");
            }

            view.Loaded += View_Loaded;
            view.Closed += View_OnClosed;
            _viewCash.Add(key, view);

            LogManager.Info($"{key} 画面を表示する。");
            view.Show();
        }

        /// <summary>
        /// 画面がロードされた際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            var view = sender as Window;
            var viewModel = view?.DataContext as ShellViewModelBase;
            viewModel?.Loaded();
        }

        /// <summary>
        /// 画面が閉じられたときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void View_OnClosed(object sender, EventArgs e)
        {
            var view = sender as Window;
            if (view != null)
            {
                view.Loaded -= View_Loaded;
                view.Closed -= View_OnClosed;

                var vm = view.DataContext as ShellViewModelBase;
                vm?.Unloaded();

                var pair = _viewCash.FirstOrDefault(x => Equals(x.Value, view));
                _viewCash.Remove(pair.Key);
            }
        }

        #endregion
    }
}